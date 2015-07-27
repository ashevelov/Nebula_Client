using UnityEngine;
using System.Collections;
using Client.UIC;
using Nebula.UI;
using Client.UIC.Implementation;

namespace Client.UIP.Implementation
{
    public class EditPricePanelProcess : MonoBehaviour 
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


        static EditPricePanelProcess editPricePanelProcess;
        public static void Init(string id, Sprite icon, int startPrice, string desc ="this is item description")
        {
            if (editPricePanelProcess == null)
            {
                editPricePanelProcess = (Instantiate(Resources.Load("Prefabs/UIC/EditPricePanel")) as GameObject).GetComponent<EditPricePanelProcess>();
                editPricePanelProcess.transform.SetParent(MainCanvas.Get.transform);
                editPricePanelProcess.transform.localScale = Vector3.one;
                editPricePanelProcess.transform.localPosition = Vector3.one;
                editPricePanelProcess.SetIcon(icon);
                editPricePanelProcess.SetDescription(desc);
                editPricePanelProcess.SetStartPrice(startPrice);
                editPricePanelProcess.itemID = id;
            }
        }

        public void Close()
        {
            editPricePanelProcess = null;
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
        public void OnEditPriceClick()
        {
            Debug.Log("new price " + SellPanel.GetPrice() + " credits");
        }
	
    }
}
