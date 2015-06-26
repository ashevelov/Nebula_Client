using UnityEngine;

namespace Client.UIP.Implementation
{
    public class ItemInfo : IItemInfo
    {
        public string Name { get; set; }

        public Sprite Icon { get; set; }

        public string Description { get; set; }

        public System.Collections.Generic.Dictionary<string, string> Parametrs { get; set; }

        public Sprite SkillIcon { get; set; }

        public string SkillDescription { get; set; }
    }
}
