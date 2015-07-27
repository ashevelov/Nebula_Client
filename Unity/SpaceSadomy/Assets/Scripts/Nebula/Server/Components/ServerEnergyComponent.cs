namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;

    public class ServerEnergyComponent : ServerComponent {

        [AddComponentMenu("Server/Objects/Components/Energy")]
        public override ComponentID componentID {
            get {
                return ComponentID.Energy;
            }
        }
    }
}
