namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Bot Ship")]
    public class ServerBotShipComponent : ServerMultiComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Ship;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ship_bot;
            }
        }
    }
}
