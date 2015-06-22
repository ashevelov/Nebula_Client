using Common;
using Nebula.Client;
using Nebula.Mmo.Games;
using System.Collections;
using UnityEngine;

namespace Nebula.Mmo.Items {
    #region Item
    public class ProtectionStationItem : NpcItem, IDamagable, IBonusHolder {
        private BaseSpaceObject component;
        private ActorBonuses bonuses;
        private readonly ClientNpcShipWeapon weapon;


        private float health;
        private float maxHealth;
        private BotItemSubType subType;
        private bool destroyed;
        private string prefab;
        private float speed;
        private Race race;

        public ProtectionStationItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name, object[] inComponents)
            : base(id, type, game, subType, name, inComponents) {
            this.bonuses = new ActorBonuses();
            this.weapon = new ClientNpcShipWeapon();
        }

        #region Overrides
        public override BaseSpaceObject Component {
            get { return this.component; }
        }



        /*
        public override void CreateView(GameObject prefab)
        {
            Debug.Log("ProtectionStationItem.CreateView(GameObject prefab, bool hasDropContainer)".Color(Color.yellow).Bold());
            //create base view
            base.CreateView(prefab);
            //set component
            this.component = this.View.AddComponent<ProtectionStation>();
            //initialize component
            this.component.Initialize(this.Game, this);
        }*/

        public override void Create(GameObject obj) {
            base.Create(obj);
            this.component = this._view.AddComponent<ProtectionStation>();
            this.component.Initialize(this.Game, this);
        }
        #endregion

        #region IDamagable
        public bool IsDead() {
            return this.destroyed;
        }

        public bool IsPowerShieldEnabled() {
            //no power shield
            return false;
        }

        public float GetHealth() {
            return this.health;
        }

        public float GetMaxHealth() {
            return this.maxHealth;
        }

        public float GetHealth01() {
            if (this.maxHealth == 0.0f)
                return 0.0f;
            return Mathf.Clamp01(this.health / this.maxHealth);
        }

        public float GetOptimalDistance() {
            return this.weapon.OptimalDistance;
        }

        public float GetRange() {
            return this.weapon.Range;
        }

        public float GetFarHitProb() {
            return this.weapon.FarProb;
        }

        public float GetNearHitProb() {
            return this.weapon.NearProb;
        }

        public float GetMaxHitSpeed() {
            return this.weapon.MaxHitSpeed;
        }

        public float GetSpeed() {
            return this.speed;
        }

        public float GetNearDist() {
            return this.weapon.NearDist;
        }

        public float GetFarDist() {
            return this.weapon.FarDist;
        }
        #endregion

        #region IBonusHolder
        public ActorBonuses Bonuses {
            get { return this.bonuses; }
        }
        #endregion

        public float Health { get { return this.health; } }
        public float MaxHealth { get { return this.maxHealth; } }
        public string Prefab { get { return this.prefab; } }
        public Race Race { get { return this.race; } }

        public override void AdditionalUpdate() {

        }
    }
    #endregion 
}