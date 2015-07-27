namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;
    using Common;
    using System;

    [AddComponentMenu("Server/Objects/Components/Turret")]
    public class ServerTurretComponent : ServerComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Turret;
            }
        }


    }
}
