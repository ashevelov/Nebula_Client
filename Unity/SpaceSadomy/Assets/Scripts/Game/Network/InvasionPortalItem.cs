using UnityEngine;
using System.Collections;
using Common;
using Nebula.UI;
using System;
using System.Collections.Generic;

namespace Nebula
{
    public class InvasionPortalItem : NpcItem, Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject _component;
        private int _model;

        public override BaseSpaceObject Component
        {
            get { return _component; }
        }

        public InvasionPortalItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name)
            : base(id, type, game, subType, name)
        {
        }
        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            _component = _view.AddComponent<InvasionPortal>();
            _component.Initialize(Game, this);
        }*/

        public override void Create(GameObject obj)
        {
            base.Create(obj);
            _component = _view.AddComponent<InvasionPortal>();
            _component.Initialize(Game, this);
        }

        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch (group)
            {
                case GroupProps.SHIP_BASE_STATE:
                    switch (propName)
                    {
                        case Props.SHIP_BASE_STATE_MODEL:
                            _model = (int)newValue;
                            break;
                    }
                    break;
            }
        }

        public override void OnSettedGroupProperties(string group, Hashtable properties)
        {
            if (group == GroupProps.SHIP_BASE_STATE)
            {
                if (properties.ContainsKey(Props.SHIP_BASE_STATE_MODEL))
                {
                    _model = (int)properties[Props.SHIP_BASE_STATE_MODEL];
                }
            }
            base.OnSettedGroupProperties(group, properties);
        }

        public override void UseSkill(Hashtable skillProperties)
        {
        }

        public int Model
        {
            get
            {
                return _model;
            }
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
    }

}