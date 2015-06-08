namespace Nebula.Mmo.Items.Components {
    using UnityEngine;
    using System.Collections;
    using Common;

    public class MmoModelComponent : MmoBaseComponent {

        public string modelId {
            get {
                if(item != null ) {
                    string mid;
                    if(item.TryGetProperty<string>((byte)PS.Model, out mid)) {
                        return mid;
                    }
                }
                return null;
            }
        }
    }
}
