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
using Nebula.Resources;
using Nebula.Mmo.Items;

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
            GameData = GameData.instance;

            nebulaGame = new NetworkGame(this, settings);
            var peer = new GamePeer(nebulaGame, ConnectionProtocol.Udp);
            nebulaGame.SetPeer(peer);


            masterGame = new MasterGame(this, settings);
            var masterPeer = new MasterPeer(masterGame, ConnectionProtocol.Udp);
            masterGame.SetPeer(masterPeer);
#if LOCAL
            masterGame.Connect("192.168.1.107", 5105, "Master");
#else
            masterGame.Connect("104.207.135.55", 5105, "Master");
#endif
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

        Debug.LogFormat("Game State Changed, old = {0} => new = {1}", pState, nState);

        if(pState == GameState.NebulaGameWaitingConnect && nState == GameState.NebulaGameConnected) {
            var character = SelectCharacterGame.PlayerCharacters.SelectedCharacter();
            if (NebulaGame.DisconnectAction == NebulaGameDisconnectAction.None) {
                NebulaGame.EnterWorld(character.World, character.World);
            } else if(NebulaGame.DisconnectAction == NebulaGameDisconnectAction.ChangeWorld) {
                NebulaGame.EnterWorld(GameData.worldTransition.PrevWorld, GameData.worldTransition.NextWorld);
            }
            Debug.LogFormat("entering to world {0}", character.World);
        }
        
        //if(nState == GameState.NebulaGameDisconnected) {
        //    if(NebulaGame.DisconnectAction == NebulaGameDisconnectAction.Menu ) {
        //        NebulaGame.SetDisconnectAction(NebulaGameDisconnectAction.None);
        //        SetActiveGame(GameType.SelectCharacter);
        //        LoadScenes.Load("select_character");
        //    }
        //}
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

    public void OnGameRefIdReceived(string grID, string login) {
        if(string.IsNullOrEmpty(grID)) {
            Debug.LogError("Invalid game ref id received");
        } else {
            Debug.LogFormat("game ref id received = {0}", grID);
        }
        LoginGame.SetGameRefId(grID);
        LoginGame.SetLogin(login);

        if (LoginGame.IsGameRefIdValid()) {
            SetActiveGame(GameType.SelectCharacter);
            var selectCharacterServer = MasterGame.GetServer(ServerType.character);
            SelectCharacterGame.Connect(selectCharacterServer.IpAddress, selectCharacterServer.Port, selectCharacterServer.Application);

            //Application.LoadLevel("select_character");
            LoadScenes.Load("select_character");
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
    public void SetActiveGame(GameType gameType) {
        Debug.LogFormat("MmoEngine set active game {0}", ActiveGame);
        activeGame = gameType;
    }


    public void OnAddToInventoryFromCurrentContainer(string containerItemID, byte containerItemType, List<ClientInventoryItem> inventoryItems) {
        if(GameData.CurrentObjectContainer.IsSelectedContainerItem(containerItemID, containerItemType)) {
            foreach(var item in inventoryItems) {
                GameData.inventory.ReplaceItem(item);
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

    public void ExitMenu() {
        NebulaGame.SetDisconnectAction(Nebula.Mmo.Games.NebulaGameDisconnectAction.None);
        NebulaGame.SetStrategy(Nebula.GameState.NebulaGameDisconnected);
        NebulaGame.Disconnect();
        SetActiveGame(Nebula.Mmo.Games.GameType.SelectCharacter);
        GameData.instance.Clear();
        LoadScenes.Load("select_character");
    }

    void OnGUI() {
        //DrawAvatarProperties();
        //DrawWeaponProperties();
        //DrawShipProperties();
        //DrawBonuses();
        //DrawSkills();
        //DrawChat();
        DrawGroup();

        /*
        GUI.Label(new Rect(5, Screen.height - 30, 0, 0), "loaded scene: " + Application.loadedLevelName, skin.GetStyle("font_upper_left"));
        if (NebulaGame != null) {
            GUI.Label(new Rect(5, Screen.height - 60, 0, 0), "game strategy: " + NebulaGame.CurrentStrategy, skin.GetStyle("font_upper_left"));
        }*/

        /*
        if (NebulaGame.Avatar.target != null) {
            GUI.Label(new Rect(100, 80, 0, 0), string.Format("Target=(ID={0}, TYPE={1}, HAS={2})", NebulaGame.Avatar.target.targetID,
                (ItemType)NebulaGame.Avatar.target.targetType,
                NebulaGame.Avatar.target.hasTarget), skin.GetStyle("font_upper_left"));
        } else {
            GUI.Label(new Rect(100, 80, 0, 0), "Target component is (NULL)", skin.GetStyle("font_upper_left"));
        }

        if (GUI.Button(new Rect(100, 100, 100, 50), "Move Target")) {

            NRPC.AddAll(NebulaGame.Avatar.target.targetID, NebulaGame.Avatar.target.targetType);
        }*/

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
        if(GameData.ship == null ) {
            return;
        }
        if(GameData.ship.Weapon == null ) {
            return;
        }

        var weapon = GameData.ship.Weapon;
        Dictionary<SPC, object> weaponProperties = new Dictionary<SPC, object> {
            { SPC.HasWeapon , weapon.HasWeapon},
            { SPC.HitProb, weapon.HitProb},
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
        if (GameData.ship == null) {
            return;
        }

        var ship = GameData.ship;
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
        if(GameData.bonuses == null ) {
            return;
        }

        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(500, 0, Screen.width, 20);
        foreach(var bonusPair in GameData.bonuses.Bonuses) {
            string content = string.Format("{0}={1}", bonusPair.Key, bonusPair.Value);
            GUI.Label(rect, content, labelStyle);
            rect = rect.addOffset(0, 10);
        }
    }

    private void DrawSkills() {
        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(10, 10, 0, 0);
        if(NebulaGame == null ) { return;  }
        if(GameData.skills == null ) { return; }
        if(GameData.skills.Skills == null ) { return; }

        float x = 300;
        float y = 10;
        foreach(var pair in GameData.skills.Skills) {
            GUI.Label(new Rect(x, y, 0, 0), string.Format("Skill at {0} = {1:X0}", pair.Key, pair.Value.Id), labelStyle);
            y += 25;
        }
    }

    private void DrawChat() {
        GUIStyle labelStyle = skin.GetStyle("font_upper_left");
        Rect rect = new Rect(10, 10, 0, 0);
        if(GameData.Chat == null ) { return; }
        if(GameData.Chat.messages == null ) { return; }

        for(int i = GameData.Chat.messages.Count - 1; i >= Mathf.Max(0, GameData.Chat.messages.Count - 10); --i) {
            GUI.Label(rect, GameData.Chat.messages[i].DecoratedMessage, labelStyle);
            rect = new Rect(rect.x, rect.y + 20, 0, 0);
        }
    }

    private void DrawGroup() {
        GUIStyle labelStyle = skin.GetStyle("font_upper_left");

        if (!GameData.group.has) {
            return;
        }

        float x = 10;
        float y = 10;
        foreach(var member in GameData.group.members) {
            GUI.Label(new Rect(x, y, 0, 0), member.Value.ToString(), labelStyle);
            y += 20;
        }
    }
}