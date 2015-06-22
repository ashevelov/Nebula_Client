namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using ServerClientCommon;
    using Common;

    public class MmoTargetComponent : MmoBaseComponent {

        public bool hasTarget {
            get {
                if(item != null ) {
                    bool has = false;
                    if(item.TryGetProperty<bool>((byte)PS.HasTarget, out has)) {
                        return has;
                    }
                }
                return false;
            }
        }

        public string targetID {
            get {
                if(item == null ) {
                    return string.Empty;
                }
                string tID = string.Empty;
                if(!item.TryGetProperty<string>((byte)PS.TargetId, out tID)) {
                    return string.Empty;
                }
                return tID;
            }
        }

        public byte targetType {
            get {
                if(item == null ) { return 0; }
                byte tType;
                if(!item.TryGetProperty<byte>((byte)PS.TargetType, out tType)) {
                    return 0;
                }
                return tType;
            }
        }
    }
}
