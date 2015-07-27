namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Bot Character")]
    public class ServerBotCharacterComponent : ServerMultiComponent {

        public Workshop workshop;
        public int level;
        public FractionType fraction;

        public override ComponentID componentID {
            get {
                return ComponentID.Character;
            }
        }

        public override ComponentSubType subType {
            get {
                return ComponentSubType.character_bot;
            }
        }


        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("workshop", workshop.ToString());
            element.SetAttributeValue("level", level.ToString());
            element.SetAttributeValue("fraction", fraction.ToString());
            return element;
        }
    }
}
