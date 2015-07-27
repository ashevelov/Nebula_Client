namespace Nebula.Server.Components {
    using Common;
    using Nebula.Server;
    using System.Xml.Linq;
    using UnityEngine;
    using System;

    [AddComponentMenu("Server/Objects/Components/Patrol Combat AI")]
    public class ServerPatrolAIComponent : ServerBaseAIComponent {

        public AttackMovingType battleMovingType;
        public Vector3 firstPoint;
        public Vector3 secondPoint;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_patrol;
            }
        }

        public override XElement ToXElement() {
            var result =  base.ToXElement();
            result.SetAttributeValue("attack_moving_type", battleMovingType.ToString());
            result.SetAttributeValue("first_point", FormatVector(firstPoint));
            result.SetAttributeValue("second_point", FormatVector(secondPoint));
            return result;
        }
    }
}
