
namespace Nebula.Mmo.Items {
    using UnityEngine;
    using System.Collections;
    using Nebula.UI;
    using System;
    using Nebula.Mmo.Objects;
    using Nebula.Mmo.Games;
    using System.Collections.Generic;
    using Common;
    using ServerClientCommon;

    public class EventItem : Item, ISelectedObjectContextMenuViewSource {

        private EventObject mEventObject;

        public EventItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents) {

        }

        public override void Create(GameObject obj) {
            base.Create(obj);
            mEventObject = View.AddComponent<EventObject>();
            mEventObject.Initialize(Game, this);
        }

        public override bool IsMine {
            get {
                return false;
            }
        }


        public override void AdditionalUpdate() {
            
        }

        #region ISelectedObjectContextMenuViewSource implementation
        public SelectedObjectContextMenuView.InputData ContextViewData() {
            var entries = new List<SelectedObjectContextMenuView.InputEntry> {
                new SelectedObjectContextMenuView.InputEntry {
                    ButtonText = "View Info",
                    ButtonAction = () => {
                        Debug.Log("Here need show popup message window with event description".Color("green"));
                    }
                }
            };
            return new SelectedObjectContextMenuView.InputData { TargetItem = this, Inputs = entries };
        }
        #endregion

        public string eventId {
            get {
                Hashtable eventProperties = GetProperty<Hashtable>((byte)PS.Event);
                if(eventProperties == null ) {
                    return string.Empty;
                }
                return eventProperties.Value<string>((int)SPC.Id);
            }
        }

        public bool eventActive {
            get {
                Hashtable eventProperties = GetProperty<Hashtable>((byte)PS.Event);
                if (eventProperties == null) {
                    return false;
                }
                return eventProperties.Value<bool>((int)SPC.Active, false);
            }
        }
    }
}
