namespace Nebula.Server.Components {
    using Common;
    using UnityEngine;
    using System.Xml.Linq;

    public abstract class ServerComponent : MonoBehaviour {

        public abstract ComponentID componentID { get; }
        public virtual XElement ToXElement() {
            XElement element = new XElement("component");
            element.SetAttributeValue("id", componentID.ToString());
            return element;
        }

        public static string FormatVector(Vector3 pos) {
            return string.Format("{0:F1},{1:F1},{2:F1}", pos.x, pos.y, pos.z);
        }
    }
}