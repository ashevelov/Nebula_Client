namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Bonuses")]
    public class ServerBonusesComponent : ServerComponent {

        public override ComponentID componentID {
            get {
                return ComponentID.Bonuses;
            }
        }

        public override XElement ToXElement() {
            return base.ToXElement();
        }
    }
}
