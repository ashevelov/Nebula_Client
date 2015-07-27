namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;
    using System;
    using System.Xml.Linq;

    [AddComponentMenu("Server/Objects/Components/Wander Point Combat AI")]
    public class ServerWanderPointAIComponent : ServerBaseAIComponent {
        public AttackMovingType battleMovingType;
        public float radius;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_wander_point;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("attack_moving_type", battleMovingType.ToString());
            element.SetAttributeValue("radius", radius.ToString("F2"));
            return element;
        }

    }
}
