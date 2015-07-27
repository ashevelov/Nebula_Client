namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;
    using System.Xml.Linq;

    public abstract class ServerMultiComponent : ServerComponent {

        public abstract ComponentSubType subType { get; }

        public override XElement ToXElement() {
            var element = base.ToXElement();
            element.SetAttributeValue("sub_type", subType.ToString());
            return element;
        }
    }
}
