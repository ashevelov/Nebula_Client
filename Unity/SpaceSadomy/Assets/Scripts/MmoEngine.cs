using Common;
using ExitGames.Client.Photon;
using Game.Space;
using Nebula;
using Nebula.Client.Servers;
using Nebula.Mmo.Games;
using Nebula.Mmo.Peers;
using System;
using System.Collections;
using UnityEngine;
using ServerClientCommon;
using Nebula.Client;
using System.Collections.Generic;
using Nebula.Client.Inventory;

public class MmoEngine : Singleton<MmoEngine>
{
    public GUISkin skin;

    private static bool created = false;

    private NetworkGame nebulaGame;
    private LoginGame loginGame;
    private MasterGame masterGame;
    private SelectCharacterGame selectCharacterGame;

    private GameType activeGame = GameType.None;
   
    void Awake() {
        if (!created) {
            created = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
         
    public void Start() {
        try {
            Application.runInBackground = true;
            Settings settings = Settings.GetDefaultSettings();
            var forceLoadResources = DataResources.Instance;
            GameData = new Nebula.GameData();

            nebulaGame = new NetworkGame(this, settings);
            var peer = new GamePeer(nebulaGame, ConnectionProtocol.Udp);
            nebulaGame.SetPeer(peer);


            masterGame = new MasterGame(this, settings);
            var masterPeer = new MasterPeer(masterGame, ConnectionProtocol.Udp);
            masterGame.SetPeer(masterPeer);
            masterGame.Connect("52.10.78.38", 5105, "Master");
            SetActiveGame(GameType.Master);

            loginGame = new LoginGame(this, settings);
            var loginPeer = new LoginPeer(loginGame, ConnectionProtocol.Udp);
            loginGame.SetPeer(loginPeer);

            selectCharacterGame = new SelectCharacterGame(this, settings);
            var selectCharacterPeer = new SelectCharacterPeer(selectCharacterGame, ConnectionProtocol.Udp);
            selectCharacterGame.SetPeer(selectCharacterPeer);

        } catch(Exception exception) {
            Debug.LogException(exception);
        }
    }

    public void Update()
    {
        
        nebulaGame.Update();
        masterGame.Update();
        loginGame.Update();
        selectCharacterGame.Update();
    }

    public void OnApplicationQuit()
    {
        try {
            nebulaGame.Disconnect();
            masterGame.Disconnect();
            loginGame.Disconnect();
            selectCharacterGame.Disconnect();
        } catch(Exception exception) {
            Debug.LogException(exception);
        } 
    }

    void OnEnable() {
        Events.GameStateChanged += Events_GameStateChanged;
    }

    void OnDisable() {
        Events.GameStateChanged -= Events_GameStateChanged;
    }

    private void Events_GameStateChanged(GameState pState, GameState nState) {
        if(pState == GameState.NebulaGameWaitingConnect && nState == GameState.NebulaGameConnected) {
            var character = SelectCharacterGame.PlayerCharacters.SelectedCharacter();
            if (NebulaGame.DisconnectAction == NebulaGameDisconnectAction.None) {
                NebulaGame.EnterWorld(character.World, character.World);
            } else if(NebulaGame.DisconnectAction == NebulaGameDisconnectAction.ChangeWorld) {
                NebulaGame.EnterWorld(NebulaGame.WorldTransition.PrevWorld, NebulaGame.WorldTransition.NextWorld);
            }
            Debug.LogFormat("entering to world {0}", character.World);
        }
        
    }


    public NetworkGame NebulaGame {
        get {
            return nebulaGame;
        }
    }

    public MasterGame MasterGame {
        get {
            return masterGame;
        }
    }

    public LoginGame LoginGame {
        get {
            return loginGame;
        }
    }

    public SelectCharacterGame SelectCharacterGame {
        get {
            return selectCharacterGame;
        }
    }

    public GameType ActiveGame {
        get {
            return activeGame;
        }
    }

    public GameData GameData {
        get; private set;
    }  

    public void CreateActor(NetworkGame engine, Item actor)
    {
        StartCoroutine(_CorCreateActor(engine, actor));
    }

    private IEnumerator _CorCreateActor(NetworkGame engine, Item actor )
    {
        while (!LevelGame.Get)
            yield return new WaitForEndOfFrame();

        while (GameData.World.LevelType != LevelType.Space)
            yield return new WaitForEndOfFrame();

        var zone = DataResources.Instance.ZoneForId(GameData.World.Name);

        while (Application.loadedLevelName != zone.Scene())
            yield return new WaitForEndOfFrame();

        if (actor != engine.Avatar && GameData.HasWorld && GameData.World.LevelType == LevelType.Space)
        {
            if (LevelGame.Get)
            {
                LevelGame.Get.CreateActorShip(engine, actor);
            }
        }
    }

    #region View distance now not used function
    //private void DecreaseViewDistance()
    //{
    //    /*
    //    InterestArea cam;
    //    this.engine.TryGetCamera(0, out cam);
    //    float[] viewDistance = (float[])cam.ViewDistanceEnter.Clone();
    //    viewDistance[0] = Math.Max(1, viewDistance[0] - (this.engine.WorldData.TileDimensions[0] / 2));
    //    viewDistance[1] = Math.Max(1, viewDistance[1] - (this.engine.WorldData.TileDimensions[1] / 2));
    //    cam.SetViewDistance(viewDistance);*/
    //}

    //private void IncreaseViewDistance()
    //{
    //    /*
    //    InterestArea cam;
    //    this.engine.TryGetCamera(0, out cam);
    //    float[] viewDistance = (float[])cam.ViewDistanceEnter.Clone();
    //    viewDistance[0] = Math.Min(this.engine.WorldData.Width, viewDistance[0] + (this.engine.WorldData.TileDimensions[0] / 2));
    //    viewDistance[1] = Math.Min(this.engine.WorldData.Height, viewDistance[1] + (this.engine.WorldData.TileDimensions[1] / 2));
    //    cam.SetViewDistance(viewDistance);*/
    //} 
    #endregion

    public void OnServersReceived(ClientServerCollection serverCollection) {

        MasterGame.SetServerCollection(serverCollection);

        if(MasterGame.AllServersPresents()) {
            Debug.Log("all servers presents");
            
        } else {
            Debug.Log("some servers not presents");
        }

        if(ActiveGame == GameType.Master ) {
            if (MasterGame.AllServersPresents()) {
                SetActiveGame(GameType.Login);
                var loginServer = MasterGame.GetServer(ServerType.login);
                LoginGame.Connect(loginServer.IpAddress, loginServer.Port, loginServer.Application);
            }
        } 
    }

    public void OnGameRefIdReceived(string grID) {
        if(string.IsNullOrEmpty(grID)) {
            Debug.LogError("Invalid game ref id received");
        } else {
            Debug.LogFormat("game ref id received = {0}", grID);
        }
        LoginGame.SetGameRefId(grID);
        if (LoginGame.IsGameRefIdValid()) {
            SetActiveGame(GameType.SelectCharacter);
            var selectCharacterServer = MasterGame.GetServer(ServerType.character);
            SelectCharacterGame.Connect(selectCharacterServer.IpAddress, selectCharacterServer.Port, selectCharacterServer.Application);

            Application.LoadLevel("select_character");
        }
    }

    public void OnPlayerCharactersReceived(ClientPlayerCharactersContainer playerCharacters) {
        SelectCharacterGame.SetCharacters(playerCharacters);
        Events.EvtPlayerCharactersReceived();
    }


    /// <summary>
    /// Setter active game type 
    /// </summary>
    /// <param name="gameType"></param>
    private void SetActiveGame(GameType gameType) {
        Debug.LogFormat("MmoEngine set active game {0}", ActiveGame);
        activeGame = gameType;
    }


    public void OnAddToInventoryFromCurrentContainer(string containerItemID, byte containerItemType, List<ClientInventoryItem> inventoryItems) {
        if(GameData.CurrentObjectContainer.IsSelectedContainerItem(containerItemID, containerItemType)) {
            foreach(var item in inventoryItems) {
                NebulaGame.Inventory.ReplaceItem(item);
            }
        }
    }

    public void ConnectToNebulaGame() {
        var selectedCharacter = SelectCharacterGame.PlayerCharacters.SelectedCharacter();
        if(selectedCharacter == null ) {
            throw new NebulaException("selected character null");
        }
        string world = selectedCharacter.World;
        if(string.IsNullOrEmpty(world)) {
            throw new NebulaException("character world is null");
        }
        var server = MasterGame.GetGameServer(world);
        if(server == null ) {
            throw new NebulaException(string.Format("game server not found for world = {0}", world));
        }
        NebulaGame.SetConnectionServer(server);
        SetActiveGame(GameType.Game);
        NebulaGame.Connect();
    }

    void OnGUI() {
        //DrawAvatarProperties();
        DrawWeaponProperties();
        DrawShipProperties();
        DrawBonuses();
    }

    private void DrawGameStates() {
        GUI.Label(new Rect(0, 0, Screen.width, 40), string.Format("Master Game State: {0}", MasterGame.CurrentStrategy), skin.GetStyle("font_upper_left"));
        GUI.Label(new Rect(0, 40, Screen.width, 40), string.Format("Login Game State: {0}", LoginGame.CurrentStrategy), skin.GetStyle("font_upper_left"));
        GUI.Label(new Rect(0, 80, Screen.width, 40), string.Format("Select Character Game State: {0}", SelectCharacterGame.CurrentStrategy), skin.GetStyle("font_upper_left"));
        GUI.Label(new Rect(0, 120, Screen.width, 40), string.Format("Nebula Game State: {0}", NebulaGame.CurrentStrategy), skin.GetStyle("font_upper_left"));
        GUI.Label(new Rect(0, 140, Screen.width, 20), "=====================================================", skin.GetStyle("font_upper_left"));
        GUI.Label(new Rect(0, 160, Screen.width, 40), string.Format("Active Engine Game: {0}", ActiveGame), skin.GetStyle("font_upper_left"));
    }

    private void DrawAvatarProperties() {
        if(NebulaGame.Avatar == null ) {
            return;
        } 
        if(NebulaGame.Avatar.props == null ) {
            return;
        }
        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(0, 0, Screen.width, 20);
        foreach(var dictionaryPair in NebulaGame.Avatar.props) {
            string content = string.Format("{0}={1}", (PS)dictionaryPair.Key, dictionaryPair.Value);
            GUI.Label(rect, content, labelStyle);
            rect = rect.addOffset(0, 10);
        }
    }

    private void DrawWeaponProperties() {
        if(NebulaGame.Avatar == null) {
            return;
        }
        if(NebulaGame.Ship == null ) {
            return;
        }
        if(NebulaGame.Ship.Weapon == null ) {
            return;
        }

        var weapon = NebulaGame.Ship.Weapon;
        Dictionary<SPC, object> weaponProperties = new Dictionary<SPC, object> {
            { SPC.HasWeapon , weapon.HasWeapon},
            { SPC.HeavyCooldown , weapon.HeavyCooldown},
            { SPC.HeavyDamage, weapon.HeavyDamage},
            { SPC.HeavyReady,  weapon.HeavyReady},
            { SPC.HeavyTimer, weapon.HeavyTimer},
            { SPC.HitProb, weapon.HitProb},
            { SPC.LightCooldown, weapon.LightCooldown},
            { SPC.LightDamage, weapon.LightDamage},
            { SPC.LightReady, weapon.LightReady},
            { SPC.LightTimer,  weapon.LightTimer},
            { SPC.OptimalDistance, weapon.OptimalDistance},
            { SPC.Range, weapon.Range},
        };

        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(0, 0, Screen.width, 20);
        foreach(var weaponPropPair in weaponProperties) {
            string content = string.Format("{0}={1}", weaponPropPair.Key, weaponPropPair.Value);
            GUI.Label(rect, content, labelStyle);
            rect = rect.addOffset(0, 10);
        }
    }

    private void DrawShipProperties() {
        if (NebulaGame.Avatar == null) {
            return;
        }
        if (NebulaGame.Ship == null) {
            return;
        }

        var ship = NebulaGame.Ship;
        Dictionary<string, object> shipProperties = new Dictionary<string, object> {
            { "Current Health", ship.Health },
            { "Max Health", ship.MaxHealth },
            { "Linera speed", ship.LinearSpeed },
            { "Acceleration", ship.Acceleration },
            { "Angle speed", ship.AngleSpeed },
            { "Min Linear speed", ship.MinLinearSpeed },
            { "Max linear speed", ship.MaxLinearSpeed },
            { "Energy", ship.Energy },
            { "Max Energy", ship.MaxEnergy }
        };

        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(300, 0, Screen.width, 20);
        foreach(var shipPropPair in shipProperties) {
            string content = string.Format("{0}={1}", shipPropPair.Key, shipPropPair.Value);
            GUI.Label(rect, content, labelStyle);
            rect = rect.addOffset(0, 10);
        }
    }

    private void DrawBonuses() {
        if (NebulaGame.Avatar == null) {
            return;
        }
        if(NebulaGame.Bonuses == null ) {
            return;
        }

        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(500, 0, Screen.width, 20);
        foreach(var bonusPair in NebulaGame.Bonuses.Bonuses) {
            string content = string.Format("{0}={1}", bonusPair.Key, bonusPair.Value);
            GUI.Label(rect, content, labelStyle);
            rect = rect.addOffset(0, 10);
        }
    }
}