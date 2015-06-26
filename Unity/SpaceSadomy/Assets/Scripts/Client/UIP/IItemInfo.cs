using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Client.UIP
{
    public interface IItemInfo
    {

        string Name { get; set; }
        Sprite Icon { get; set; }
        string Description { get; set; }
        Dictionary<string, string> Parametrs { get; set; }
        Sprite SkillIcon { get; set; }
        string SkillDescription { get; set; }
    }
}
