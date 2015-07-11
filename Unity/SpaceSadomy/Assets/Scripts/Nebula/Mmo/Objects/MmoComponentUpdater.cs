namespace Nebula.Mmo.Objects {
    using UnityEngine;
    using System.Collections;
    using Nebula.Mmo.Items;

    public class MmoComponentUpdater : MonoBehaviour {

        private Item mItem;

        public void SetItem(Item it) { mItem = it; }

        private float mTimer;

        void Start() {
            mTimer = 1;
        }

        void Update() {
            if(mItem == null) { return; }

            mTimer -= Time.deltaTime;
            if (mTimer <= 0f) {

                mTimer = 1f;

                foreach (var comp in mItem.components) {
                    comp.Value.Update();
                }
            }
        }
    }
}
