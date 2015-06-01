using Common;
using Nebula.Mmo.Games;
using Nebula.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nebula {
    public class InvasionPortalItem : NpcItem, Nebula.UI.ISelectedObjectContextMenuViewSource {

        private BaseSpaceObject _component;
        private int _model;

        public override BaseSpaceObject Component {
            get { return _component; }
        }

        public InvasionPortalItem(string id, byte type, NetworkGame game, BotItemSubType subType, string name)
            : base(id, type, game, subType, name) {
        }
        /*
        public override void CreateView(GameObject prefab)
        {
            base.CreateView(prefab);
            _component = _view.AddComponent<InvasionPortal>();
            _component.Initialize(Game, this);
        }*/

        public override void Create(GameObject obj) {
            base.Create(obj);
            _component = _view.AddComponent<InvasionPortal>();
            _component.Initialize(Game, this);
        }

        public override void OnPropertySetted(byte key, object oldValue, object newValue) {
            switch((PS)key) {
                case PS.Model:
                    _model = (int)newValue;
                    break;
            }
        }


        public override void UseSkill(Hashtable skillProperties) {
        }

        public int Model {
            get {
                return _model;
            }
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