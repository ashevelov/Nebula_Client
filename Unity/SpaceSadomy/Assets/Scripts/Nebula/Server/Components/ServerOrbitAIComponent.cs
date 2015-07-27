namespace Nebula.Server.Components {
    using Common;
    using Nebula.Server;
    using System.Xml.Linq;
    using UnityEngine;
    using System;

    [RequireComponent(typeof(ServerShipDamagableComponent))]
    [RequireComponent(typeof(ServerBonusesComponent))]
    [RequireComponent(typeof(ServerRaceableComponent))]
    [AddComponentMenu("Server/Objects/Components/Orbit Combat AI")]
    public class ServerOrbitAIComponent : ServerBaseAIComponent {

        public AttackMovingType battleMovingType;
        public float phiSpeed;
        public float thetaSpeed;
        public float radius;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_orbit;
            }
        }

        public override XElement ToXElement() {
            var result =  base.ToXElement();
            result.SetAttributeValue("attack_moving_type", battleMovingType.ToString());
            result.SetAttributeValue("phi_speed", phiSpeed.ToString("F2"));
            result.SetAttributeValue("theta_speed", thetaSpeed.ToString("F2"));
            result.SetAttributeValue("radius", radius.ToString("F2"));
            return result;
        }
    }
}
