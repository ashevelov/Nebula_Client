namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class MmoCharacterComponent : MmoBaseComponent {

        public Workshop workshop {
            get {
                if(item != null ) {
                    byte w;
                    if(item.TryGetProperty<byte>((byte)PS.Workshop, out w)) {
                        return (Workshop)w;
                    }
                }
                return Workshop.Arlen;
            }
        }

        public int level {
            get {
                if(item != null ) {
                    int lvl;
                    if(item.TryGetProperty<int>((byte)PS.Level, out lvl)) {
                        return lvl;
                    }
                }
                return 0;
            }
        }

        public FractionType fraction {
            get {
                if(item != null ) {
                    int f;
                    if(item.TryGetProperty<int>((byte)PS.Fraction, out f)) {
                        return (FractionType)f;
                    }
                }
                return FractionType.PlayerHumans;
            }
        }
    }
}