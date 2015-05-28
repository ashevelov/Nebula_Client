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


        public void ParseProp(string propName, object value) {
            switch (propName) {
                case Props.MECHANICAL_SHIELD_ENABLED:
                    {
                        bool _old = _enabled;
                        _enabled = (bool)value;
                    }
                    break;
                case Props.MECHANICAL_SHIELD_CURRENT_DAMAGE_ABSORB:
                    {
                        float _old = _currentDamageAbsorbAllow;
                        _currentDamageAbsorbAllow = (float)value;
                    }
                    break;
                case Props.MECHANICAL_SHIELD_FULL_DAMAGE_ABSORB:
                    {
                        float _old = _fullDamageAbsorb;
                        _fullDamageAbsorb = (float)value;
                    }
                    break;
            }
        }

        public void ParseProps(Hashtable properties) {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }
    }
}