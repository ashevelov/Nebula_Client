using Common;
using System.Collections;

namespace Nebula {

    public class Weapon : IServerPropertyParser {

        private float _damage;


        //private float _rangeMin;
        //private float _rangeMax;
        //private float _minHitProb;

        private float _optimalDistance;
        private float _range;
        private float _nearProb;
        private float _farProb;
        private float _maxHitSpeed;
        private float _maxFireDistance;

        public Weapon(float damage) {
            _damage = damage;
        }

        public float OptimalDistance
        {
            get
            {
                return _optimalDistance;
            }
        }
        public float Range
        {
            get
            {
                return _range;
            }
        }
        public float NearProb
        {
            get
            {
                return _nearProb;
            }
        }

        public float FarProb
        {
            get
            {
                return _farProb;
            }
        }
        public float MaxHitSpeed
        {
            get
            {
                return _maxHitSpeed;
            }
        }
        public float MaxFireDistance
        {
            get
            {
                return _maxFireDistance;
            }
        }


        public float Damage {
            get {
                return _damage;
            }
        }




        public void ParseProp(string propName, object value)
        {
            switch (propName) { 
                case Props.SHIP_WEAPON_STATE_BASE_DAMAGE:
                    _damage = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_OPTIMAL_DISTANCE:
                    _optimalDistance = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_RANGE:
                    _range = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_PROB_NEAR2OPTIMALDISTANCE:
                    _nearProb = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_PROB_FAR2OPTIMALDISTANCE:
                    _farProb = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_MAX_HIT_SPEED:
                    _maxHitSpeed = (float)value;
                    break;
                case Props.SHIP_WEAPON_STATE_MAX_FIRE_DISTANCE:
                    _maxFireDistance = (float)value;
                    break;
            }
        }

        public void ParseProps(System.Collections.Hashtable properties)
        {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }
    }
}