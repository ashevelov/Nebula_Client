using UnityEngine;
using System.Collections;
using Game.Space;
using Common;
using Game.Space.UI;
using Nebula.UI;
using System;
using System.Collections.Generic;
using Nebula.Client;

namespace Nebula
{
    public class StandardNpcCombatItem : NpcItem, IDamagable, IBonusHolder, Nebula.UI.ICombatObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject component;
        private ForeignShip ship;
        private ActorBonuses bonuses;
        private int level;
        private TextureSubCache<string> texSubCache = new TextureSubCache<string>();
        private ClientItemEventInfo eventInfo;

        public StandardNpcCombatItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name)
            : base(id, type, game, subType, name)
        {
            this.ship = new ForeignShip(this);
            this.bonuses = new ActorBonuses();
            this.eventInfo = ClientItemEventInfo.Default;
        }

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.component = this.View.AddComponent<StandardNpcCombatObject>();
            this.component.Initialize(this.Game, this);
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case "me":
                    switch (propName)
                    {
                        case "ship":
                            this.ship.ParseInfo(newValue as Hashtable);
                            break;
                        case "bonuses":
                            this.bonuses.Replace(newValue as Hashtable);
                            break;
                        case "level":
                            this.level = (int)newValue;
                            break;
                    }
                    break;
                case "default":
                    {
                        switch (propName)
                        {
                            case "cur_health":
                                this.ship.ParseProp("cur_health", newValue);
                                break;
                        }
                    }
                    break;
                case GroupProps.event_info:
                    {
                        switch (propName)
                        {
                            case Props.from_event:
                                this.eventInfo.SetFromEvent((bool)newValue);
                                break;
                            case Props.event_id:
                                this.eventInfo.SetEventId((string)newValue);
                                break;
                            case Props.event_world_id:
                                this.eventInfo.SetEventWorldId((string)newValue);
                                break;
                        }
                    }
                    break;
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            base.OnSettedGroupProperties(group, properties);
            foreach (DictionaryEntry entry in properties)
            {
                object oldProp = this.GetProperty(group, entry.Key.ToString());
                this.OnSettedProperty(group, entry.Key.ToString(), entry.Value, oldProp);
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
            if (this.ship != null)
                return this.ship.Health;
            return 0f;
        }

        public float GetMaxHealth()
        {
            if (this.ship != null)
                return this.ship.MaxHealth;
            return float.MaxValue;
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

        public ClientItemEventInfo EventInfo
        {
            get
            {
                return this.eventInfo;
            }
        }

        #region ICombatObjectInfo
        public int Level
        {
            get { return this.level; }
        }

        public float CurrentHealth
        {
            get { return GetHealth(); }
        }

        public float MaxHealth
        {
            get { return GetMaxHealth(); }
        }

        public Sprite Icon
        {
            get
            {
                return SpriteCache.TargetSprite("npc");
            }
        }
        #endregion


        public float HitProb
        {
            get
            {
                return G.GetHitProbTo(this);
            }
        }

        public ObjectInfoType InfoType
        {
            get
            {
                return ObjectInfoType.StandardCombatNpc;
            }
        }

        public string Description
        {
            get
            {
                return "This is npc bot item";
            }
        }

        public Color Relation
        {
            get
            {
                return Color.yellow;
            }
        }

        public float DistanceToPlayer
        {
            get
            {
                return G.DistanceTo(this);
            }
        }
    }
}