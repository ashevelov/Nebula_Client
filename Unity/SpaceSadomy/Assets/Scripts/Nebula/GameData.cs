using Common;
using Game.Space;
using Nebula.Client;
using Nebula.Client.Guilds;
using Nebula.Client.Mail;
using Nebula.Client.Notifications;
using UnityEngine;
using System.Collections;
using Nebula.Mmo.Games;
using Nebula.UI;
using Nebula.Client.Store;
using Nebula.Client.Auction;
using Nebula.Client.Worlds;

namespace Nebula {

    public class GameData  {
        public WorldData World { get; private set; }
        public Chat Chat { get; private set; }
        public CurrentObjectContainer CurrentObjectContainer { get; private set; }
        public MailBox mailBox { get; private set; }
        public CharacterNotifications notifications { get; private set; }
        public Guild guild { get; private set; }
        public Group group { get; private set; }
        public ActorBonuses bonuses { get; private set; }
        public ClientInventory inventory { get; private set; }
        public ClientWorkhouseStation station { get; private set; }
        public PlayerShip ship { get; private set; }
        public ClientShipCombatStats stats { get; private set; }
        public ClientPlayerSkills skills { get; private set; }
        public ClientPlayerInfo playerInfo { get; private set; }
        public WorldTransitionInfo worldTransition { get; private set; }
        public IServiceMessageReceiver sericeMessages { get; private set; }
        public ClientWorld clientWorld { get; private set; }
        public ClientSearchGroupsResult searchGroupResult { get; private set; }
        public PlayerStore store { get; private set; }
        public AuctionRequest auction { get; private set; }
        public WorldCollection worlds { get; private set; }

        private GameData() {
            mailBox = new MailBox();
            notifications = new CharacterNotifications();
            guild = new Guild();
            group = new Group();
            bonuses = new ActorBonuses();
            inventory = new ClientInventory();
            station = new ClientWorkhouseStation(new Hashtable());
            ship = new PlayerShip();
            stats = new ClientShipCombatStats();
            skills = new ClientPlayerSkills();
            playerInfo = new ClientPlayerInfo();
            playerInfo.SetExpChanged(OnExpChanged);
            Settings settings = Settings.GetDefaultSettings();
            worldTransition = new WorldTransitionInfo(settings.DefaultZones[Race.Humans], settings.DefaultZones[Race.Humans]);
            sericeMessages = new ServiceMessageReceiver(100);
            clientWorld = new ClientWorld();
            searchGroupResult = new ClientSearchGroupsResult();
            store = new PlayerStore();
            auction = new AuctionRequest();
            worlds = new WorldCollection();
        }

        public void Clear() {
            CurrentObjectContainer.Reset();
            notifications.Clear();
            guild.Clear();
            group.Clear();
            bonuses.Clear();
            inventory.Clear();
            station.Clear();
            ship.Clear();
            stats.Clear();
            sericeMessages.Clear();
            searchGroupResult.Clear();
            worlds.Clear();
        }

        public void SetNewWorld(string worldID, Vector3 cornerMin, Vector3 cornerMax, Vector3 tileDimensions, LevelType levelType) {
            World = new WorldData(worldID, cornerMin, cornerMax, tileDimensions, levelType);
            Chat = new Chat();
            CurrentObjectContainer = new CurrentObjectContainer();
        }

        public void SetNewWorld(WorldData world) {
            World = world;
        }

        public bool HasWorld {
            get {
                return World != null;
            }
        }

        private static GameData _instance;

        public static GameData instance {
            get {
                if(_instance == null ) {
                    _instance = new GameData();
                }
                return _instance;
            }
        }

        private static void OnExpChanged(int oldExp, int newExp ) {
            if(NetworkGame.Instance().CurrentStrategy == GameState.NebulaGameWorldEntered && (oldExp != newExp)) {
                if(MainCanvas.Get == null ) { return; }
                if (MainCanvas.Get.Exists(CanvasPanelType.ControlHUDView)) {
                    var hud = MainCanvas.Get.GetView(CanvasPanelType.ControlHUDView);
                    if (hud) {
                        hud.GetComponentInChildren<ControlHUDView>().OnExpChanged(oldExp, newExp);
                    }
                }
            }
        }
    }
}
