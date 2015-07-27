namespace Nebula.Server.Components {
    using System.Xml.Linq;
    using Common;
    using UnityEngine;
    [AddComponentMenu("Server/Objects/Components/Ship Damagable")]
    public class ServerShipDamagableComponent : ServerMultiComponent {

        public bool createChestWhenKilled = true;

        public override ComponentID componentID {
            get {
                return ComponentID.Damagable;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.damagable_ship;
            }
        }

        public override XElement ToXElement() {
            var e =  base.ToXElement();
            e.SetAttributeValue("create_chest_when_killed", createChestWhenKilled.ToString());
            return e;
        }
    }
}