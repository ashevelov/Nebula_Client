using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UIP;

namespace UIC
{
    public class InventoryItem : IInventoryItem
    {
        public InventoryItem(string id, Sprite icon, string color, string name, string type, int count, int price, IItemInfo info)
        {
            this.id = id;
            this.Icon = icon;
            this.Color = color;
            this.Name = name;
            this.Type = type;
            this.Count = count;
            this.Price = price;
            this.itemInfo = info;
        }

        public string id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        public IItemInfo itemInfo { get; set; }

        public Sprite Icon { get; set; }

        public string Color { get; set; }
    }
}
