namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Simple Weapon")]
    [RequireComponent(typeof(ServerTargetComponent))]
    [RequireComponent(typeof(ServerBotCharacterComponent))]
    public class SimpleWeaponComponent : ServerMultiComponent {
        public float optimalDistance;
        public float damage;
        public float cooldown;
        public bool useTargetHP = false;
        public float targetHPPercent = 0f;


        public override ComponentID componentID {
            get {
                return ComponentID.Weapon;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.weapon_simple;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("optimal_distance", optimalDistance.ToString("F2"));
            element.SetAttributeValue("damage", optimalDistance.ToString("F2"));
            element.SetAttributeValue("cooldown", cooldown.ToString("F2"));
            element.SetAttributeValue("use_target_hp", useTargetHP.ToString());
            element.SetAttributeValue("target_hp_percent", targetHPPercent.ToString("F4"));
            return element;
        }

    }
}
