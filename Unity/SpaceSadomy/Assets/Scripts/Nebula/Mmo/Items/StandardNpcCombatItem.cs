using Common;
using Nebula.Client;
using Nebula.Mmo.Games;
using Nebula.Resources;
using Nebula.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula.Mmo.Items {
    public class StandardNpcCombatItem : NpcItem, IDamagable, IBonusHolder, Nebula.UI.ICombatObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject component;
        private ForeignShip ship;
        private ActorBonuses bonuses;
        private int level;
        private TextureSubCache<string> texSubCache = new TextureSubCache<string>();
        private ClientItemEventInfo eventInfo;

        public StandardNpcCombatItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name, object[] inComponents)
            : base(id, type, game, subType, name, inComponents) {
            this.ship = new ForeignShip(this);
            this.bonuses = new ActorBonuses();
            this.eventInfo = ClientItemEventInfo.Default;
        }

        public override void Create(GameObject obj) {
            base.Create(obj);
            this.component = this.View.AddComponent<StandardNpcCombatObject>();
            this.component.Initialize(this.Game, this);
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.Ship:
                    ship.ParseInfo(newValue as Hashtable);
                    break;
                case PS.Bonuses:
                    bonuses.Replace(newValue as Hashtable);
                    break;
                case PS.Level:
                    level = (int)newValue;
                    break;
                case PS.FromEvent:
                    eventInfo.SetFromEvent((bool)newValue);
                    break;
                case PS.EventId:
                    eventInfo.SetEventId((string)newValue);
                    break;
                case PS.EventWorldId:
                    eventInfo.SetEventWorldId((string)newValue);
                    break;
                case PS.ModelInfo:
                case PS.CurrentHealth:
                case PS.MaxHealth:
                case PS.Destroyed:
                case PS.Workshop:
                    ship.ParseProp(key, newValue);
                    break;
            }
        }


        public override BaseSpaceObject Component {
            get { return component; }
        }

        public ForeignShip Ship {
            get { return this.ship; }
        }

        public bool IsDead() {
            return this.ship.Destroyed;
        }

        public bool IsPowerShieldEnabled() {
            return false;
        }

        public float GetHealth() {
            if (this.ship != null)
                return this.ship.Health;
            return 0f;
        }

        public float GetMaxHealth() {
            if (this.ship != null)
                return this.ship.MaxHealth;
            return float.MaxValue;
        }

        public float GetHealth01() {
            if (this.ship.MaxHealth == 0.0f)
                return 0.0f;
            return Mathf.Clamp01(this.ship.Health / this.ship.MaxHealth);
        }

        public float GetOptimalDistance() {
            return this.ship.Weapon.OptimalDistance;
        }

        public float GetRange() {
            return this.ship.Weapon.Range;
        }

        public float GetFarHitProb() {
            return this.ship.Weapon.FarProb;
        }

        public float GetNearHitProb() {
            return this.ship.Weapon.NearProb;
        }

        public float GetMaxHitSpeed() {
            return this.ship.Weapon.MaxHitSpeed;
        }

        public float GetMaxFireDistance() {
            return this.ship.Weapon.MaxFireDistance;
        }

        public float GetSpeed() {
            return this.ship.Speed;
        }



        public float GetNearDist() {
            Debug.LogError("not implemented");
            return 0;
        }

        public float GetFarDist() {
            Debug.LogError("not implemented 2");
            return 0;
        }

        public ActorBonuses Bonuses {
            get { return this.bonuses; }
        }

        public override void AdditionalUpdate() {

        }

        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                //new SelectedObjectContextMenuView.InputEntry {
                //     ButtonText = "Info",
                //      ButtonAction = ()=> { Debug.Log("Show chest info"); }
                //},
            };
            return new SelectedObjectContextMenuView.InputData {
                TargetItem = this,
                Inputs = entries
            };
        }

        public ClientItemEventInfo EventInfo {
            get {
                return this.eventInfo;
            }
        }

        #region ICombatObjectInfo
        public int Level {
            get { return this.level; }
        }

        public float CurrentHealth {
            get { return GetHealth(); }
        }

        public float MaxHealth {
            get { return GetMaxHealth(); }
        }

        public Sprite Icon {
            get {
                return SpriteCache.TargetSprite("npc");
            }
        }
        #endregion


        public float HitProb {
            get {
                return G.GetHitProbTo(this);
            }
        }

        public override ObjectInfoType InfoType {
            get {
                return ObjectInfoType.StandardCombatNpc;
            }
        }

        public override  string Description {
            get {
                return "This is npc bot item";
            }
        }
    }
}