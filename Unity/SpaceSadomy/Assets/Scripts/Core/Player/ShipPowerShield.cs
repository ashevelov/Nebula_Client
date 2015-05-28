using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Game.Network;

namespace Nebula {
    public class ShipPowerShield : IServerPropertyParser {

        private float _fieldPower;
        private float _fieldRecoverSpeed;
        private float _fullDamage;
        private bool _enabled;
        private Item _owner;

        public ShipPowerShield(Item owner) {
            _owner = owner;
        }

        public float Power {
            get {
                return _fieldPower;
            }
        }

        public bool Enabled {
            get {
                return _enabled;
            }
        }

        public float RecoverSpeed {
            get {
                return _fieldRecoverSpeed;
            }
        }

        public float FullDamageAllowed {
            get {
                return _fullDamage;
            }
        }

        public void ParseProp(string propName, object value) {
            switch (propName) {
                case Props.POWER_FIELD_SHIELD_CURRENT_POWER_PERCENT:
                    {
                        float _old = _fieldPower;
                        _fieldPower = (float)value;
                    }
                    break;
                case Props.POWER_FIELD_SHIELD_ENABLED:
                    {
                        bool _old = _enabled;
                        _enabled = (bool)value;
                        if (_owner != null && _owner.Component) {
                            _owner.Component.OnPowerShieldStateChanged(_enabled);
                        }
                    }
                    break;
                case Props.POWER_FIELD_SHIELD_FULL_DAMAGE_ABSORB:
                    {
                        float _old = _fullDamage;
                        _fullDamage = (float)value;
                    }
                    break;
                case Props.POWER_FIELD_SHIELD_RECOVER_SPEED:
                    {
                        float _old = _fieldRecoverSpeed;
                        _fieldRecoverSpeed = (float)value;
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