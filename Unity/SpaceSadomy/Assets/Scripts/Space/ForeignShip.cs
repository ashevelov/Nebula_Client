using Common;
using Nebula.Mmo.Items;
using ServerClientCommon;
using System.Collections;

namespace Nebula {

    public class ForeignShip : IServerPropertyParser {

        private float _maxHealth;
        private float _health;
        private Weapon _weapon;
        private bool _destroyed;
        private Item _owner;
        private float _speed;
        private Hashtable modelInfo;
        private Workshop workshop;



        public ForeignShip(Item owner) 
        {
            _owner = owner;
            _weapon = new Weapon( 0);
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


        public float Speed {
            get {
                return _speed;
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

        public void ParseProp(byte propName, object value)
        {
            switch ((PS)propName) 
            {
                case PS.MaxHealth:
                    _maxHealth = (float)value;
                    break;
                case PS.CurrentHealth:
                    _health = (float)value;
                    break;
                case PS.Destroyed:
                    {
                        bool old = _destroyed;
                        _destroyed = (bool)value;
                        _owner.SetShipDestroyed(_destroyed);
                    }
                    break;
                case PS.CurrentLinearSpeed:
                    _speed = (float)value;
                    break;
                case PS.ModelInfo:

                    {
                        this.modelInfo = (Hashtable)value;
                    }
                    break;
                case PS.Workshop:
                    {
                        this.workshop = (Workshop)(byte)value;
                    }
                    break;
            }
        }

        public void ParseInfo(Hashtable info )
        {
            foreach(DictionaryEntry entry in info )
            {
                if((int)entry.Key == (int)SPC.Model )
                {
                    this.modelInfo = (entry.Value as Hashtable);
                }
                else if((int)entry.Key == (int)SPC.MaxHealth)
                {
                    this._maxHealth = (float)entry.Value;
                }
                else if((int)entry.Key == (int)SPC.CurHealth)
                {
                    this._health = (float)entry.Value;
                    //Debug.Log("Current health received: {0:F1}".f(this._health));
                }
                else if((int)entry.Key == (int)SPC.Destroyed)
                {
                    this._destroyed = (bool)entry.Value;
                    this._owner.SetShipDestroyed(this._destroyed);
                }
                else if((int)entry.Key == (int)SPC.Speed )
                {
                    this._speed = (float)entry.Value;
                }
                else if((int)entry.Key == (int)SPC.Workshop)
                {
                    this.workshop = (Workshop)(byte)entry.Value;
                }
            }
        }
    }
}
