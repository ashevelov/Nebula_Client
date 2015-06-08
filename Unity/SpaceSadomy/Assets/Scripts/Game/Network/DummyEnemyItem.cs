using Common;
using Game.Space;
using System.Collections;
using UnityEngine;
using Nebula.Client;
using Nebula.Mmo.Games;

namespace Nebula
{
    public class DummyEnemyItem : NpcItem, IDamagable, IBonusHolder
    {

        private BaseSpaceObject component;
        private ForeignShip ship;
        private ActorBonuses bonuses;


        public DummyEnemyItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name, object[] inComponents)
            : base(id, type, game, subType, name, inComponents)
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


        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.Model:
                case PS.MaxHealth:
                case PS.CurrentHealth:
                case PS.Destroyed:
                case PS.CurrentLinearSpeed:
                case PS.ModelInfo:
                case PS.Workshop:
                    ship.ParseProp(key, newValue);
                    break;
                case PS.Bonuses:
                    {
                        Hashtable bonusesProps = newValue as Hashtable;
                        bonuses.Replace(bonusesProps);
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