using UnityEngine;

namespace Nebula {
    public class ProtectionStation : BaseSpaceObject {

        private float updateNextTime;

        public override void Start() {
            base.Start();
            this.updateNextTime = Time.time;
        }

        public override void OnDestroy() {
            print("ProtectionStation.OnDetsroy()");
            //this.DestroySkillAndBonusesEffects();
        }

        public override void Update() {
            if (this.Item != null) {
                this.UpdateProperties();
            }
            base.Update();
        }

        private void UpdateProperties() {
            if (Time.time > this.updateNextTime) {
                this.updateNextTime = Time.time + 1.0f;
                this.Item.GetProperties();
            }
        }
    }



}