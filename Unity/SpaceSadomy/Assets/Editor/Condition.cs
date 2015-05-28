namespace Game.Space.Editor
{
    using System.Xml.Linq;

    public class Condition 
    {
        private ConditionType type;
        private string varName;
        private VarType varType;
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public VarType VarType
        {
            get { return varType; }
            set { varType = value; }
        }

        public string VarName
        {
            get { return varName; }
            set { varName = value; }
        }

        public ConditionType Type
        {
            get { return type; }
            set { type = value; }
        }

        public Condition()
        {
            this.type = ConditionType.GEQ;
            this.varName = "unnamed...";
            this.varType = VarType.@int;
            this.value = "0";
        }

		public override string ToString ()
		{
			return string.Format ("[Condition: Value={0}, VarType={1}, VarName={2}, Type={3}]", Value, VarType, VarName, Type);
		}

        public XElement ToXElement()
        {
            XElement result = new XElement("condition");
            result.SetAttributeValue("type", this.Type.ToString());
            result.SetAttributeValue("var_name", this.VarName);
            result.SetAttributeValue("var_type", this.VarType.ToString());
            result.SetAttributeValue("value", this.Value);
            return result;
        }

        public static Condition Parse(XElement conditionElement)
        {
            ConditionType cType = (ConditionType)System.Enum.Parse(typeof(ConditionType), conditionElement.Attribute("type").Value);
            string varName = conditionElement.Attribute("var_name").Value;
            VarType varType = GetVarType(conditionElement.Attribute("var_type").Value);
            string sValue = conditionElement.Attribute("value").Value;

            Condition result = new Condition();
            result.Type = cType;
            result.VarName = varName;
            result.VarType = varType;
            result.Value = sValue;
            return result;
        }

        private static VarType GetVarType(string s)
        {
            switch(s)
            {
                case "int": return VarType.@int;
                case "float": return VarType.@float;
                case "bool": return VarType.@bool;
                default:
                    throw new System.Exception("invalid var type");
            }
        }
    }
}
