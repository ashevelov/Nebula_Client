namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Respawn")]
    public class ServerRespawnableComponent : ServerComponent {

        public float interval;

        public override ComponentID componentID {
            get {
                return ComponentID.Respawnable;
            }
        }

        public override XElement ToXElement() {
            var element = base.ToXElement();
            element.SetAttributeValue("interval", interval.ToString("F2"));
            return element;
        }
    }
}
