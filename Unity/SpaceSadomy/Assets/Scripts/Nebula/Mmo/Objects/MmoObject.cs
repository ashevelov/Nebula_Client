namespace Nebula.Mmo.Objects {
    using UnityEngine;
    using System.Collections;
    using System;

    public class MmoObject : BaseSpaceObject {

        private float mTimer = 3;

        public override void OnDestroy() {
            
        }

        public override void Update() {
            base.Update();
            mTimer -= Time.deltaTime;
            if(mTimer <= 0f) {
                mTimer = 3;
                Item.GetProperties();
            }
        }
    }
}