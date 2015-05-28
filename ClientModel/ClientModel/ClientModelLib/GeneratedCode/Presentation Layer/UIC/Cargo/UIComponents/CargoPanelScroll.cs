using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIC.CargoComponents.UIComponents
{
    public class CargoPanelScroll : MonoBehaviour {

        public CargoPanelItem tempCargoItem;
        public RectTransform content;
        public List<CargoPanelItem> cargoPanelItems;

        public void Init(List<ICargoItem> items)
        {
 
        }

        public void SortItems()
        {
 
        }

        private void AddItem(ICargoItem cargoitem)
        {
            CargoPanelItem panelItem = Instantiate(tempCargoItem) as CargoPanelItem;
            panelItem.transform.SetParent(this.transform);
            panelItem.Init(cargoitem);
            
        }

        private void UpdateItemsPosition()
        {
            for (int i = 0; i < cargoPanelItems.Count; i++)
            {
                RectTransform itemRect = cargoPanelItems[i].transform as RectTransform;
                itemRect.localPosition = Vector3.up * -itemRect.sizeDelta.y;
            }
        }
    }
}

