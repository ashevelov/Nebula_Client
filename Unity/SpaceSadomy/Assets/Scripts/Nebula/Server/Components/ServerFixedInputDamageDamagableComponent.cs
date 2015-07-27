namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;
    using System.Xml.Linq;

    [AddComponentMenu("Server/Objects/Components/Fixed Input Damage Damagable")]
    public class ServerFixedInputDamageDamagableComponent : NotShipDamagableComponent {

        public float fixedInputDamage = 50;

        public override ComponentSubType subType {
            get {
                return ComponentSubType.damagable_fixed_damage;
            }
        }

        public override XElement ToXElement() {
            var e =  base.ToXElement();
            e.SetAttributeValue("fixed_input_damage", fixedInputDamage.ToString("F2"));
            return e;
        }
    }
}
