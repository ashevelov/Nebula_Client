namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;
    using System.Xml.Linq;
    using Common;

    [AddComponentMenu("Server/Objects/Components/Outpost Damagable")]
    public class ServerOutpostDamagableComponent : ServerFixedInputDamageDamagableComponent {

        public float additionalHP;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.damagable_outpost;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("additional_hp", additionalHP.ToString("F2"));
            return element;
        }

    }
}