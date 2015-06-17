using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UIC
{
    public class InventoryPanel : MonoBehaviour, IInventoryPanel {

        public RectTransform scrollContent;

        private List<InventoryPanelItem> items;

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

        public void SetEquipAction(System.Action<string> action)
        {
            
        }

        private void UpdatePositions()
        {
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < items.Count; i++ )
            {
                RectTransform rctTransform = items[i].transform as RectTransform;
                pos.y = i* rctTransform.sizeDelta.y;
                rctTransform.localPosition = pos;
            }
        }
    }
}
