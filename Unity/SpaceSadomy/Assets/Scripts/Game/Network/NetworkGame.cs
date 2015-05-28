#define USE_LOCAL_SERVER
using Common;
using ExitGames.Client.Photon;
using Game.Space;
using Game.Space.UI;
using Nebula;
using Nebula.Client;
using Nebula.Client.Inventory;
using Nebula.Client.Mail;
using Nebula.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Nebula
{
    public class NetworkGame : IPhotonPeerListener
    {

        public const float CHAT_UPDATE_INTERVAL = 1.0f;
        public const float START_Z = 0;
        public const int MAX_CHAT_MESSAGES_COUNT = 100;
        public const float INVENTORY_UPDATE_INTERVAL = 5.0f;
        public const float EVENTS_UPDATE_INTERVAL = 5.0f;
        public const float BUFFS_UPDATE_INTERVAL = 2.0f;
        public const float STATION_UPDATE_INTERVAL = 5.0f;


        private MyItem _avatar;
        //private readonly Dictionary<byte, InterestArea> _areas = new Dictionary<byte, InterestArea>();
        private readonly Dictionary<byte, Dictionary<string, Item>> _itemCache = new Dictionary<byte, Dictionary<string, Item>>();
        private readonly MmoEngine _engine;
        private readonly Settings _settings;
        private int outgoingOperationCount;
        private PhotonPeer _peer;
        private IGameLogicStrategy m_stateStrategy;
        private WorldData _world;
        private readonly Chat _chat;
        private readonly CurrentObjectContainer _currentObjectContainer;
        private readonly ClientInventory _inventory;
        private readonly WorldTransitionInfo _worldTransitionInfo;
        private readonly ActorBonuses actorBonuses;
        private readonly IServiceMessageReceiver serviceMessageReceiver;
        private readonly ClientWorkhouseStation station;
        private readonly ClientUserInfo userInfo;
        private readonly ClientWorld clientWorld;
        private readonly ClientWorldEventConnection worldEventConnection;
        private readonly ClientPlayerSkills skills;
        private readonly ClientShipCombatStats combatStats;
        private readonly DebugClientActionExecutor debugRPCExecutor;
        private readonly ClientMailBox mailBox = new ClientMailBox();
        private readonly ClientCooperativeGroup cooperativeGroup = new ClientCooperativeGroup();
        private readonly ClientSearchGroupsResult searchGroupsResult = new ClientSearchGroupsResult();

        private Vector3 lastPosition = Vector3.zero;


        private LoginInfo loginInfo;
        private PlayerShip _ship;

        //contains info about player( level, exp, current group etc.)
        private readonly ClientPlayerInfo playerInfo;


        public void OnTargetUpdated()
        {
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

        private void ChangeStrategy(IGameLogicStrategy newStrategy)
        {
            IGameLogicStrategy oldStrategy = m_stateStrategy;
            m_stateStrategy = newStrategy;
            if (oldStrategy != null && (oldStrategy.State != m_stateStrategy.State))
            {
                Events.OnGameStateChanged(oldStrategy.State, m_stateStrategy.State);
                NetworkGame.OnStrategyChanged(oldStrategy.State, m_stateStrategy.State);
            }
            Debug.LogFormat("<color=orange>New Strategy: {0}</color>", newStrategy.State);
        }

        public void DeleteAvatar()
        {
            _avatar = null;
        }


        public NetworkGame(MmoEngine engine, Settings settings, string avatarName)
        {
            _engine = engine;
            _settings = settings;
            _chat = new Chat(MAX_CHAT_MESSAGES_COUNT);
            _currentObjectContainer = new CurrentObjectContainer();
            _inventory = new ClientInventory(10);
            _worldTransitionInfo = new WorldTransitionInfo(settings.DefaultZones[Race.Humans], settings.DefaultZones[Race.Humans]);
            actorBonuses = new ActorBonuses();
            serviceMessageReceiver = new ServiceMessageReceiver(100);
            this.station = new ClientWorkhouseStation(new Hashtable());
            this.playerInfo = new ClientPlayerInfo();
            this.playerInfo.SetExpChanged((oldExp, newExp) =>
                {
                    if (this.State == GameState.WorldEntered && (oldExp != newExp))
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
            this.userInfo = new ClientUserInfo();
            this.clientWorld = new ClientWorld();
            this.worldEventConnection = new ClientWorldEventConnection();
            this.skills = new ClientPlayerSkills();
            this.combatStats = new ClientShipCombatStats();
            this.debugRPCExecutor = new DebugClientActionExecutor(this);
            ChangeStrategy(Disconnected.Instance);
        }

        public void Login(string login, string password, string displayName)
        {
            //m_stateStrategy = Photon.Mmo.Client.GameStateStrategies.Login.Instance;
            ChangeStrategy(Nebula.Login.Instance);
            Operations.Login(this, login, password, displayName);
        }

        public void EnterWorldWithSelectedCharacter()
        {
            if (this.userInfo.HasSelectedCharacter())
            {
                this.WorldTransition.SetNextWorld(HomeWorldForCharacter());
                this.WorldTransition.SetNextWorld(HomeWorldForCharacter());
                EnterWorld();
            }
        }


        private void EnterWorld() {
            if(this.Avatar != null) {
                this.Avatar.DestroyView();
            }
            if (this.WorldTransition.HasNextWorld()) {
                WorldData nextWorld = new WorldData();
                nextWorld.SetWorldParameters(this.WorldTransition.NextWorld, this.Settings.WorldCornerMin, this.Settings.WorldCornerMax, this.Settings.TileDimensions, LevelType.Space);
                this.SetWorld(nextWorld);
                this.ClearItemCache();
                this.AddItem(this.Avatar);
            }else {
                if (this.HasWorld) {
                    this.World.SetWorldParameters(HomeWorldForCharacter(), LevelType.Space);
                } else {
                    WorldData world = new WorldData();
                    world.SetWorldParameters(HomeWorldForCharacter(), this.Settings.WorldCornerMin, this.Settings.WorldCornerMax, this.Settings.TileDimensions, LevelType.Space);
                    this.SetWorld(world);
                }
            }
            this.OperationEnterWorld();
        }

        private string HomeWorldForCharacter() {
            if (this.UserInfo != null && this.UserInfo.HasSelectedCharacter()) {
                try {
                    var race = (Race)(byte)this.UserInfo.GetCharacter(this.UserInfo.SelectedCharacterId).Race;
                    return this.Settings.DefaultZones[race];
                }catch(KeyNotFoundException exc) {
                    Debug.LogError(string.Format("key not found for race: {0}", (Race)(byte)this.UserInfo.GetCharacter(this.UserInfo.SelectedCharacterId).Race));
                }
            }
            return this.Settings.DefaultZones[Race.Humans];
        }

        public void OperationEnterWorld() {
            var position = new float[] { 0.0f, 0.0f, NetworkGame.START_Z };
            this.Avatar.SetPositions(position, position, null, null, 0);
            var properties = new Hashtable {
                {Props.DEFAULT_STATE_INTEREST_AREA_ATTACHED, this.Avatar.InterestAreaAttached },
                {Props.DEFAULT_STATE_VIEW_DISTANCE_ENTER, this.Settings.ViewDistanceEnter },
                {Props.DEFAULT_STATE_VIEW_DISTANCE_EXIT, this.Settings.ViewDistanceExit }
            };
            Dictionary<string, Hashtable> dictProps = new Dictionary<string, Hashtable> {
                {GroupProps.DEFAULT_STATE, properties }
            };
            Operations.EnterWorld(this, this.World.Name, dictProps, this.Avatar.Position, this.Avatar.Rotation, this.Settings.ViewDistanceEnter,
                this.Settings.ViewDistanceExit, this.LoginInfo.gameRefId);
        }



        public void OnLoginCompleted(LoginInfo loginInfo)
        {
            this.loginInfo = loginInfo;
            CreateCharacter(loginInfo);
            this.ChangeStrategy(SelectCharacterStrategy.Instance);
            Application.LoadLevel("select_character");
            //request current user info
            NRPC.RequestUserInfo();
        }

        private void CreateCharacter(LoginInfo loginInfo)
        {
            if (_avatar == null)
            {
                _avatar = new MyItem(loginInfo.gameRefId, (byte)ItemType.Avatar, this, loginInfo.displayName);
                _ship = new PlayerShip(_avatar);
                _avatar.AddVisibleInterestArea(0);
                AddItem(_avatar);
                OperationResetViewDistance();
                _avatar.SetInterestAreaAttached(true);

            }
        }

        private void OperationResetViewDistance() {
            Operations.SetViewDistance(this, Settings.ViewDistanceEnter, Settings.ViewDistanceExit);
        }

        private void OperationRemoveInterestArea() {
            Operations.RemoveInterestArea(this, 0);
        }

        public bool TryRecreateCharacter()
        {
            if (this.loginInfo != null && _avatar == null)
            {
                CreateCharacter(this.loginInfo);

                return true;
            }
            return false;
        }



        public void Initialize(PhotonPeer photonPeer)
        {
            _peer = photonPeer;
            ChangeStrategy(WaitingForConnect.Instance);
            //_stateStrategy = WaitingForConnect.Instance;
            photonPeer.Connect(_settings.ServerAddress, _settings.ApplicationName);
        }

        public List<ClientWorldEventInfo> ActiveWorldEvents()
        {
            return this.worldEventConnection.ActiveEvents();
        }


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

        public bool HasWorld
        {
            get
            {
                return _world != null;
            }
        }

        public WorldData World
        {
            get
            {
                return _world;
            }
        }

        public MmoEngine Engine
        {
            get { return _engine; }
        }

        public PhotonPeer Peer
        {
            get { return _peer; }
        }

        public Settings Settings
        {
            get { return _settings; }
        }

        public GameState State
        {
            get { return m_stateStrategy.State; }
        }

        public Chat Chat
        {
            get
            {
                return _chat;
            }
        }

        public CurrentObjectContainer CurrentObjectContainer
        {
            get
            {
                return _currentObjectContainer;
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

        public LoginInfo LoginInfo
        {
            get
            {
                return this.loginInfo;
            }
        }


        public ClientUserInfo UserInfo
        {
            get
            {
                return this.userInfo;
            }
        }

        public ClientWorld ClientWorld
        {
            get
            {
                return this.clientWorld;
            }
        }

        public ClientWorldEventConnection WorldEventConnection
        {
            get
            {
                return this.worldEventConnection;
            }
        }

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

        public ClientMailBox MailBox()
        {
            return this.mailBox;
        }

        public ClientSearchGroupsResult SearchGroupsResult() {
            return this.searchGroupsResult;
        }

        public ClientCooperativeGroup CooperativeGroup() {
            return this.cooperativeGroup;
        }
        #endregion

        public bool IsSelectedCharacter(string characterId) {
            if (this.UserInfo == null) {
                return false;
            }
            return (this.UserInfo.SelectedCharacterId == characterId);
        }

        public string CharacterId() {
            if(this.UserInfo == null ) {
                return string.Empty;
            }
            return this.UserInfo.SelectedCharacterId;
        }
        public ClientWorldEventInfo GetEvent(string worldId, string eventId)
        {
            return this.WorldEventConnection.GetEvent(worldId, eventId);
        }

        public float ShipWeaponLightShotTimer01()
        {
            return this.Ship.WeaponLightShotTimer01();
        }

        public float ShipWeaponHeavyShotTimer01()
        {
            return this.Ship.WeaponHeavyShotTimer01();
        }

        public ClientPlayerSkill PlayerSkill(int index)
        {
            return this.Skills.Skill(index);
        }

        public float ShotEnergy(ShotType shotType)
        {
            return this.Ship.ShotEnergy(shotType);
        }

        public float ShipEnergy()
        {
            return this.Ship.Energy;
        }

        //public void RequestFireShot(ShotType shotType)
        //{
        //    this.Avatar.RequestMakeFire(shotType);
        //}

        //public void PlayerRequestUseSkill(int index)
        //{
        //    this.Avatar.RequestUseSkill(index);
        //}

        public void SetEvent(Hashtable eventInfo)
        {
            this.WorldEventConnection.SetEvent(eventInfo);
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

        public void SetWorld(WorldData world)
        {
            _world = world;
        }

        private void ReleaseWorld()
        {
            _world = null;
        }

        //public void AddCamera(InterestArea area)
        //{
        //    _areas.Add(area.Id, area);
        //}

        public void AddItem(Item item)
        {
            Dictionary<string, Item> typedItems;
            if (Items.TryGetValue(item.Type, out typedItems) == false)
            {
                typedItems = new Dictionary<string, Item>();
                Items.Add(item.Type, typedItems);
            }
            typedItems.Add(item.Id, item);
            _engine.CreateActor(this, item);
        }

        public void Disconnect()
        {
            _peer.Disconnect();
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
            string sourceId = (string)properties[GenericEventProps.source_id];
            byte sourceType = (byte)properties[GenericEventProps.source_type];
            string targetId = (string)properties[GenericEventProps.target_id];
            byte targetType = (byte)properties[GenericEventProps.target_type];

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

        public void OnUnexpectedEventReceive(EventData @event)
        {
        }

        public void OnUnexpectedOperationError(OperationResponse operationResponse)
        {
        }

        public void OnUnexpectedPhotonReturn(OperationResponse operationResponse)
        {
        }

        //public bool RemoveCamera(byte cameraID)
        //{
        //    return _areas.Remove(cameraID);
        //}

        //public void ClearCameras()
        //{
        //    _areas.Clear();
        //}

        public bool RemoveItem(Item item)
        {
            Dictionary<string, Item> typedItems;
            if (_itemCache.TryGetValue(item.Type, out typedItems))
            {
                if (typedItems.Remove(item.Id))
                {
                    Dbg.Print("<color=orange><b>item removed successfully</b></color>");
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

        public void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
        {
            this.m_stateStrategy.SendOperation(this, operationCode, parameter, sendReliable, channelId);

            // avoid operation congestion (QueueOutgoingUnreliableWarning)
            this.outgoingOperationCount++;
            if (this.outgoingOperationCount > 10)
            {
                this._peer.SendOutgoingCommands();
                this.outgoingOperationCount = 0;
            }
        }

        public void SetConnected()
        {
            ChangeStrategy(Connected.Instance);
        }

        public void SetWaitingForChangeWorld()
        {
            ChangeStrategy(WaitingForChangeWorld.Instance);
            if (!this.WorldTransition.HasNextWorld()) {
                return;
            }
            if (this.Avatar != null && this.Avatar.ExistsView) {
                this.Avatar.DestroyView();
            }
            WorldData nextWorld = new WorldData();
            nextWorld.SetWorldParameters(this.WorldTransition.NextWorld, this.Settings.WorldCornerMin, this.Settings.WorldCornerMax,
                this.Settings.TileDimensions, LevelType.Space);
            this.SetWorld(nextWorld);
            this.ClearItemCache();
            this.AddItem(this.Avatar);
            OperationEnterWorld();
        }

        public void SetSelectCharacter() {
            ChangeStrategy(SelectCharacterStrategy.Instance);

            if (this.Avatar != null && this.Avatar.ExistsView) {
                this.Avatar.DestroyView();
            }
            this.ClearItemCache();
            this.AddItem(this.Avatar);
            this.Ship.Clear();

            //LoadScenes.Load("select_character");
            LoadScenes.Load("select_character");

        }

        public void ClearItemCache()
        {
            _itemCache.Clear();
        }

        public void SetDisconnected(StatusCode returnCode)
        {
            ChangeStrategy(Disconnected.Instance);
            this.ClearItemCache();
            if(this.Avatar != null) {
                this.Avatar.DestroyView();
            }
            Debug.LogFormat("Disconnected: {0}", returnCode);
        }

        public void SetStateWorldEntered(WorldData worldData)
        {
            _world = worldData;
            ChangeStrategy(WorldEntered.Instance);
            OperationResetViewDistance();
            var position = new float[] { 0, 0, START_Z };
            _avatar.SetPositions(position, position, null, null, 0);
            LoadScenes.Load(DataResources.Instance.ZoneForId(World.Name).Scene());
            Debug.LogFormat("<color=orange>LOAD SCENE={0}</color>", DataResources.Instance.ZoneForId(World.Name).Scene());
        }

        public void OnWorkshopExited(float[] pos)
        {
            if (TryRecreateCharacter())
            {
                this.Avatar.SetPositions(pos, pos, this.Avatar.Rotation, this.Avatar.Rotation, 0);
                ChangeStrategy(WorldEntered.Instance);
                Application.LoadLevel(DataResources.Instance.ZoneForId(this.clientWorld.Id).Scene());
            }
        }

        public void TryEnterWorkshop(WorkshopStrategyType workshopStrategyType)
        {
            if (this.State != GameState.Workshop)
            {
                ChangeStrategy(WorkshopStrategy.Instance);
                Operations.EnterWorkshop(this, this.Avatar.Id, workshopStrategyType);
            }
        }



        public void TryExitWorkshop()
        {
            Operations.ExitWorkshop(this);
        }

        public void ExitWorld() {
            if(this.Avatar != null)
                this.Avatar.RequestTarget(string.Empty, (byte)ItemType.Avatar, false);
            Operations.ExitWorld(this);

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

        public void Update()
        {
            m_stateStrategy.OnUpdate(this);
            this.UpdateProperties();
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
            if (Time.time > this.nextUpdateModelTime)
            {
                this.nextUpdateModelTime = Time.time + 3.0f;
                if (this.Avatar != null && this.Ship != null)
                {
                    if (this.Ship.ShipModel.HasAllModules() == false)
                    {
                        if (this.State != GameState.SelectCharacter)
                        {
                            NRPC.RequestShipModel();
                        }
                    }

                }
            }
            if (State == GameState.Workshop)
            {
                if (Time.time > this.nextUpdateModelTimeInWorkshop)
                {
                    this.nextUpdateModelTimeInWorkshop = Time.time + 1.0f;
                    NRPC.RequestShipModel();
                }
                if (Time.time > this.nextStationUpdate)
                {
                    this.nextStationUpdate = Time.time + 2.5f;
                    NRPC.RequestStation();
                }
            }
            if (State == GameState.Workshop || State == GameState.WorldEntered)
            {
                //make request for combat params
                if (Time.time > this.nextRequestOfCombatParams)
                {
                    this.nextRequestOfCombatParams = Time.time + .5f;
                    NRPC.RequestCombatStats();
                }
                if (Time.time > this.nextUpdateWeaponTime)
                {
                    this.nextUpdateWeaponTime = Time.time + 0.5f;
                    NRPC.RequestWeapon();
                }
                if (Time.time > this.nextUpdateInventoryTime)
                {
                    this.nextUpdateInventoryTime = Time.time + INVENTORY_UPDATE_INTERVAL;
                    NRPC.RequestInventory();
                }
                if (Time.time > this.nextUpdatePlayerInfoTime)
                {
                    this.nextUpdatePlayerInfoTime = Time.time + 10;
                    NRPC.RequestPlayerInfo();
                }
                if (Time.time > this.nextUpdateSkillsTime)
                {
                    this.nextUpdateSkillsTime = Time.time + 1.0f;
                    NRPC.RequestSkills();
                }
                if (Time.time > this.nextUpdateEventsTime)
                {
                    this.nextUpdateEventsTime = Time.time + 2.0f;
                    NRPC.RequestEvents();
                }

                if (Time.time > this.nextChatUpdate)
                {
                    this.nextChatUpdate = Time.time + CHAT_UPDATE_INTERVAL;
                    NRPC.GetChatUpdate();
                }

                if (this.Avatar != null)
                {
                    if (Time.time > nextPlayerPropertiesUpdate)
                    {
                        if (this.Avatar.ShipDestroyed)
                            Debug.Log("Player avatar ship destroyed");

                        nextPlayerPropertiesUpdate = Time.time + 0.5f;
                        //Dbg.Print("request properties", "PLAYER");
                        this.Avatar.GetProperties(new string[] { 
                        GroupProps.DEFAULT_STATE, 
                        GroupProps.MECHANICAL_SHIELD_STATE, 
                        GroupProps.POWER_FIELD_SHIELD_STATE, 
                        GroupProps.SHIP_BASE_STATE});
                    }

                    if (Time.time > nextPlayerBonusesUpdate)
                    {
                        nextPlayerBonusesUpdate = Time.time + 0.3f;
                        this.Avatar.GetBonuses();
                    }
                }
                else
                    Dbg.Print("AVATAR NULL", "PLAYER");


                if (State == GameState.WorldEntered)
                {
                    if (Time.time > nextOtherItemsUpdate)
                    {
                        nextOtherItemsUpdate = Time.time + 1;
                        if (this._itemCache.ContainsKey(ItemType.Avatar.toByte()))
                        {
                            Dictionary<string, Item> avatars = this._itemCache[ItemType.Avatar.toByte()];
                            foreach (var p2 in avatars)
                            {
                                if (p2.Value != null)
                                {
                                    if (p2.Value.IsMine == false)
                                    {
                                        p2.Value.AdditionalUpdate();
                                    }
                                }
                            }
                        }

                        Dictionary<string, Item> botItems = null;
                        if (this._itemCache.TryGetValue(ItemType.Bot.toByte(), out botItems))
                        {
                            foreach (var pBots in botItems)
                            {
                                pBots.Value.AdditionalUpdate();
                            }
                        }
                    }
                }

            }

        }

        public void DebugReturn(DebugLevel debugLevel, string debug)
        {
        }

        public void OnEvent(EventData ev)
        {
            this.m_stateStrategy.OnEventReceive(this, ev);
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            this.m_stateStrategy.OnOperationReturn(this, operationResponse);
        }


        public void OnStatusChanged(StatusCode returnCode)
        {
            
            if (m_stateStrategy != null)
                this.m_stateStrategy.OnPeerStatusCallback(this, returnCode);
        }


        public string CurrentWorldId()
        {
            return this.World.Name;
        }


        public void AddToInventoryTransaction(string containerItemId, byte containerItemType, List<ClientInventoryItem> inventoryItems)
        {
            if (_currentObjectContainer.IsSelectedContainerItem(containerItemId, containerItemType))
            {
                foreach (var item in inventoryItems)
                {
                    _inventory.ReplaceItem(item);
                }
            }
        }


        //=========================================HELPER FUNCTIONS==========================================================
        #region HELPER FUNCTIONS
        public static void OnShipModelUpdated(NetworkGame game, Hashtable modelInfo)
        {
            //Debug.Log(modelInfo.ToStringBuilder().ToString());
            Hashtable slots = modelInfo.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());
            //Dbg.PrintArray("SLOTS ARRAY", slots);

            foreach (DictionaryEntry entry in slots)
            {
                switch ((ShipModelSlotType)(byte)entry.Key)
                {
                    case ShipModelSlotType.CB:
                        {
                            Hashtable slotHashtable = entry.Value as Hashtable;
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());

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
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());

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
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());

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
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());

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
                            Hashtable moduleHashtable = slotHashtable.GetValue<Hashtable>(GenericEventProps.info, new Hashtable());

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
            if (newState == GameState.WorldEntered) {
                if (MainCanvas.Get != null) {
                    MainCanvas.Get.Show(CanvasPanelType.ControlHUDView);
                    MainCanvas.Get.Show(CanvasPanelType.MenuHUDView);
                    MainCanvas.Get.Show(CanvasPanelType.ChatView);
                    MainCanvas.Get.Show(CanvasPanelType.EventTasksView);
                }
            } else {
                if (MainCanvas.Get != null) {
                    MainCanvas.Get.Destroy(CanvasPanelType.ControlHUDView);
					MainCanvas.Get.Destroy(CanvasPanelType.MenuHUDView);
					MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
					MainCanvas.Get.Destroy(CanvasPanelType.SelectedObjectContextMenuView);
					MainCanvas.Get.Destroy(CanvasPanelType.TargetObjectView);
                    MainCanvas.Get.Destroy(CanvasPanelType.EventTasksView);
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

    }


}







