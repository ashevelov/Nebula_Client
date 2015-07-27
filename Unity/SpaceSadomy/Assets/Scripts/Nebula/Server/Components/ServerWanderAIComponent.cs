namespace Nebula.Server.Components {
    using Common;
    using Nebula.Server;
    using System.Xml.Linq;
    using UnityEngine;
    using System;

    [AddComponentMenu("Server/Objects/Components/Wander Combat AI")]
    public class ServerWanderAIComponent : ServerBaseAIComponent {

        public AttackMovingType battleMovingType;
        public Vector3 min;
        public Vector3 max;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_wander_cube;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("attack_moving_type", battleMovingType.ToString());
            element.SetAttributeValue("min", FormatVector(min));
            element.SetAttributeValue("max", FormatVector(max));
            return element;
        }
    }
}
