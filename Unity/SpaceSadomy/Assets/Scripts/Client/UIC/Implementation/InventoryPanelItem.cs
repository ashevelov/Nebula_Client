using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UIC
{
    public class InventoryPanelItem : MonoBehaviour, IInventoryItem
    {

        public static InventoryPanelItem Create(IInventoryItem item)
        {
            InventoryPanelItem invItem = Instantiate(Resources.Load("Prefabs/UIC/InventoryPanelItem") as GameObject).GetComponent<InventoryPanelItem>();
            InventoryItem itm = item as InventoryItem;
            invItem.Name = itm.Name;
            invItem.Type = itm.Type;
            invItem.Count = itm.Count;
            invItem.Price = itm.Price;
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
    }
}
