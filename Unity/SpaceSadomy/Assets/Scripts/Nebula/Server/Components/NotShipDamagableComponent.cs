namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Destructable Not Ship Object")]
    [RequireComponent(typeof(ServerBonusesComponent))]
    public class NotShipDamagableComponent : ServerMultiComponent {

        public float maxHealth;
        public bool ignoreDamageAtstart;
        public float ignoreDamageInterval;
        public bool createChestWhenKilled = true;

        public override ComponentID componentID {
            get {
                return ComponentID.Damagable;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.damagable_not_ship;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("max_health", maxHealth.ToString("F2"));
            element.SetAttributeValue("ignore_damage_at_start", ignoreDamageAtstart.ToString());
            element.SetAttributeValue("ignore_damage_interval", ignoreDamageInterval.ToString("F2"));
            element.SetAttributeValue("create_chest_when_killed", createChestWhenKilled.ToString());
            return element;
        }
    }
}