using UnityEngine;
using System.Collections;
using Game.Space;
using Common;

namespace Nebula {
    public class ShipMechanicalShield : IServerPropertyParser {
        private float _fullDamageAbsorb;
        private float _currentDamageAbsorbAllow;
        private bool _enabled;

        public ShipMechanicalShield() { }

        public float FullDamageAbsorbAllow {
            get {
                return _fullDamageAbsorb;
            }
        }

        public float CurrentDamageAbsorbAllow {
            get {
                return _currentDamageAbsorbAllow;
            }
        }

        public bool Enabled {
            get { return _enabled; }
        }

        public void SetEnabled(bool enabled) {
            _enabled = enabled;
        }

        public void SetFullDamageAllowed(float damage) {
            _fullDamageAbsorb = damage;
        }

        public void SetCurrentDamageAllowed(float damage) {
            _currentDamageAbsorbAllow = damage;
        }


        public void ParseProp(byte propName, object value) {
            switch ((PS)propName) {
                case PS.MechanicalShieldEnabled:
                    {
                        bool _old = _enabled;
                        _enabled = (bool)value;
                    }
                    break;
                case PS.MechanicalShieldCurrentDamageAbsorb:
                    {
                        float _old = _currentDamageAbsorbAllow;
                        _currentDamageAbsorbAllow = (float)value;
                    }
                    break;
                case PS.MechanicalShieldFullDamageAbsorb:
                    {
                        float _old = _fullDamageAbsorb;
                        _fullDamageAbsorb = (float)value;
                    }
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