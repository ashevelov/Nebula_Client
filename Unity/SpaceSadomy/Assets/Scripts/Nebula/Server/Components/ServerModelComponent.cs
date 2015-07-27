namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;
    [AddComponentMenu("Server/Objects/Components/Model")]
    public class ServerModelComponent : ServerComponent {

        public string model;

        public override ComponentID componentID {
            get {
                return ComponentID.Model;
            }
        }

        public override XElement ToXElement() {
            var element = base.ToXElement();
            element.SetAttributeValue("model", model);
            return element;
        }
    }
}