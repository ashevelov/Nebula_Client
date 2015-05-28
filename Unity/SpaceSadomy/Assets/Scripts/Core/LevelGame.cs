using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using System.Collections.Generic;
using Nebula.Game.Network.Items;
using Nebula;

public class LevelGame : Game.Space.Singleton<LevelGame> {

    void Start()
    {
        bool existWorld = MmoEngine.Get.Game.HasWorld;
        if(existWorld )
        {
            switch(MmoEngine.Get.Game.World.LevelType)
            {
                case LevelType.Space:
                    MmoEngine.Get.Game.Avatar.GetProperties(new string[]{});
                    StartCoroutine(CreatePlayerShip());
                    break;
                case LevelType.Map:
                    break;
            }
        }
    }


    public void CreateActorShip(NetworkGame game, Item actor)
    {
        StartCoroutine(_CreateActorShip(game, actor));
    }


    private IEnumerator CreatePlayerShip()
    {
        Debug.Log("<color=orange>Starting Coroutine for CREATING MY PLAYER SHIP</color>");
        var game = MmoEngine.Get.Game;
        while (game.Ship.ShipModel.HasAllModules() == false)
        {
            Debug.Log("Wait FOR MY PLAYER MODULES");
            yield return new WaitForEndOfFrame();
        }

        while (game.Avatar == null)
        {
            Debug.Log("WAIT FOR MY PLAYER ITEM");
            yield return new WaitForEndOfFrame();
        }

        var prefabs = game.Ship.ShipModel.SlotPrefabs();
        if (prefabs.Count != 5)
        {
            Debug.Log("EXCEPTION");
            throw new System.Exception("must be all 5 modules!!");
        }
        MmoEngine.Get.Game.Avatar.Create(ShipModel.Init(prefabs, true));
        G.Game.SetSpawnPosition(G.PlayerItem);
        MouseOrbitRotateZoom.Get.SetTarget(MmoEngine.Get.Game.Avatar.View.transform);
    }

    private IEnumerator _CreateActorShip(NetworkGame engine, Item actor)
    {
        do
        {
            //Debug.Log("CreateActorShip: Wait for player Avatar");
            yield return new WaitForEndOfFrame();
        } while (MmoEngine.Get.Game.Avatar == null);

        while (MmoEngine.Get.Game.ExistAvatarView == false)
        {
            //Debug.Log("CreateActorShip: Wait for player avatar view");
            yield return new WaitForEndOfFrame();
        }

        switch ((ItemType)actor.Type)
        {
            case ItemType.Avatar:
                {
                    ForeignPlayerItem foreignPlayerItem = actor as ForeignPlayerItem;
                    while (foreignPlayerItem.Modules.Prefabs.Count != 5)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    foreignPlayerItem.Create(ShipModel.Init(foreignPlayerItem.Modules.Prefabs, false));
                }
                break;
            case ItemType.Bot:
                {
                    
                    if (actor is InvasionPortalItem) {
                        InvasionPortalItem invasionPortal = actor as InvasionPortalItem;
                        while(invasionPortal.Model == 0 )
                            yield return new WaitForEndOfFrame();
                        invasionPortal.Create(Instantiate(GetShipModelPrefab(invasionPortal.Model)) as GameObject);
                    }
                    else if (actor is InvasionPortalEnemyItem) {
                        InvasionPortalEnemyItem enemy = actor as InvasionPortalEnemyItem;
                        while (enemy.Ship.Model == 0)
                            yield return new WaitForEndOfFrame();
                        enemy.Create(Instantiate(GetShipModelPrefab(1)) as GameObject);
                    }
                    else if (actor is DummyEnemyItem)
                    {
                        DummyEnemyItem de = actor as DummyEnemyItem;
                        while(de.Ship.ModelInfo == null || de.Ship.ModelInfo.Count != 5)
                            yield return new WaitForEndOfFrame();
                        Dictionary<ShipModelSlotType, string> prefabs = new Dictionary<ShipModelSlotType,string>();
                        foreach(DictionaryEntry entry in de.Ship.ModelInfo)
                        {
                            prefabs.Add((ShipModelSlotType)(byte)entry.Key, (string)entry.Value);
                        }
                        de.Create(ShipModel.Init(prefabs, false));
                    }
                    else if(actor is StandardNpcCombatItem)
                    {
                        var sItem = actor as StandardNpcCombatItem;

                        while (sItem.Ship.ModelInfo == null || sItem.Ship.ModelInfo.Count != 5)
                        {
                            Debug.Log("CreateActorShip(StandarNpcCombatObject): wait for ship model");
                            yield return new WaitForEndOfFrame();
                        }

                        Dictionary<ShipModelSlotType, string> prefabs = new Dictionary<ShipModelSlotType, string>();
                        foreach (DictionaryEntry entry in sItem.Ship.ModelInfo)
                        {
                            prefabs.Add((ShipModelSlotType)(byte)entry.Key, (string)entry.Value);
                        }

                        sItem.Create(ShipModel.Init(prefabs, false));
                    }
                    else if (actor is ProtectionStationItem)
                    {
                        var protectionStation = actor as ProtectionStationItem;
                        while (string.IsNullOrEmpty(protectionStation.Prefab))
                            yield return new WaitForEndOfFrame();
                        protectionStation.Create(Instantiate(PrefabCache.Get(protectionStation.Prefab)) as GameObject);
                    }
                    else if(actor is PirateStationItem)
                    {
                        (actor as PirateStationItem).Create(Instantiate(PrefabCache.Get("Prefabs/WorldObjects/ProtectionStation")) as GameObject);
                    }
                    else if(actor is PlanetItem)
                    {
                        var planetItem = actor as PlanetItem;
                        planetItem.Create(Instantiate(PrefabCache.Get("Prefabs/Planets/" + planetItem.Id)) as GameObject);
                    }
                    else if(actor is WorldActivatorItem)
                    {
                        //Debug.Log("Found Activator item for create view");
                        var activator = actor as WorldActivatorItem;
                        while (activator.ActivatorType < 0)
                        {
                            //Debug.Log("Wait activator type");
                            yield return new WaitForSeconds(1);
                        }
                        //Debug.Log("Create Activator");
                        this.CreateActivator(activator);
                    }
                }
                break;
            case ItemType.Chest:
                {
                    CreateChest(actor);
                }
                break;
            case ItemType.Asteroid:
                {
                    this.CreateAsteroid(actor);
                }
                break;
        }

    }

