using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StarSystem {
    public string _id;
    public Fraction _control;
    //public List<SpaceZone> _spaceZone;
    public List<string> _nearbyStarSystems;
    public Vector3 pos;
    private GameObject _go;



    public void SetGO(GameObject go)
    {
        _go = go;
    }
    public GameObject GetGO()
    {
        return _go;
    }

    public bool NearbyStarExist(string sId)
    {
        foreach (string s in _nearbyStarSystems)
        {
            if (s == sId)
            {
                return true;
            }
        }
        return false;
    }
}

public enum Fraction
{
    BORGUZANDS,
    HUMANS,
    KRIPTIZIDS,
    NEUTRAL
}
