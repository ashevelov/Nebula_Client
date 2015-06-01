using Common;
using System.Collections;

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

        public void ParseProp(byte propName, object value) {
            switch ((PS)propName) {
                case PS.PowerFieldShieldCurrentPowerPercent:
                    {
                        float _old = _fieldPower;
                        _fieldPower = (float)value;
                    }
                    break;
                case PS.PowerFieldShieldEnabled:
                    {
                        bool _old = _enabled;
                        _enabled = (bool)value;
                        if (_owner != null && _owner.Component) {
                            _owner.Component.OnPowerShieldStateChanged(_enabled);
                        }
                    }
                    break;
                case PS.PowerFieldShieldFullDamageAbsorb:
                    {
                        float _old = _fullDamage;
                        _fullDamage = (float)value;
                    }
                    break;
                case PS.PowerFieldShieldRecoverSpeed:
                    {
                        float _old = _fieldRecoverSpeed;
                        _fieldRecoverSpeed = (float)value;
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