using UnityEngine;
using System.Collections;
using Client.UIC;
using Nebula.UI;
using Client.UIC.Implementation;
using Nebula.Mmo.Games;

namespace Client.UIP.Implementation
{
    public class SellPanelProcess : MonoBehaviour 
    {
        ISellPanel _uicPanel;
        ISellPanel SellPanel
        {
            get
            {
                if (_uicPanel == null)
                {
                    _uicPanel = GetComponent<SellPanel>();
                }
                return _uicPanel;
            }
        }


        static SellPanelProcess sellPanelInstans;
        public static void Init(string id, Sprite icon, int startPrice, string desc ="this is item description")
        {
            if (sellPanelInstans == null)
            {
                sellPanelInstans = (Instantiate(Resources.Load("Prefabs/UIC/SellPanel")) as GameObject).GetComponent<SellPanelProcess>();
                sellPanelInstans.transform.SetParent(MainCanvas.Get.transform);
                sellPanelInstans.transform.localScale = Vector3.one;
                sellPanelInstans.transform.localPosition = Vector3.one;
                sellPanelInstans.SetIcon(icon);
                sellPanelInstans.SetDescription(desc);
                sellPanelInstans.SetStartPrice(startPrice);
                sellPanelInstans.itemID = id;
            }
        }

        public void Close()
        {
            sellPanelInstans = null;
            Destroy(gameObject);
        }

        private void SetIcon(Sprite icon)
        {
            SellPanel.SetIcon(icon);
        }

        private void SetDescription(string text)
        {
            SellPanel.SetDescrption(text);
        }

        private void SetStartPrice(int price)
        {
            SellPanel.SetStartPrice(price);
        }

        private string itemID;
        public void OnSellClick()
        {
            Debug.Log("OnSellClick");
            Debug.Log("sell " + itemID + " for " + SellPanel.GetPrice() + " credits");
            //в аукцион
            SelectCharacterGame.Instance().PutToAuction(itemID, Common.InventoryType.ship, 1, SellPanel.GetPrice());
           //в магазин по умолчанию
           SelectCharacterGame.Instance().PutStore();
        }
	
    }
}
