using Common;
using Game.Space;
using System.Collections;
using UnityEngine;
using Nebula.Client;

namespace Nebula
{
    public class DummyEnemyItem : NpcItem, IDamagable, IBonusHolder
    {

        private BaseSpaceObject component;
        private ForeignShip ship;
        private ActorBonuses bonuses;


        public DummyEnemyItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name)
            : base(id, type, game, subType, name)
        {
            this.ship = new ForeignShip(this);
            this.bonuses = new ActorBonuses();
        }

        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            this.component = this.View.AddComponent<DummyEnemy>();
            this.component.Initialize(this.Game, this);
        }*/

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.component = this.View.AddComponent<DummyEnemy>();
            this.component.Initialize(this.Game, this);
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    this.ship.ParseProp(propName, newValue);
                    break;
                case GroupProps.DEFAULT_STATE:
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    break;
                case GroupProps.BONUSES:
                    {
                        if (propName == Props.BONUSES)
                        {
                            Hashtable bons = newValue as Hashtable;
                            if (bons != null)
                            {
                                this.bonuses.Replace(bons);
                            }
                        }
                    }
                    break;
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    this.ship.ParseProps(properties);
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    break;
                case GroupProps.BONUSES:
                    {
                        Hashtable bonusesProps = properties.GetValue<Hashtable>(Props.BONUSES, new Hashtable());
                        this.bonuses.Replace(bonusesProps);
                    }
                    break;
            }
        }


        public override BaseSpaceObject Component
        {
            get { return component; }
        }

        public ForeignShip Ship
        {
            get { return this.ship; }
        }

        public bool IsDead()
        {
            return this.ship.Destroyed;
        }

        public bool IsPowerShieldEnabled()
        {
            return false;
        }

        public float GetHealth()
        {
            return this.ship.Health;
        }

        public float GetMaxHealth()
        {
            return this.ship.MaxHealth;
        }

        public float GetHealth01()
        {
            if (this.ship.MaxHealth == 0.0f)
                return 0.0f;
            return Mathf.Clamp01(this.ship.Health / this.ship.MaxHealth);
        }

        public float GetOptimalDistance()
        {
            return this.ship.Weapon.OptimalDistance;
        }

        public float GetRange()
        {
            return this.ship.Weapon.Range;
        }

        public float GetFarHitProb()
        {
            return this.ship.Weapon.FarProb;
        }

        public float GetNearHitProb()
        {
            return this.ship.Weapon.NearProb;
        }

        public float GetMaxHitSpeed()
        {
            return this.ship.Weapon.MaxHitSpeed;
        }

        public float GetMaxFireDistance()
        {
            return this.ship.Weapon.MaxFireDistance;
        }

        public float GetSpeed()
        {
            return this.ship.Speed;
        }

        public override void UseSkill(Hashtable skillProperties)
        {
            //throw new System.NotImplementedException();
        }


        public float GetNearDist()
        {
            Debug.LogError("not implemented");
            return 0;
        }

        public float GetFarDist()
        {
            Debug.LogError("not implemented 2");
            return 0;
        }

        public ActorBonuses Bonuses
        {
            get { return this.bonuses; }
        }

        public override void AdditionalUpdate()
        {

        }
    }
}