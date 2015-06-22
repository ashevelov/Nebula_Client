#define USE_LOCAL_SERVER
using Common;
using ExitGames.Client.Photon;
using Game.Space;
using Nebula.Client;
using Nebula.Client.Inventory;
using Nebula.Client.Mail;
using Nebula.Mmo.Games.Strategies;
using Nebula.Mmo.Items;
using Nebula.UI;
using ServerClientCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula.Mmo.Games {
    public class NetworkGame : BaseGame
    {
        private static NetworkGame instance = null;

        public static NetworkGame Instance() {
            if(instance == null ) {
                instance = MmoEngine.Get.NebulaGame;
            }
            return instance;
        }

        private MyItem _avatar;
        private readonly Dictionary<byte, Dictionary<string, Item>> _itemCache = new Dictionary<byte, Dictionary<string, Item>>();
        private readonly Dictionary<byte, InterestArea> interestAreas = new Dictionary<byte, InterestArea>();
        
        private readonly ClientInventory _inventory;
        private readonly WorldTransitionInfo _worldTransitionInfo;
        private readonly ActorBonuses actorBonuses;
        private readonly IServiceMessageReceiver serviceMessageReceiver;
        private readonly ClientWorkhouseStation station;
        private readonly ClientWorld clientWorld;
        //private readonly ClientWorldEventConnection worldEventConnection;
        private readonly ClientPlayerSkills skills;
        private readonly ClientShipCombatStats combatStats;
        private readonly DebugClientActionExecutor debugRPCExecutor;
        //private readonly ClientMailBox mailBox = new ClientMailBox();
        private readonly ClientCooperativeGroup cooperativeGroup = new ClientCooperativeGroup();
        private readonly ClientSearchGroupsResult searchGroupsResult = new ClientSearchGroupsResult();
        private Vector3 lastPosition = Vector3.zero;
        private PlayerShip _ship;
        private readonly ClientPlayerInfo playerInfo;

        private ServerInfo connectionServer = null;
        private NebulaGameDisconnectAction disconnectAction = NebulaGameDisconnectAction.None;
        
        public NetworkGame(MmoEngine engine, Settings settings)
            : base(engine, settings)
        {
            //_chat = new Chat(Settings.MAX_CHAT_MESSAGES_COUNT);
            //_currentObjectContainer = new CurrentObjectContainer();
            _inventory = new ClientInventory(10);
            _worldTransitionInfo = new WorldTransitionInfo(settings.DefaultZones[Race.Humans], settings.DefaultZones[Race.Humans]);
            actorBonuses = new ActorBonuses();
            serviceMessageReceiver = new ServiceMessageReceiver(100);
            this.station = new ClientWorkhouseStation(new Hashtable());
            this.playerInfo = new ClientPlayerInfo();

            this.playerInfo.SetExpChanged((oldExp, newExp) =>
                {
                    if (this.CurrentStrategy == GameState.NebulaGameWorldEntered && (oldExp != newExp))
                    {
                        //G.UI.PlayerInfo.OnExpChanged(oldExp, newExp);
                        if (MainCanvas.Get == null) {
                            return;
                        }
                        if (MainCanvas.Get.Exists(CanvasPanelType.ControlHUDView)) {
                            var hud = MainCanvas.Get.GetView(CanvasPanelType.ControlHUDView);
                            if (hud) {
                                hud.GetComponentInChildren<ControlHUDView>().OnExpChanged(oldExp, newExp);
                            }
                        }
                    }
                });
            //this.userInfo = new ClientUserInfo();
            this.clientWorld = new ClientWorld();
            //this.worldEventConnection = new ClientWorldEventConnection();
            this.skills = new ClientPlayerSkills();
            this.combatStats = new ClientShipCombatStats();
            this.debugRPCExecutor = new DebugClientActionExecutor(this);

            strategies = new Dictionary<GameState, IGameStrategy> {
                {GameState.NebulaGameChangingWorld, new NebulaGameChangingWorldStrategy() },
                {GameState.NebulaGameConnected, new NebulaGameConnectedStrategy()  },
                {GameState.NebulaGameDisconnected, new NebulaGameDisconnectedStrategy()  },
                {GameState.NebulaGameWaitingConnect, new NebulaGameWaitingConnectStrategy() },
                {GameState.NebulaGameWorkshopEntered, new NebulaGameWorkshopEnteredStrategy() },
                {GameState.NebulaGameWorldEntered, new NebulaGameWorldEnteredStrategy() }
            };
            SetStrategy(GameState.NebulaGameDisconnected);
        }

        public void CreateAvatar(string gameRefId) {
            _avatar = new MyItem(gameRefId, (byte)ItemType.Avatar, this, "", new object[] { });
            _ship = new PlayerShip(_avatar);
            _avatar.AddVisibleInterestArea(0);
            AddItem(_avatar);
            interestAreas[0] = new InterestArea(0, this, _avatar);
            interestAreas[0].ResetViewDistance();

            _avatar.SetInterestAreaAttached(true);
        }

        public void RemoveAvatar() { _avatar = null; }

        public void EnterWorld(string fromWorldId, string toWorldId) {

            if(Avatar == null ) {
                CreateAvatar(Engine.LoginGame.GameRefId);
            }

            if(string.IsNullOrEmpty(toWorldId)) {
                throw new NebulaException("target transition world must be not null");
            }

            WorldTransition.SetPrevAndNextWorld(fromWorldId, toWorldId);
            if(Avatar != null ) {
                Avatar.DestroyView();
            }
            Engine.GameData.SetNewWorld(WorldTransition.NextWorld, Settings.WorldCornerMin, Settings.WorldCornerMax, Settings.TileDimensions, LevelType.Space);
            ClearItemCache();
            AddItem(Avatar);

            var position = new float[] { 0.0f, 0.0f, Settings.START_Z };
            Avatar.SetPositions(position, position, null, null, 0);

            SetStrategy(GameState.NebulaGameChangingWorld);

            var properties = new Hashtable {
                {(byte)PS.InterestAreaAttached, this.Avatar.InterestAreaAttached },
                {(byte)PS.ViewDistanceEnter, this.Settings.ViewDistanceEnter },
                {(byte)PS.ViewDistanceExit, this.Settings.ViewDistanceExit }
            };
            Operations.EnterWorld(this,
                Engine.GameData.World.Name,
                properties,
                Avatar.Position,
                Avatar.Rotation,
                Settings.ViewDistanceEnter,
                Settings.ViewDistanceExit,
                Engine.LoginGame.GameRefId,
                Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacterId,
                (Workshop)Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().HomeWorkshop,
                (Race)Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Race,
                Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().ModelHash(),
                Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().CharacterName,
                Engine.LoginGame.login);
        }


        public void EnterWorkshop(WorkshopStrategyType workshopStrategyType) {
            if(this.CurrentStrategy == GameState.NebulaGameWorkshopEntered) {
                throw new NebulaException("We alredy in workshop");
            }
            SetStrategy(GameState.NebulaGameWorkshopEntered);
            Operations.EnterWorkshop(this, this.Avatar.Id, workshopStrategyType);
        }


        public override void SetStrategy(GameState state) {
            GameState old = GameState.NebulaGameDisconnected;
            if (activeStrategy != null) {
                old = CurrentStrategy;
            }
            base.SetStrategy(state);
            if (old != CurrentStrategy) {
                Events.OnGameStateChanged(old, CurrentStrategy);
                NetworkGame.OnStrategyChanged(old, CurrentStrategy);
            }
        }

        public override void Connect(string ipAddress, int port, string application) {
            throw new NebulaException("Calling restriced by game logic");
        }

        public void Connect() {
            if(connectionServer == null ) {
                throw new NebulaException("not exists connection server");
            }
            SetStrategy(GameState.NebulaGameWaitingConnect);
            base.Connect(connectionServer.IpAddress, connectionServer.Port, connectionServer.Application);
        }


        public void SetConnectionServer(ServerInfo serverInfo) {
            connectionServer = serverInfo;
        }

        public void SetDisconnectAction(NebulaGameDisconnectAction onDisconnect) {
            disconnectAction = onDisconnect;
        }

        public string HomeWorldForCharacter() {
            if(Engine.SelectCharacterGame.PlayerCharacters.HasSelectedCharacter()) {
                try {
                    var race = (Race)Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Race;
                    return Settings.DefaultZones[race];
                }catch(KeyNotFoundException exc) {
                    Debug.LogErrorFormat("key not found for race {0}", 
                        (Race)(byte)Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Race);
                }
            }
            return this.Settings.DefaultZones[Race.Humans];
        }



        public void OnTargetUpdated() {
            if (this.Avatar == null) {
                if (MainCanvas.Get) {
                    MainCanvas.Get.SetTarget(null);
                }
                return;
            }
            if (!this.Avatar.Target.HasTarget) {
                if (MainCanvas.Get) {
                    MainCanvas.Get.SetTarget(null);
                }
                return;
            }
            if (this.Avatar.Target.Item == null) {
                if (MainCanvas.Get) {
                    MainCanvas.Get.SetTarget(null);
                }
                return;
            }
            if (!(this.Avatar.Target.Item is IObjectInfo)) {
                if (MainCanvas.Get) {
                    MainCanvas.Get.SetTarget(null);
                }
                return;
            }
            if (MainCanvas.Get) {
                MainCanvas.Get.SetTarget(this.Avatar.Target.Item as IObjectInfo);
            }
        }

        //public List<ClientWorldEventInfo> ActiveWorldEvents()
        //{
        //    return this.worldEventConnection.ActiveEvents();
        //}


        #region Properties
        public MyItem Avatar
        {
            get { return _avatar; }
        }

        public string AvatarId
        {
            get
            {
                return (this.Avatar != null) ? this.Avatar.Id : string.Empty;
            }
        }

        public Dictionary<byte, Dictionary<string, Item>> Items
        {
            get { return _itemCache; }
        }

        public List<Item> FlatItems
        {
            get
            {
                List<Item> items = new List<Item>();
                foreach (var pair in _itemCache)
                {
                    foreach (var pair2 in pair.Value)
                    {
                        items.Add(pair2.Value);
                    }
                }
                return items;
            }
        }


        public ClientInventory Inventory
        {
            get
            {
                return _inventory;
            }
        }

        public ActorBonuses Bonuses
        {
            get
            {
                return actorBonuses;
            }
        }

        public bool ExistAvatarView
        {
            get
            {
                return (this.Avatar != null) && (this.Avatar.ExistsView);
            }
        }

        /// <summary>
        /// Contains player info( level, exp, group, workshop, etc). Updated from server
        /// </summary>
        public ClientPlayerInfo PlayerInfo
        {
            get
            {
                return this.playerInfo;
            }
        }

        /// <summary>
        /// Hold information about previous and next world which player moves
        /// </summary>
        public WorldTransitionInfo WorldTransition
        {
            get
            {
                return _worldTransitionInfo;
            }
        }

        public IServiceMessageReceiver ServiceMessageReceiver
        {
            get
            {
                return serviceMessageReceiver;
            }
        }

        public PlayerShip Ship
        {
            get
            {
                return _ship;
            }
        }

        public ClientWorkhouseStation Station
        {
            get
            {
                return this.station;
            }
        }

        //public LoginInfo LoginInfo
        //{
        //    get
        //    {
        //        return this.loginInfo;
        //    }
        //}

        public ClientWorld ClientWorld
        {
            get
            {
                return this.clientWorld;
            }
        }

        //public ClientWorldEventConnection WorldEventConnection
        //{
        //    get
        //    {
        //        return this.worldEventConnection;
        //    }
        //}

        public ClientPlayerSkills Skills
        {
            get
            {
                return this.skills;
            }
        }

        public ClientShipCombatStats CombatStats
        {
            get
            {
                return this.combatStats;
            }
        }

        public DebugClientActionExecutor DebugRPC
        {
            get
            {
                return this.debugRPCExecutor;
            }
        }

        public override GameType GameType {
            get {
                return GameType.Game;
            }
        }

        //public ClientMailBox MailBox()
        //{
        //    return this.mailBox;
        //}

        public ClientSearchGroupsResult SearchGroupsResult() {
            return this.searchGroupsResult;
        }

        public ClientCooperativeGroup CooperativeGroup() {
            return this.cooperativeGroup;
        }

        public ServerInfo ConnectionServer {
            get {
                return connectionServer;
            }
        }

        public NebulaGameDisconnectAction DisconnectAction {
            get {
                return disconnectAction;
            }
        }
        #endregion

        public bool IsSelectedCharacter(string characterId) {
            return (Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacterId == characterId);
        }

        public string CharacterId() {
            return Engine.SelectCharacterGame.PlayerCharacters.SelectedCharacterId;
        }

        public ClientPlayerSkill PlayerSkill(int index)
        {
            return this.Skills.Skill(index);
        }



        public float ShipEnergy()
        {
            return this.Ship.Energy;
        }


        public void SetPlayerInfo(Hashtable info)
        {
            this.PlayerInfo.ParseInfo(info);
        }

        public void SetSkills(Hashtable skillsInfo)
        {
            this.Skills.ParseInfo(skillsInfo);
        }

        public void SetShipDestroyed(byte itemType, string itemId, bool shipDestroyed)
        {
            Item item;
            if (TryGetItem(itemType, itemId, out item))
            {
                item.SetShipDestroyed(shipDestroyed);
            }
        }


        public void AddItem(Item item)
        {
            Dictionary<string, Item> typedItems;
            if (Items.TryGetValue(item.Type, out typedItems) == false)
            {
                typedItems = new Dictionary<string, Item>();
                Items.Add(item.Type, typedItems);
            }
            typedItems.Add(item.Id, item);
            Engine.CreateActor(this, item);
        }

        public void OnCameraAttached(string itemId, byte itemType)
        {
            _avatar.AddVisibleInterestArea(0);
        }

        public void OnCameraDetached()
        {
            _avatar.RemoveVisibleInterestArea(0);
        }

        public void OnItemSpawned(byte itemType, string itemId)
        {
        }

        public void OnItemFire(Hashtable properties)
        {
            
            string sourceId = (string)properties[(int)SPC.Source];
            byte sourceType = (byte)properties[(int)SPC.SourceType];
            string targetId = (string)properties[(int)SPC.Target];
            byte targetType = (byte)properties[(int)SPC.TargetType];

            if(sourceId == AvatarId) {
                Debug.Log("PLAYER SHOT RECEIVED");
            }

            Item sourceItem = null;
            Item targetItem = null;

            if (string.IsNullOrEmpty(sourceId) == false) {
                if (TryGetItem(sourceType, sourceId, out sourceItem) == false) {
                    return;
                }
            } else {

            }
            if (string.IsNullOrEmpty(targetId) == false) {
                if (TryGetItem(targetType, targetId, out targetItem) == false) {
                    return;
                }
            }

            if (sourceItem != null && sourceItem.View && targetItem != null && targetItem.View) {

                sourceItem.Component.Fire(targetItem.Component, properties);
            }
        }

        public bool RemoveItem(Item item)
        {
            Dictionary<string, Item> typedItems;
            if (_itemCache.TryGetValue(item.Type, out typedItems))
            {
                if (typedItems.Remove(item.Id))
                {
                    if (typedItems.Count == 0)
                    {
                        _itemCache.Remove(item.Type);
                    }
                    if(item.ExistsView && item.View) {
                        item.DestroyView();
                    }
                    return true;
                }
            }
            return false;
        }

        public void ClearItemCache()
        {
            _itemCache.Clear();
        }


        public bool TryGetItem(byte itemType, string itemid, out Item item)
        {
            Dictionary<string, Item> typedItems;
            if (this._itemCache.TryGetValue(itemType, out typedItems))
            {
                return typedItems.TryGetValue(itemid, out item);
            }

            item = null;
            return false;
        }

        public override void Update() {
            base.Update();
            UpdateProperties();

            if(Peer != null && CurrentStrategy == GameState.NebulaGameWorldEntered) {
                if(Peer.QueuedIncomingCommands > 10 ) {
                    Debug.Log("DISPATCH INCOMING COMMANDS");
                    Peer.DispatchIncomingCommands();
                }
            }
        }


        private float nextUpdateModelTime;
        private float nextUpdateModelTimeInWorkshop;
        private float nextUpdateWeaponTime;
        private float nextUpdateInventoryTime;
        private float nextUpdatePlayerInfoTime;
        private float nextUpdateSkillsTime;
        private float nextUpdateEventsTime;
        private float nextChatUpdate;
        private float nextPlayerPropertiesUpdate;
        private float nextPlayerBonusesUpdate;
        private float nextOtherItemsUpdate;
        private float nextRequestOfCombatParams;
        private float nextStationUpdate;

        void UpdateProperties()
        {
            var currentState = CurrentStrategy;

            if(currentState == GameState.NebulaGameWorldEntered) {
                if(Time.time > nextUpdateModelTime) {
                    nextUpdateModelTime = Time.time + Settings.REQUEST_PLAYER_MODEL_INTERVAL_AT_WORLD;
                    if(Avatar != null && Ship != null && (!Ship.ShipModel.HasAllModules()) ) {
                        NRPC.RequestShipModel();
                    }
                }
            }
            if(currentState == GameState.NebulaGameWorkshopEntered) {
                if(Time.time > nextUpdateModelTimeInWorkshop) {
                    nextUpdateModelTimeInWorkshop = Time.time + Settings.REQUEST_PLAYER_MODEL_INTERVAL_AT_WORKSHOP;
                    NRPC.RequestShipModel();
                }
                if(Time.time > nextStationUpdate) {
                    nextStationUpdate = Time.time + Settings.REQUEST_WORKSHOP_INTERVAL;
                    NRPC.RequestStation();
                }
            }

            if(currentState == GameState.NebulaGameWorldEntered || currentState == GameState.NebulaGameWorkshopEntered) {
                if(Time.time > nextRequestOfCombatParams) {
                    nextRequestOfCombatParams = Time.time + Settings.REQUEST_COMBAT_PARAMS_INTERVAL;
                    NRPC.RequestCombatStats();
                }
                if(Time.time > nextUpdateWeaponTime) {
                    nextUpdateWeaponTime = Time.time + Settings.REQUEST_WEAPON_INTERVAL;
                    NRPC.RequestWeapon();
                }
                if(Time.time > nextUpdateInventoryTime) {
                    nextUpdateInventoryTime = Time.time + Settings.INVENTORY_UPDATE_INTERVAL;
                    NRPC.RequestInventory();
                }
                if (Time.time > this.nextUpdatePlayerInfoTime) {
                    this.nextUpdatePlayerInfoTime = Time.time + Settings.REQUEST_PLAYER_INFO_INTERVAL; ;
                    NRPC.RequestPlayerInfo();
                }
                if (Time.time > this.nextUpdateSkillsTime) {
                    this.nextUpdateSkillsTime = Time.time + Settings.REQUEST_SKILLS_INTERVAL;
                    NRPC.RequestSkills();
                }
                //if (Time.time > this.nextUpdateEventsTime) {
                //    this.nextUpdateEventsTime = Time.time + Settings.REQUEST_EVENTS_INTERVAL;
                //    NRPC.RequestEvents();
                //}
                if (Time.time > this.nextChatUpdate) {
                    this.nextChatUpdate = Time.time + Settings.CHAT_UPDATE_INTERVAL;
                    NRPC.GetChatUpdate();
                }

                if (this.Avatar != null) {
                    if (Time.time > nextPlayerPropertiesUpdate) {
                        if (this.Avatar.ShipDestroyed)
                            Debug.Log("Player avatar ship destroyed");

                        nextPlayerPropertiesUpdate = Time.time + 1f;
                        //Dbg.Print("request properties", "PLAYER");
                        this.Avatar.GetProperties();
                    }

                    if (Time.time > nextPlayerBonusesUpdate) {
                        nextPlayerBonusesUpdate = Time.time + 0.3f;
                        this.Avatar.GetBonuses();
                    }
                } 
            }

            if(currentState == GameState.NebulaGameWorldEntered) {
                if (Time.time > nextOtherItemsUpdate) {
                    nextOtherItemsUpdate = Time.time + 1;
                    if (this._itemCache.ContainsKey(ItemType.Avatar.toByte())) {
                        Dictionary<string, Item> avatars = this._itemCache[ItemType.Avatar.toByte()];
                        foreach (var p2 in avatars) {
                            if (p2.Value != null) {
                                if (p2.Value.IsMine == false) {
                                    p2.Value.AdditionalUpdate();
                                }
                            }
                        }
                    }

                    Dictionary<string, Item> botItems = null;
                    if (this._itemCache.TryGetValue(ItemType.Bot.toByte(), out botItems)) {
                        foreach (var pBots in botItems) {
                            pBots.Value.AdditionalUpdate();
                        }
                    }
                }
            }
        }

        public string CurrentWorldId()
        {
            return Engine.GameData.World.Name;
        }




        //=========================================HELPER FUNCTIONS==========================================================
        #region HELPER FUNCTIONS
        public static void OnShipModelUpdated(NetworkGame game, Hashtable modelInfo)
        {
            //Debug.Log(modelInfo.ToStringBuilder().ToString());
            Hashtable slots = modelInfo.GetValue<Hashtable>((int)SPC.Info, new Hashtable());
            //Dbg.PrintArray("SLOTS ARRAY", slots);

            foreach (DictionaryEntry entry in slots)
            {
                switch ((ShipModelSlotType)(byte)entry.Key)
                {
                    case ShipModelSlotType.CB:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>((int)SPC.Info, new Hashtable());

                            if (moduleHashtable != null && moduleHashtable.Count > 0)
                            {
                                ClientShipModule module = new ClientShipModule(moduleHashtable);
                                game.Ship.ShipModel.SetModule(module);
                            }
                            else
                            {
                                game.Ship.ShipModel.RemoveModule(ShipModelSlotType.CB);
                            }
                        }
                        break;
                    case ShipModelSlotType.CM:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>((int)SPC.Info, new Hashtable());

                            if (moduleHashtable != null && moduleHashtable.Count > 0)
                            {
                                ClientShipModule module = new ClientShipModule(moduleHashtable);
                                game.Ship.ShipModel.SetModule(module);
                            }
                            else
                            {
                                game.Ship.ShipModel.RemoveModule(ShipModelSlotType.CM);
                            }
                        }
                        break;
                    case ShipModelSlotType.DF:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>((int)SPC.Info, new Hashtable());

                            if (moduleHashtable != null && moduleHashtable.Count > 0)
                            {
                                ClientShipModule module = new ClientShipModule(moduleHashtable);
                                game.Ship.ShipModel.SetModule(module);
                            }
                            else
                            {
                                game.Ship.ShipModel.RemoveModule(ShipModelSlotType.DF);
                            }
                        }
                        break;
                    case ShipModelSlotType.DM:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>((int)SPC.Info, new Hashtable());

                            if (moduleHashtable != null && moduleHashtable.Count > 0)
                            {
                                ClientShipModule module = new ClientShipModule(moduleHashtable);
                                game.Ship.ShipModel.SetModule(module);
                            }
                            else
                            {
                                game.Ship.ShipModel.RemoveModule(ShipModelSlotType.DM);
                            }
                        }
                        break;
                    case ShipModelSlotType.ES:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>((int)SPC.Info, new Hashtable());

                            if (moduleHashtable != null && moduleHashtable.Count > 0)
                            {
                                ClientShipModule module = new ClientShipModule(moduleHashtable);
                                game.Ship.ShipModel.SetModule(module);
                            }
                            else
                            {
                                game.Ship.ShipModel.RemoveModule(ShipModelSlotType.ES);
                            }
                        }
                        break;
                }
            }
        }

        public static void OnStationHoldUpdated(NetworkGame game, Hashtable holdInfo)
        {
            game.Station.LoadInfo(holdInfo);
        }

        /// <summary>
        /// Called when received CustomEventCode.InventoryUpdated when strategy Workshop
        /// </summary>
        /// <param name="game"></param>
        /// <param name="inventoryInfo"></param>
        public static void OnInventoryUpdated(NetworkGame game, Hashtable inventoryInfo)
        {
            ClientInventory clientInv = new ClientInventory(inventoryInfo);
            game.Inventory.Replace(clientInv);


        }

        /// <summary>
        /// Called when changed current game strategy
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        public static void OnStrategyChanged(GameState oldState, GameState newState)
        {
            if (newState == GameState.NebulaGameWorldEntered) {
                if (MainCanvas.Get != null) {
                    MainCanvas.Get.Show(CanvasPanelType.ControlHUDView);
                    MainCanvas.Get.Show(CanvasPanelType.MenuHUDView);
                    MainCanvas.Get.Show(CanvasPanelType.ChatView);
                    //MainCanvas.Get.Show(CanvasPanelType.EventTasksView);
                }
            } else {
                if (MainCanvas.Get != null) {
                    MainCanvas.Get.Destroy(CanvasPanelType.ControlHUDView);
					MainCanvas.Get.Destroy(CanvasPanelType.MenuHUDView);
					MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
					MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);
					MainCanvas.Get.Destroy(CanvasPanelType.TargetObjectView);
                    //MainCanvas.Get.Destroy(CanvasPanelType.EventTasksView);
                }
            }
        }

        public void OnWeaponReceived(Hashtable weaponInfo)
        {
            this.Ship.Weapon.ParseInfo(weaponInfo);
        }

        public void SetSpawnPosition(Item item)
        {
            if (item.View && item.Race != Common.Race.None)
            {
                SpawnPoint point = null;
                var spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
                foreach (var sp in spawnPoints)
                {
                    if (sp.race == item.Race)
                    {
                        point = sp;
                        break;
                    }
                }

                if (point)
                {
                    item.View.transform.position = point.transform.position;
                }
            }
        }
        #endregion
        //===================================================================================================================


        public static void ChangeWorld(string targetWorld) {
            var game = NetworkGame.Instance();
            if(game.ConnectionServer.ContainsLocation(targetWorld)) {
                game.SetDisconnectAction(NebulaGameDisconnectAction.None);
                game.WorldTransition.SetNextWorld(targetWorld);
                Operations.ExitWorld(game);
            } else {
                var newServer = MmoEngine.Get.MasterGame.GetGameServer(targetWorld);
                if(newServer == null ) {
                    throw new NebulaException(string.Format("Not found server for world {0}", targetWorld));
                }
                game.SetDisconnectAction(NebulaGameDisconnectAction.ChangeWorld);
                game.WorldTransition.SetNextWorld(targetWorld);
                game.SetConnectionServer(newServer);
                game.Peer.Disconnect();
            }
        }

        public static void SetRandomBuff() {
            Operations.ExecAction(MmoEngine.Get.NebulaGame, MmoEngine.Get.NebulaGame.AvatarId, "SetRandomBonus", new object[] { });
        }
    }


}







