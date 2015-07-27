namespace Nebula.Server.Components {
    using Common;
    using Nebula.Server;
    using System.Xml.Linq;
    using UnityEngine;
    using System;

    [AddComponentMenu("Server/Objects/Components/Stay Combat AI")]
    public class ServerStayAIComponent : ServerBaseAIComponent {
        public AttackMovingType battleMovingType;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_stay;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("attack_moving_type", battleMovingType.ToString());
            return element;
        }
    }
}
