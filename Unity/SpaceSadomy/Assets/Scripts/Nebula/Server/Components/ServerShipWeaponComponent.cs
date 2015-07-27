namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Ship Weapon")]
    public class ServerShipWeaponComponent : ServerMultiComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Weapon;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.weapon_ship;
            }
        }
    }
}
