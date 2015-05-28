using UnityEngine;
using System.Collections;
using Game.Space.UI;
using Game.Space;
using Common;
using Nebula.UI;
using System;
using System.Collections.Generic;
using Nebula.Client;

namespace Nebula
{
    public class PirateStationItem : NpcItem, IDamagable, ICombatObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {
        private BaseSpaceObject component;
        private TextureSubCache<string> texSubCache = new TextureSubCache<string>();
        private ClientItemEventInfo eventInfo;
        private float currentHealth;
        private float maximumHealth;



        public PirateStationItem(string id, byte type, NetworkGame game, BotItemSubType botSubType, string name)
            : base(id, type, game, botSubType, name)
        {
            this.eventInfo = ClientItemEventInfo.Default;
        }

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            this.component = this.View.AddComponent<PirateStation>();
            this.Component.Initialize(this.Game, this);
        }

        public override BaseSpaceObject Component
        {
            get { return this.component; }
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case "default":
                    {
                        switch (propName)
                        {
                            case "cur_health":
                                {
                                    this.currentHealth = (float)newValue;
                                }
                                break;
                        }
                    }
                    break;
                case "me":
                    {
                        switch (propName)
                        {
                            case "ship":
                                {
                                    Hashtable shipInfo = (Hashtable)newValue;
                                    foreach (DictionaryEntry entry in shipInfo)
                                    {
                                        switch (entry.Key.ToString())
                                        {
                                            case "max_health":
                                                {
                                                    this.maximumHealth = (float)entry.Value;
                                                }
                                                break;
                                            case "destroyed":
                                                {
                                                    //Debug.Log("DESTROYED TYPE:" + newValue.GetType().ToString());
                                                    //Debug.Log("DESTROYED VALUE: " + newValue);
                                                    this.SetShipDestroyed((bool)entry.Value);
                                                }
                                                break;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case GroupProps.event_info:
                    {
                        switch (propName)
                        {
                            case Props.from_event:
                                {
                                    this.eventInfo.SetFromEvent((bool)newValue);
                                }
                                break;
                            case Props.event_id:
                                {
                                    this.eventInfo.SetEventId((string)newValue);
                                }
                                break;
                            case Props.event_world_id:
                                {
                                    this.eventInfo.SetEventWorldId((string)newValue);
                                }
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

        public override void UseSkill(Hashtable skillProperties) { }

        public override void AdditionalUpdate() { }

        #region IDamagable
        public bool IsDead()
        {
            return this.ShipDestroyed;
        }

        public bool IsPowerShieldEnabled()
        {
            return false;
        }

        public float GetHealth()
        {
            return this.currentHealth;
        }

        public float GetMaxHealth()
        {
            return this.maximumHealth;
        }

        public float GetHealth01()
        {
            return (this.maximumHealth == 0f) ? 0f : Mathf.Clamp01(this.GetHealth() / this.GetMaxHealth());
        }

        public float GetOptimalDistance()
        {
            return 0f;
        }

        public float GetRange()
        {
            return 0f;
        }

        public float GetMaxHitSpeed()
        {
            return 0f;
        }

        public float GetSpeed()
        {
            return 0f;
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
        #endregion

        #region ICombatObjectInfo
        public int Level
        {
            get { return 1; }
        }

        public float CurrentHealth
        {
            get { return this.GetHealth(); }
        }

        public float MaxHealth
        {
            get { return this.GetMaxHealth(); }
        }

        public float HitProb
        {
            get { return G.GetHitProbTo(this); }
        }

        public Sprite Icon
        {
            get { return SpriteCache.TargetSprite("station"); }
        }

        public ObjectInfoType InfoType
        {
            get { return ObjectInfoType.PirateStation; }
        }

        public string Description
        {
            get { return string.Empty; }
        }

        public Color Relation
        {
            get { return Color.red; }
        }

        public float DistanceToPlayer
        {
            get { return G.DistanceTo(this); }
        }
        #endregion
    }
}