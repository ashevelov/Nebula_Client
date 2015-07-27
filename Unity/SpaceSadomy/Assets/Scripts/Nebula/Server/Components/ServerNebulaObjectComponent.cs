namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;

    [AddComponentMenu("Server/Objects/Components/Nebula Object")]
    public class ServerNebulaObjectComponent : ServerComponent  {

        public ItemType itemType;
        public TagElement[] tags;
        public string badge = "";
        public string script = "";

        public override ComponentID componentID {
            get {
                return ComponentID.NebulaObject;
            }
        }

        public override XElement ToXElement() {

            var element = base.ToXElement();
            element.SetAttributeValue("item_type", itemType.ToString());

            if (badge == null) { badge = string.Empty; }
            element.SetAttributeValue("badge", badge);

            if(tags != null && tags.Length > 0) {
                foreach (var tag in tags) {
                    XElement tagElement = new XElement("tag");
                    tagElement.SetAttributeValue("key", tag.key.ToString());
                    tagElement.SetAttributeValue("value", tag.value);
                    tagElement.SetAttributeValue("type", tag.tagType.ToString());
                    element.Add(tagElement);
                }
            }
            return element;
        }
    }

    [System.Serializable]
    public class TagElement {
        public byte key;
        public string value;
        public TagType tagType;
    }
}