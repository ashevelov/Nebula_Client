using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpaceZone
{
    public string _zoneId;
    public string _zoneDesc;
    private StarSystem _starSystem;
    public Vector3 pos;

    public void SetStarSystem(StarSystem starSystem)
    {
        _starSystem = starSystem;
    }
    public StarSystem GetStarSystem()
    {
        return _starSystem;
    }
}
