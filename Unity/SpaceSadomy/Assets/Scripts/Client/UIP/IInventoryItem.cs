using System;
using System.Collections.Generic;
using UnityEngine;
namespace UIC
{
    public interface IInventoryItem
    {
        string id { get; set; }
        Sprite Icon { set; }
        string Color { set; }
        string Name { set; }
        string Type { set; }
        int Count { set; }
        int Price { set; }
        IItemInfo itemInfo { get; set; }
        Dictionary<string, Action> Actions { get; set; }
    }
}
