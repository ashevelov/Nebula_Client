using Common;
using System.Collections;

namespace Nebula {

    public class ForeignShip : IServerPropertyParser {

        private int _model;
        private float _maxHealth;
        private float _health;
        private Weapon _weapon;
        private bool _destroyed;
        private Item _owner;
        private ShipPowerShield _powerShield;
        private float _speed;
        private Hashtable modelInfo;
        private Workshop workshop;



        public ForeignShip(Item owner) 
        {
            _owner = owner;
            _weapon = new Weapon( 0);
            _powerShield = new ShipPowerShield(owner);
            this.modelInfo = new Hashtable();
        }

        public Weapon Weapon {
            get {
                return _weapon;
            }
        }

        public float MaxHealth {
            get {
                return _maxHealth;
            }
        }

        public float Health {
            get {
                return _health;
            }
        }

        public bool Destroyed {
            get {
                return _destroyed;
            }
        }

        public int Model {
            get {
                return _model;
            }
        }

        public float Speed {
            get {
                return _speed;
            }
        }

        public ShipPowerShield PowerField {
            get {
                return _powerShield;
            }
        }

        public Hashtable ModelInfo
        {
            get
            {
                return this.modelInfo;
            }
        }

        public Workshop Workshop
        {
            get
            {
                return this.workshop;
            }
        }

        public void ParseProp(string propName, object value)
        {
            switch (propName) 
            { 
                case Props.SHIP_BASE_STATE_MODEL:
                    _model = (int)value;
                    break;
                case Props.SHIP_BASE_STATE_MAX_HEALTH:
                    _maxHealth = (float)value;
                    break;
                case Props.SHIP_BASE_STATE_HEALTH:
                    _health = (float)value;
                    break;
                case Props.SHIP_BASE_STATE_DESTROYED:
                    {
                        bool old = _destroyed;
                        _destroyed = (bool)value;
                        _owner.SetShipDestroyed(_destroyed);
                    }
                    break;
                case Props.SHIP_BASE_STATE_CURRENT_LINEAR_SPEED:
                    _speed = (float)value;
                    break;
                case Props.SHIP_BASE_STATE_MODEL_INFO:
                    {
                        this.modelInfo = (Hashtable)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_WORKSHOP:
                    {
                        this.workshop = (Workshop)(byte)value;
                    }
                    break;
                case "cur_health":
                    this._health = (float)value;
                    break;
            }
        }

        public void ParseProps(System.Collections.Hashtable properties)
        {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }

        public void ParseInfo(Hashtable info )
        {
            foreach(DictionaryEntry entry in info )
            {
                if(entry.Key.ToString() == "model" )
                {
                    this.modelInfo = (entry.Value as Hashtable);
                }
                else if(entry.Key.ToString() == "max_health")
                {
                    this._maxHealth = (float)entry.Value;
                }
                else if(entry.Key.ToString() == "cur_health")
                {
                    this._health = (float)entry.Value;
                    //Debug.Log("Current health received: {0:F1}".f(this._health));
                }
                else if(entry.Key.ToString() == "destroyed")
                {
                    this._destroyed = (bool)entry.Value;
                    this._owner.SetShipDestroyed(this._destroyed);
                }
                else if(entry.Key.ToString() == "speed" )
                {
                    this._speed = (float)entry.Value;
                }
                else if(entry.Key.ToString() == "workshop")
                {
                    this.workshop = (Workshop)(byte)entry.Value;
                }
            }
        }
    }
}
