namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class MmoDamagableComponent : MmoBaseComponent {

        public float maxHealth {
            get {
                if(item != null ) {
                    float h;
                    if(item.TryGetProperty<float>((byte)PS.MaxHealth, out h)) {
                        return h;
                    }
                }
                return 0;
            }
        }

        public float health {
            get {
                if(item != null) {
                    float h;
                    if(item.TryGetProperty<float>((byte)PS.CurrentHealth, out h)) {
                        return h;
                    }
                }
                return 0f;
            }
        }

        public bool destroyed {
            get {
                if(item != null ) {
                    bool dest;
                    if(item.TryGetProperty<bool>((byte)PS.Destroyed, out dest)) {
                        return dest;
                    }
                }
                return false;
            }
        }
    }
}
