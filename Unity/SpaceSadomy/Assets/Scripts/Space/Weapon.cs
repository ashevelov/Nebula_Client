using Common;
using System.Collections;

namespace Nebula {

    public class Weapon : IServerPropertyParser {

        private float _damage;
        private float _optimalDistance;
        private float _range;
        private float _nearProb;
        private float _farProb;
        private float _maxHitSpeed;
        private float _maxFireDistance;

        public Weapon(float damage) {
            _damage = damage;
        }

        public float OptimalDistance {
            get {
                return _optimalDistance;
            }
        }
        public float Range {
            get {
                return _range;
            }
        }
        public float NearProb {
            get {
                return _nearProb;
            }
        }

        public float FarProb {
            get {
                return _farProb;
            }
        }
        public float MaxHitSpeed {
            get {
                return _maxHitSpeed;
            }
        }
        public float MaxFireDistance {
            get {
                return _maxFireDistance;
            }
        }

        public float Damage {
            get {
                return _damage;
            }
        }

        public void ParseProp(byte propName, object value) {
            switch ((PS)propName) {
                case PS.BaseDamage:
                    _damage = (float)value;
                    break;
                case PS.OptimalDistance:
                    _optimalDistance = (float)value;
                    break;
                case PS.WeaponRange:
                    _range = (float)value;
                    break;
                case PS.ProbNear2OptimalDistance:
                    _nearProb = (float)value;
                    break;
                case PS.ProbFar2OptimalDistance:
                    _farProb = (float)value;
                    break;
                case PS.MaxHitSpeed:
                    _maxHitSpeed = (float)value;
                    break;
                case PS.MaxFireDistance:
                    _maxFireDistance = (float)value;
                    break;
            }
        }

        public void ParseProps(Hashtable properties) {
            foreach (DictionaryEntry entry in properties) {
                ParseProp((byte)entry.Key, entry.Value);
            }
        }
    }
}