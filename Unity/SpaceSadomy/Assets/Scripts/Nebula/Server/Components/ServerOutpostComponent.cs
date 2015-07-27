namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;
    [AddComponentMenu("Server/Objects/Components/Outpost")]
    public class ServerOutpostComponent : ServerComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Outpost;
            }
        }

        public override string ToString() {
            return base.ToString();
        }
    }
}
