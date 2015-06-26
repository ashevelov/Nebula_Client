using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Client.UIP;
using Client.UIP.Implementation;

namespace Client.UIC.Implementation
{
    public class InventoryPanelItem : MonoBehaviour, IInventoryItem
    {

        public static InventoryPanelItem Create(IInventoryItem item)
        {
            InventoryPanelItem invItem = Instantiate(Resources.Load("Prefabs/UIC/InventoryPanelItem") as GameObject).GetComponent<InventoryPanelItem>();
            InventoryItem itm = item as InventoryItem;
            invItem.id = item.id;
            invItem.Name = itm.Name;
            invItem.Type = itm.Type;
            invItem.Count = itm.Count;
            invItem.Price = itm.Price;
            invItem.Color = itm.Color;
            invItem.Icon = itm.Icon;
            invItem.itemInfo = item.itemInfo;
            invItem.Actions = item.Actions;
            return invItem;
        }

        public Text itemName;
        public string Name
        {
            set { itemName.text = value; }
        }


        public Text itemType;
        public string Type
        {
            set { itemType.text = value; }
        }

        public Text itemCount;
        public int Count
        {
            set { itemCount.text = value.ToString(); }
        }

        public Text itemPrice;
        public int Price
        {
            set
            {
                if (value == 0)
                    itemPrice.text = "-";
                else
                    itemPrice.text = value.ToString();
            }
        }

        public string id { get; set; }


        public IItemInfo itemInfo { get; set; }

        public Image itemIcon;
        public Sprite Icon
        {
            set { itemIcon.sprite = value; }
        }

        public string Color
        {
            set { itemName.color = ToColor(value); }
        }

        private UnityEngine.Color ToColor(string stColor)
        {
            switch(stColor)
            {
                case "white" : return UnityEngine.Color.white;
                case "blue" : return UnityEngine.Color.blue;
                case "yellow" : return UnityEngine.Color.yellow;
                case "green" : return UnityEngine.Color.green;
                case "orange" : return new UnityEngine.Color (255,111,0);
            }
            return UnityEngine.Color.white;
        }


        public Dictionary<string, System.Action> Actions { get; set; }
    }
}
