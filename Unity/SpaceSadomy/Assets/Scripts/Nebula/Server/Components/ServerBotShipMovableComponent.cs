namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;
    [AddComponentMenu("Server/Objects/Components/Ship Movable Bot")]
    public class ServerBotShipMovableComponent : ServerComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Movable;
            }
        }

        public override XElement ToXElement() {
            var result =  base.ToXElement();
            result.SetAttributeValue("sub_type", ComponentSubType.bot_ship_movable.ToString());
            return result;
        }
    }
}
