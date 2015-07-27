namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Race")]
    public class ServerRaceableComponent : ServerComponent {

        public Race race;

        public override ComponentID componentID {
            get {
                return ComponentID.Raceable;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("race", race.ToString());
            return element;
        }
    }
}
