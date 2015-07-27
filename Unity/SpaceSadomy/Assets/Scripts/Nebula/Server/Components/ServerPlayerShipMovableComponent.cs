namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Player Ship Movable")]
    public class PlayerShipMovableComponent : ServerComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Movable;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("sub_type", ComponentSubType.player_ship_movable.ToString());
            return element;
        }
    }
}
