using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Network;
using Nebula.Client;

namespace Nebula
{
    public class PlayerShip : IServerPropertyParser
    {
        private MyItem _owner;
        private float _currentHealth;
        private float _maxHealth;
        private float _linearSpeed;
        private float _acceleration;
        private float _angleSpeed;


        //player weapon, contains info ( weapon type, ready to fire or not, weapon reload interval), all it's return from server 'weapon_state' properties request
        private ClientPlayerShipWeapon weapon;

        //min linear speed of moving in not VARP mode
        private float _minLinearSpeed;
        //max linear speed of moving in not VARP mode
        private float _maxLinearSpeed;

        private ShipPowerShield _powerShield;
        private ShipMechanicalShield _mechanicalShield;

		private PlayerBonuses _bonuses;
		//private PlayerSkills _skills;
        private ClientShipModel _shipModel;

        private float _energy;
        private float _maxEnergy;

        private bool needRespawnFlagSetted = false;

        public void SetOwner(MyItem item)
        {
            this._owner = item;
        }

        public void Clear() {
            if(this._shipModel != null ) {
                this._shipModel.Clear();
            }
        }

		public PlayerShip(MyItem owner)
		{
            _owner = owner;
			_bonuses = new PlayerBonuses();
			//_skills = new PlayerSkills();
            this.weapon = new ClientPlayerShipWeapon();
            _powerShield = new ShipPowerShield(owner);
            _mechanicalShield = new ShipMechanicalShield();
            _shipModel = new ClientShipModel();

		}

        #region SHIP PROPERTIES
        public float Health
		{
            get { return _currentHealth; }
		}

        public float MaxHealth {
            get {
                return _maxHealth;
            }
        }
		
		public bool Destroyed
		{
            get 
            { 
                if(Owner() == null)
                {
                    return false;
                }
                return this.Owner().ShipDestroyed;
            }
		}

        private MyItem Owner()
        {
            return this._owner;
        }
		

        public float Acceleration { get { return _acceleration; } }

        public float LinearSpeed { get { return _linearSpeed; } }


		public float HealthPercent
		{
			get {
				if (Mathf.Approximately(_maxHealth, 0.0f))
					return 0.0f;
				else
				{
					return _currentHealth / _maxHealth;
				}
			}
		}

		public PlayerBonuses Bonuses
		{
			get{ return _bonuses; }
		}

        //public PlayerSkills Skills
        //{
        //    get{ return _skills; }
        //}

        public ClientPlayerShipWeapon Weapon
        {
            get { return this.weapon; }
        }
        public float MinLinearSpeed
        {
            get { return _minLinearSpeed;  }
        }

        public float MaxLinearSpeed
        {
            get { return _maxLinearSpeed; }
        }
        public ShipPowerShield PowerShield
        {
            get { return _powerShield;  }
        }

        public float AngleSpeed {
            get {
                return _angleSpeed;
            }
        }

        public float Energy {
            get {
                return _energy;
            }
        }

        public float MaxEnergy {
            get {
                return _maxEnergy;
            }
        }

        public float Energy01 {
            get {
                if (_maxEnergy == 0.0f)
                    return 0.0f;
                return Mathf.Clamp01(_energy / _maxEnergy);
            }
        }

        public ShipMechanicalShield MechanicalShield
        {
            get { return _mechanicalShield; }
        }


        public float Health01 {
            get {
                if (_maxHealth == 0.0f) {
                    //Debug.LogError("max health must be non zero");
                    return 0.0f;
                }
                return Mathf.Clamp01(_currentHealth / _maxHealth);
            }
        }

        public ClientShipModel ShipModel {
            get {
                return _shipModel;
            }
        }
        #endregion


       


        public void ParseProp(string propName, object value )
        {
            if(this.Owner() == null )
            {
                Debug.LogError("Not setted owner for PlayerShip");
                return;
            }

            switch (propName)
            {
                case Props.SHIP_BASE_STATE_HEALTH:
                    {
                        float _old = _currentHealth;
                        _currentHealth = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_MAX_HEALTH:
                    {
                        float _old = _maxHealth;
                        _maxHealth = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_DESTROYED:
                    {
                        this.Owner().SetShipDestroyed((bool)value);

                        if(((bool)value) && this.Owner().ExistsView && this.Owner().View.activeSelf)
                        {
                            this.Owner().SetShipDestroyed(true);
                        }
                        else if(!((bool)value))
                        {
                            if(NeedRespawn())
                            {
                                ResetRespawnFlag();
                                Owner().Respawn();
                            }
                        }
                        Debug.Log("<color=orange>Received player destroyed: {0}</color>".f(this.Owner().ShipDestroyed));
                    }
                    break;

                case Props.SHIP_BASE_STATE_CURRENT_LINEAR_SPEED:
                    {
                        float _old = _linearSpeed;
                        _linearSpeed = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_ACCELERATION:
                    {
                        float _old = _acceleration;
                        _acceleration = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_MIN_LINEAR_SPEED:
                    {
                        float old = _minLinearSpeed;
                        _minLinearSpeed = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_MAX_LINEAR_SPEED:
                    {
                        float old = _maxLinearSpeed;
                        _maxLinearSpeed = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_ANGLE_SPEED:
                    {
                        _angleSpeed = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_ENERGY:
                    {
                        _energy = (float)value;
                    }
                    break;
                case Props.SHIP_BASE_STATE_MAX_ENERGY:
                    {
                        _maxEnergy = (float)value;
                    }
                    break;
            }
        }

        public void ParseProps(Hashtable properties) {
            foreach (DictionaryEntry entry in properties) {
                ParseProp(entry.Key.ToString(), entry.Value);
            }
        }

        public void SetNeedRespawnFlag()
        {
            this.needRespawnFlagSetted = true;
        }

        private bool NeedRespawn()
        {
            return this.needRespawnFlagSetted;
        }
        private void ResetRespawnFlag()
        {
            this.needRespawnFlagSetted = false;
        }

        public float WeaponLightShotTimer01()
        {
            if(this.Weapon == null )
            {
                return 0f;
            }
            return this.Weapon.LightTimer01;
        }

        public float WeaponHeavyShotTimer01()
        {
            if(this.Weapon == null )
            {
                return 0f;
            }
            return this.Weapon.HeavyTimer01;
        }

        public float HeavyShotEnergy()
        {
            if(this.Weapon == null )
            {
                return 0f;
            }
            if(!this.Weapon.HasWeapon)
            {
                return 0f;
            }
            return this.Weapon.WeaponObject.HeavyEnergy;
        }

        public float LightShotEnergy()
        {
            return 0f;
        }

        public float ShotEnergy(ShotType shotType)
        {
            if (shotType == ShotType.Heavy)
                return HeavyShotEnergy();
            else
                return LightShotEnergy();
        }
    }


    public enum ElementState
    {
        on,
        off,
        dead
    }
}