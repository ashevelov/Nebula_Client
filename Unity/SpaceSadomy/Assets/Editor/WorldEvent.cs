namespace Game.Space.Editor
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Xml.Linq;
    using System.Linq;

    public class WorldEvent
    {
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private int cooldown;

        public int Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }
        private int rewardExp;

        public int RewardExp
        {
            get { return rewardExp; }
            set { rewardExp = value; }
        }
        private int rewardCoins;

        public int RewardCoins
        {
            get { return rewardCoins; }
            set { rewardCoins = value; }
        }
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private List<EventStage> stages;

        public List<EventStage> Stages
        {
            get { return stages; }
            set { stages = value; }
        }

        public WorldEvent()
        {
            id = string.Empty;
            name = string.Empty;
            description = string.Empty;
            cooldown = 0;
            rewardExp = 0;
            rewardCoins = 0;
            position = Vector3.zero;
            stages = new List<EventStage>();
        }

        public XElement ToXElement()
        {
            XElement result = new XElement("event");
            result.SetAttributeValue("id", this.Id);
            result.SetAttributeValue("name", this.Name);
            result.SetAttributeValue("description", this.Description);
            result.SetAttributeValue("cooldown", this.Cooldown.ToString());
            result.SetAttributeValue("reward_exp", this.RewardExp.ToString());
            result.SetAttributeValue("reward_coins", this.RewardCoins.ToString());
            result.SetAttributeValue("position", string.Format("{0:F1},{1:F1},{2:F1}", this.Position.x, this.Position.y, this.Position.z));

            XElement stages = new XElement("stages");
            result.Add(stages);
            foreach(var s in this.Stages)
            {
                stages.Add(s.ToXElement());
            }
            return result;
        }

        public static WorldEvent Parse(XElement element)
        {
            string sId = element.Attribute("id").Value;
            string sName = element.Attribute("name").Value;
            string sDescription = element.Attribute("description").Value;
            int sCooldown = int.Parse(element.Attribute("cooldown").Value);
            int sRewardExp = int.Parse(element.Attribute("reward_exp").Value);
            int sRewardCoins = int.Parse(element.Attribute("reward_coins").Value);
            Vector3 sPosition = Parse(element.Attribute("position").Value);

            List<EventStage> sStages = new List<EventStage>();

            if(element.Element("stages") != null )
            {
                sStages = element.Element("stages").Elements("stage").Select(e =>
                    {
                        return EventStage.Parse(e);
                    }).ToList();
            }

            WorldEvent result = new WorldEvent()
            {
                Id = sId,
                Name = sName,
                Description = sDescription,
                Cooldown = sCooldown,
                RewardExp = sRewardExp,
                RewardCoins = sRewardCoins,
                Position = sPosition,
                Stages = sStages
            };
            return result;

        }

        private static Vector3 Parse(string s)
        {
            string[] comps = s.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            float x = float.Parse(comps[0], System.Globalization.CultureInfo.InvariantCulture);
            float y = float.Parse(comps[1], System.Globalization.CultureInfo.InvariantCulture);
            float z = float.Parse(comps[2], System.Globalization.CultureInfo.InvariantCulture);
            return new Vector3(x, y, z);
        }
    }
}

