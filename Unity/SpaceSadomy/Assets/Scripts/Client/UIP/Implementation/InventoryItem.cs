using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIC
{
    public class InventoryItem : IInventoryItem
    {
        public InventoryItem(string id, string name, string type, int count, int price)
        {
            this.id = id;
            this.Name = name;
            this.Type = type;
            this.Count = count;
            this.Price = price;
        }

        public string id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }
    }
}
