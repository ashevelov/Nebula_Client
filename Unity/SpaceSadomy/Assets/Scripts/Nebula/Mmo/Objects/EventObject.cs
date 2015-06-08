using UnityEngine;
using System.Collections;
using System;

namespace Nebula.Mmo.Objects {
    public class EventObject : BaseSpaceObject {

        private float mTimer = 2.3f;


        public override void OnDestroy() {
            Debug.Log("EventObject.OnDestroy()");
        }

        public override void Update() {
            base.Update();
            mTimer -= Time.deltaTime;
            if(mTimer <= 0f ) {
                mTimer = 2.3f;
                Item.GetProperties();
            }
        }
    }
}
