namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;

    public abstract class ServerBaseAIComponent : ServerMultiComponent {

        public bool alignWithForwardDirection = true;
        public float rotationSpeed = 0.5f;

        public override ComponentID componentID {
            get {
                return ComponentID.CombatAI;
            }
        }

        public override XElement ToXElement() {
            var element =  base.ToXElement();
            element.SetAttributeValue("align_with_forward_direction", alignWithForwardDirection.ToString());
            element.SetAttributeValue("rotation_speed", rotationSpeed.ToString("F3"));
            return element;
        }
    }
}
