namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Main Outpost")]
    public class ServerMainOutpostComponent : ServerComponent {
        public override ComponentID componentID {
            get {
                return ComponentID.MainOutpost;
            }
        }
    }
}
