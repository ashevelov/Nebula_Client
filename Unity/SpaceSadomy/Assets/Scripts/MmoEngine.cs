using Common;
using Game.Space;
using Nebula;
using System;
using System.Collections;
using UnityEngine;


public class MmoEngine : Singleton<MmoEngine>
{
    private NetworkGame _game;
    private static bool _created = false;
    

    void Awake()
    {
        if (!_created)
        {
            _created = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        try
        {
            Application.runInBackground = true;
            Settings settings = Settings.GetDefaultSettings();
            var forceLoadResources = DataResources.Instance;

            this._game = new NetworkGame(this, settings, "Unity");
            var peer = new Nebula.PhotonPeer(this._game, settings.UseTcp);
            this._game.Initialize(peer);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.LogError(e.StackTrace);
        }
    }

    public void Update()
    {
        this._game.Update();
    }

    public void OnApplicationQuit()
    {
        try
        {
            this._game.Disconnect();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void OnLevelWasLoaded(int level) {
        Debug.LogFormat("<color=orange>LOADED LEVEL: {0}</color>", Application.loadedLevelName);
    }

    public NetworkGame Game {
        get {
            return _game;
        }
    }

    public void CreateActor(NetworkGame engine, Item actor)
    {
        StartCoroutine(_CorCreateActor(engine, actor));
    }

    private IEnumerator _CorCreateActor(NetworkGame engine, Item actor )
    {
        while (!LevelGame.Get)
            yield return new WaitForEndOfFrame();

        while (engine.World.LevelType != LevelType.Space)
            yield return new WaitForEndOfFrame();

        var zone = DataResources.Instance.ZoneForId(Game.World.Name);

        while (Application.loadedLevelName != zone.Scene())
            yield return new WaitForEndOfFrame();

        if (actor != engine.Avatar && engine.World != null && engine.World.LevelType == LevelType.Space)
        {
            if (LevelGame.Get)
            {
                LevelGame.Get.CreateActorShip(engine, actor);
            }
        }
    }

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

}