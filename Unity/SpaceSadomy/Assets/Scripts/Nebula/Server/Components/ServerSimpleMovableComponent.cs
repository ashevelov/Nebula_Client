namespace Nebula.Server.Components {
    using Common;
    using Nebula.Server.Components;
    using System.Xml.Linq;
    using UnityEngine;
    [AddComponentMenu("Server/Objects/Components/Simple Movable")]
    public class ServerSimpleMovableComponent : ServerComponent {

        public float speed;

        public override ComponentID componentID {
            get {
                return ComponentID.Movable;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("sub_type", ComponentSubType.simple_movable.ToString());
            element.SetAttributeValue("speed", speed.ToString("F2"));
            return element;
        }
    }
}
