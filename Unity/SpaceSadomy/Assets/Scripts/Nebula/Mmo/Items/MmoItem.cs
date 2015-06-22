namespace Nebula.Mmo.Items {
    using UnityEngine;
    using System.Collections;
    using System;
    using Nebula.Mmo.Games;
    using Nebula.Mmo.Objects;

    public class MmoItem : Item {

        private MmoObject mmoObject;

        public MmoItem(string id, byte type, NetworkGame game, string name, object[] inComponents)
            : base(id, type, game, name, inComponents) {

        }

        public override void Create(GameObject obj) {
            base.Create(obj);
            mmoObject = View.AddComponent<MmoObject>();
            mmoObject.Initialize(Game, this);
        }

        public override bool IsMine {
            get {
                return false;
            }
        }

        public override void AdditionalUpdate() {
            
        }

        public override BaseSpaceObject Component {
            get {
                return mmoObject;
            }
        }
    }

}
