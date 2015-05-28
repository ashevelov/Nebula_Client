using UnityEngine;
using System.Collections;
using Common;

public class StaticWorldObject : MonoBehaviour {

    private WorldObject _data;

    public void Initialize(WorldObject data) { _data = data;  }

    public WorldObjectType Type { get {
        if (_data != null)
            return (WorldObjectType)_data.Type;
        return WorldObjectType.planet;
    } }

    public string Id { get { return _data.Id; } }
}
