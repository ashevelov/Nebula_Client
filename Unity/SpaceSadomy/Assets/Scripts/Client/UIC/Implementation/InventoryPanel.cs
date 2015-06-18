using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UIC
{
    public class InventoryPanel : MonoBehaviour, IInventoryPanel {

        // scroll list inventory items

        public RectTransform scrollContent;

        private List<InventoryPanelItem> items = new List<InventoryPanelItem>();

        public void AddItem(IInventoryItem item)
        {
            InventoryPanelItem invItem = InventoryPanelItem.Create(item);
            RectTransform rctTransform = invItem.transform as RectTransform;
            rctTransform.parent = scrollContent;
            rctTransform.localScale = Vector3.one;
            items.Add(invItem);
            UpdatePositions();
        }
        public void ModifiedItem(string id, int count)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == id)
                {
                    items[i].Count = count;
                }
            }
        }

        public void RemoveItem(string id)
        {
            int index = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == id)
                {
                    index = i;
                }
            }
            InventoryPanelItem item = items[index];
            items.RemoveAt(index);
            Destroy(item.gameObject);
            item = null;
            UpdatePositions();
        }

        private void UpdatePositions()
        {
            if(items.Count != 0)
            {
                Vector3 pos = Vector3.zero;
                float ySize = (items[0].transform as RectTransform).sizeDelta.y;
                Vector3 scrollViewSize = scrollContent.sizeDelta;
                scrollViewSize.y = Mathf.Max((scrollContent.parent as RectTransform).sizeDelta.y, items.Count * ySize);
                scrollContent.sizeDelta = scrollViewSize;

                for (int i = 0; i < items.Count; i++ )
                {
                    RectTransform rctTransform = items[i].transform as RectTransform;
                    pos.y = -i * ySize;
                    rctTransform.anchoredPosition = pos;
                }

            }
            
        }
        
        // item info

        public Image itemIcon;

        public void UpdateInfo(string selectItemId)
        {
            
        }


        // action buttons
        public void SetEquipAction(System.Action<string> action)
        {

        }


    }
}
