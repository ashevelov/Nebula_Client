//using Common;
//using Nebula.Client;
//using Nebula.Mmo.Games;
//using Nebula.UI;
//using ServerClientCommon;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Nebula {
//    public class PirateStationItem : NpcItem, IDamagable, ICombatObjectInfo, Nebula.UI.ISelectedObjectContextMenuViewSource {
//        private BaseSpaceObject component;
//        private TextureSubCache<string> texSubCache = new TextureSubCache<string>();
//        private ClientItemEventInfo eventInfo;
//        private float currentHealth;
//        private float maximumHealth;



//        public PirateStationItem(string id, byte type, NetworkGame game, BotItemSubType botSubType, string name, object[] inComponents)
//            : base(id, type, game, botSubType, name, inComponents) {
//            this.eventInfo = ClientItemEventInfo.Default;
//        }

//        public override void Create(GameObject obj) {
//            base.Create(obj);
//            this.component = this.View.AddComponent<PirateStation>();
//            this.Component.Initialize(this.Game, this);
//        }

//        public override BaseSpaceObject Component {
//            get { return this.component; }
//        }


//        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
//            switch((PS)key) {
//                case PS.CurrentHealth:
//                    this.currentHealth = (float)newValue;
//                    break;
//                case PS.Ship:
//                    {
//                        Hashtable shipInfo = (Hashtable)newValue;
//                        foreach (DictionaryEntry entry in shipInfo) {
//                            switch ((SPC)(int)entry.Key) {
//                                case SPC.MaxHealth:
//                                    {
//                                        this.maximumHealth = (float)entry.Value;
//                                    }
//                                    break;
//                                case SPC.Destroyed:
//                                    {
//                                        //Debug.Log("DESTROYED TYPE:" + newValue.GetType().ToString());
//                                        //Debug.Log("DESTROYED VALUE: " + newValue);
//                                        this.SetShipDestroyed((bool)entry.Value);
//                                    }
//                                    break;
//                            }
//                        }
//                    }
//                    break;
//                case PS.FromEvent:
//                    {
//                        this.eventInfo.SetFromEvent((bool)newValue);
//                    }
//                    break;
//                case PS.EventId:
//                    {
//                        this.eventInfo.SetEventId((string)newValue);
//                    }
//                    break;
//                case PS.EventWorldId:
//                    {
//                        this.eventInfo.SetEventWorldId((string)newValue);
//                    }
//                    break;
//            }
//        }


//        public override void UseSkill(Hashtable skillProperties) { }

//        public override void AdditionalUpdate() { }

//        #region IDamagable
//        public bool IsDead() {
//            return this.ShipDestroyed;
//        }

//        public bool IsPowerShieldEnabled() {
//            return false;
//        }

//        public float GetHealth() {
//            return this.currentHealth;
//        }

//        public float GetMaxHealth() {
//            return this.maximumHealth;
//        }

//        public float GetHealth01() {
//            return (this.maximumHealth == 0f) ? 0f : Mathf.Clamp01(this.GetHealth() / this.GetMaxHealth());
//        }

//        public float GetOptimalDistance() {
//            return 0f;
//        }

//        public float GetRange() {
//            return 0f;
//        }

//        public float GetMaxHitSpeed() {
//            return 0f;
//        }

//        public float GetSpeed() {
//            return 0f;
//        }

//        public SelectedObjectContextMenuView.InputData ContextViewData() {
//            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
//                new SelectedObjectContextMenuView.InputEntry {
//                     ButtonText = "Info",
//                      ButtonAction = ()=> { Debug.Log("Show chest info"); }
//                },
//            };
//            return new SelectedObjectContextMenuView.InputData {
//                TargetItem = this,
//                Inputs = entries
//            };
//        }
//        #endregion

//        #region ICombatObjectInfo
//        public int Level {
//            get { return 1; }
//        }

//        public float CurrentHealth {
//            get { return this.GetHealth(); }
//        }

//        public float MaxHealth {
//            get { return this.GetMaxHealth(); }
//        }

//        public float HitProb {
//            get { return G.GetHitProbTo(this); }
//        }

//        public Sprite Icon {
//            get { return SpriteCache.TargetSprite("station"); }
//        }

//        public ObjectInfoType InfoType {
//            get { return ObjectInfoType.PirateStation; }
//        }

//        public string Description {
//            get { return string.Empty; }
//        }

//        public Color Relation {
//            get { return Color.red; }
//        }

//        public float DistanceToPlayer {
//            get { return G.DistanceTo(this); }
//        }
//        #endregion
//    }
//}