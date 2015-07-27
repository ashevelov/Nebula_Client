namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Target")]
    public class ServerTargetComponent : ServerComponent {
        public override ComponentID componentID {
            get {
                return ComponentID.Target;
            }
        }
    }
}
