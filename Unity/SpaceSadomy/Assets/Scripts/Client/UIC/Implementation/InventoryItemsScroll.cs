using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Client.UIP;

namespace Client.UIC.Implementation
{
	public class InventoryItemsScroll : MonoBehaviour {

		// scroll list inventory items

		public System.Action<InventoryPanelItem> updateinfo;
		
		public RectTransform scrollContent;
		private ToggleGroup toggleGroup;
		
		private List<InventoryPanelItem> items = new List<InventoryPanelItem>();
		
		public void AddItem(IInventoryItem item)
		{
			if (toggleGroup == null)
			{
				toggleGroup = scrollContent.GetComponent<ToggleGroup>();
			}
			InventoryPanelItem invItem = InventoryPanelItem.Create(item);
			RectTransform rctTransform = invItem.transform as RectTransform;
			rctTransform.SetParent(scrollContent);
			rctTransform.localScale = Vector3.one;
			Toggle toggle = invItem.GetComponent<Toggle>();
			toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((b) => { this.updateinfo(invItem); });
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

        public void RemoveAllItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
                items[i] = null;
            } 
            items.Clear();
            UpdatePositions();
        }

	}
}
