using Common;
using Nebula.Client;
using Nebula.Mmo.Games;
using Nebula.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula {
    public class InvasionPortalEnemyItem : NpcItem, IDamagable, IBonusHolder,
        Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject _component;
        private ForeignShip _ship;
        private ActorBonuses bonuses;

        public override BaseSpaceObject Component {
            get { return _component; }
        }

        public ForeignShip Ship {
            get {
                return _ship;
            }
        }

        public InvasionPortalEnemyItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name, object[] inComponents)
            : base(id, type, game, subType, name, inComponents) {
            _ship = new ForeignShip(this);
            this.bonuses = new ActorBonuses();
        }

        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            _component = _view.AddComponent<InvasionPortalEnemy>();
            _component.Initialize(Game, this);
        }*/

        public override void Create(GameObject obj) {
            base.Create(obj);
            _component = _view.AddComponent<InvasionPortalEnemy>();
            _component.Initialize(Game, this);
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
                    _ship.ParseProp(key, newValue);
                    break;
                case PS.BaseDamage:
                case PS.OptimalDistance:
                case PS.WeaponRange:
                case PS.ProbNear2OptimalDistance:
                case PS.ProbFar2OptimalDistance:
                case PS.MaxHitSpeed:
                case PS.MaxFireDistance:
                    _ship.Weapon.ParseProp(key, newValue);
                    break;
                case PS.Bonuses:
                    {
                        Hashtable bons = newValue as Hashtable;
                        if(bons != null ) {
                            bonuses.Replace(bons);
                        }
                    }
                    break;

            }
        }

        public bool IsDead() {
            return _ship.Destroyed;
        }
        public override void UseSkill(Hashtable skillProperties) {
            if (Component && (false == IsDead())) {
                Component.UseSkill(skillProperties);
            }
        }
        public bool IsPowerShieldEnabled() {
            return false;
        }


        public float GetHealth() {
            return _ship.Health;
        }

        public float GetMaxHealth() {
            return _ship.MaxHealth;
        }




        public float GetHealth01() {
            if (_ship.MaxHealth == 0.0f)
                return 0.0f;
            return Mathf.Clamp01(_ship.Health / _ship.MaxHealth);
        }


        public float GetOptimalDistance() {
            return _ship.Weapon.OptimalDistance;
        }

        public float GetRange() {
            return _ship.Weapon.Range;
        }

        public float GetFarHitProb() {
            return _ship.Weapon.FarProb;
        }

        public float GetNearHitProb() {
            return _ship.Weapon.NearProb;
        }

        public float GetMaxHitSpeed() {
            return _ship.Weapon.MaxHitSpeed;
        }

        public float GetMaxFireDistance() {
            return _ship.Weapon.MaxFireDistance;
        }

        public float GetSpeed() {
            return _ship.Speed;
        }


        public float GetNearDist() {
            Debug.LogError("not implemented 5");
            return 0;
        }

        public float GetFarDist() {
            Debug.LogError("not implemented 6");
            return 0;
        }

        public ActorBonuses Bonuses {
            get { return this.bonuses; }
        }

        public override void AdditionalUpdate() {

        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                new SelectedObjectContextMenuView.InputEntry {
                     ButtonText = "Info",
                      ButtonAction = ()=> { Debug.Log("Show chest info"); }
                },
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }
    }
}