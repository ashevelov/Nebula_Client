using UnityEngine;
using System.Collections;
using Game.Space;
using Nebula;
using Nebula.Resources;

public class SolarMapObject : MonoBehaviour {

    public SolarMapObjectType objectType;

    public LocalMapItem GetMapIcon(Transform parent)
    {
        GameObject mObj = GameObject.Instantiate(PrefabCache.Get(ObjPath())) as GameObject;
        mObj.transform.position = transform.position / 600;
        mObj.transform.parent = parent;
        return mObj.GetComponent<LocalMapItem>();
    }

    private string ObjPath()
    {
        switch (objectType)
        {
            case SolarMapObjectType.ivent:
                return "Prefabs/map/LocalMap/event";
            case SolarMapObjectType.asteroids_fild:
                return "Prefabs/map/LocalMap/asteroid_fild";
            case SolarMapObjectType.my_ship:
                return "Prefabs/map/LocalMap/MyShip";
            case SolarMapObjectType.station:
                return "Prefabs/map/LocalMap/station";
            case SolarMapObjectType.player:
                return "Prefabs/map/LocalMap/player";

        }
        return "Prefabs/map/LocalMap/station";
    }

}

public enum SolarMapObjectType
{
    asteroids_fild,
    my_ship,
    station,
    ivent,
    player
}
