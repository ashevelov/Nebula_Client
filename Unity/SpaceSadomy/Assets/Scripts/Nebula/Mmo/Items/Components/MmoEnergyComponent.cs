namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class MmoEnergyComponent : MmoBaseComponent {

        private float mCurrentEnergy = 0f;
        private float mMaxEnergy = 0f;

        public float currentEnergy {
            get {
                return mCurrentEnergy;
            }
        }

        public float maxEnergy {
            get {
                return mMaxEnergy;
            }
        }

        public override void Update() {
            item.TryGetProperty<float>((byte)PS.CurrentEnergy, out mCurrentEnergy);
            item.TryGetProperty<float>((byte)PS.MaxEnergy, out mMaxEnergy);
        }
    }
}