    //create chest view in scene
    private void CreateChest(Item item) 
    {
        ChestItem chestItem = item as ChestItem;
        chestItem.Create(Instantiate(PrefabCache.Get("Prefabs/WorldObjects/chest")) as GameObject);
    }

    private void CreateActivator(Item item)
    {
        var activatorItem = item as WorldActivatorItem;

        switch(activatorItem.ActivatorType)
        {
            case ActivatorType.SPACE_SCHEME:
                {
                    //Debug.Log("Create Data activator");
                    activatorItem.Create(Instantiate(PrefabCache.Get("Prefabs/WorldObjects/data")) as GameObject);
                }
                break;
            default:
                {
                    //Debug.Log("Create default activator");
                    activatorItem.Create(Instantiate(PrefabCache.Get("Prefabs/WorldObjects/WorldActivator")) as GameObject);
                }
                break;
        }

    }

    private void CreateAsteroid(Item item)
    {
        AsteroidItem asteroidItem = item as AsteroidItem;
        asteroidItem.Create(Instantiate(PrefabCache.Get("Prefabs/Asteroid")) as GameObject);
    }

    public static GameObject GetShipModelPrefab(int model)
    {
        switch (model)
        {
            case 1:
                return ShipModel.Init(false);  //PrefabCache.Get("Prefabs/Ships/model_1");
            case 2:
                return PrefabCache.Get("Prefabs/Ships/model_2");
            case 3:
                return PrefabCache.Get("Prefabs/Ships/model_3");
            case 4:
                return PrefabCache.Get("Prefabs/Ships/model_4");
            case 5:
                return PrefabCache.Get("Prefabs/Ships/model_5");
            case 6:
                return PrefabCache.Get("Prefabs/Ships/model_6");
            case 7:
                return PrefabCache.Get("Prefabs/Ships/model_7");
            case 8:
                return PrefabCache.Get("Prefabs/Ships/model_8");
            case 9:
                return PrefabCache.Get("Prefabs/Ships/model_9");
            case 10:
                return PrefabCache.Get("Prefabs/Ships/model_10");
            case 11:
                return PrefabCache.Get("Prefabs/Ships/model_11");
            case 12:
                return PrefabCache.Get("Prefabs/Ships/model_12");
            case 13:
                return PrefabCache.Get("Prefabs/Ships/model_13");
            case 14:
                return PrefabCache.Get("Prefabs/Ships/model_14");
            case 15:
                return PrefabCache.Get("Prefabs/Ships/model_15");
            case 100:
                return PrefabCache.Get("Prefabs/Ships/invasion_gate");
            default:
                return PrefabCache.Get("Prefabs/Ships/model_1");
        }

    }

}
