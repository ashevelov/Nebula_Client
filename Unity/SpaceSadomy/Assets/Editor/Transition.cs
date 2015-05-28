
namespace Game.Space.Editor
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Linq;

    public class Transition
    {
        private int toStage;
        private List<Condition> conditions;

        public Transition()
        {
            toStage = 0;
            conditions = new List<Condition>();
        }

		public int ToStage
		{
			get { return toStage; }
			set { toStage = value; }
		}

		public List<Condition> Conditions
		{
			get { return conditions; }
			set { conditions = value; }
		}

        public override string ToString()
        {
            return string.Format("=> {0}, conditions count: {1}", this.ToStage, this.Conditions.Count);
        }

        public XElement ToXElement()
        {
            XElement result = new XElement("transition");
            result.SetAttributeValue("to", this.ToStage.ToString());
            XElement conditions = new XElement("conditions");
            result.Add(conditions);
            foreach(var c in this.Conditions)
            {
                conditions.Add(c.ToXElement());
            }
            return result;
        }

        public static Transition Parse(XElement transitionElement)
        {
            int toStage = int.Parse(transitionElement.Attribute("to").Value);

            List<Condition> conditions = new List<Condition>();
            XElement conditionsElement = transitionElement.Element("conditions");
            if( conditionsElement != null )
            {
                conditions = conditionsElement.Elements("condition").Select(e =>
                    {
                        return Condition.Parse(e);
                    }).ToList();
            }

            Transition result = new Transition();
            result.ToStage = toStage;
            result.Conditions = conditions;
            return result;
        }
    }
}
