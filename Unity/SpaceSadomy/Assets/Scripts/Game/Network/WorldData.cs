using Common;
using UnityEngine;

namespace Nebula
{
    public class WorldData
    {
        private Vector3 _cornerMin;
        private Vector3 _cornerMax;
        private string _name;
        private Vector3 _tileDimensions;
        private LevelType _levelType;

        public WorldData()
        {
        }

        public void SetWorldParameters(string name, Vector3 cornerMin, Vector3 cornerMax, Vector3 tileDimensions, LevelType levelType)
        {
            _name = name;
            _cornerMin = cornerMin;
            _cornerMax = cornerMax;
            _tileDimensions = tileDimensions;
            _levelType = levelType;
        }

        public void SetWorldParameters(string name, LevelType levelType)
        {
            _name = name;
            _levelType = levelType;
        }

        public string Name { get { return _name; } }
        public Vector3 CornerMin { get { return _cornerMin; } }
        public Vector3 CornerMax { get { return _cornerMax; } }
        public Vector3 TileDimensions { get { return _tileDimensions; } }
        public LevelType LevelType { get { return _levelType; } }
    }
}