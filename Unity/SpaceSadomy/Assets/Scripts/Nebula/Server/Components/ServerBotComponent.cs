namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Bot")]
    public class ServerBotComponent : ServerComponent{

        public BotItemSubType subType;

        public override ComponentID componentID {
            get {
                return ComponentID.Bot;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("bot_sub_type", subType.ToString());
            return element;
        }

    }
}
