using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UIP;

namespace UIC
{
    public class InventoryPanel : MonoBehaviour, IInventoryPanel {

        // scroll list inventory items

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
            rctTransform.parent = scrollContent;
            rctTransform.localScale = Vector3.one;
            Toggle toggle = invItem.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((b)=>{this. UpdateInfo(invItem);});
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
                Debug.Log("remove item = " + id);
                Debug.Log("remove items[i].id = " + items[i].id);
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
        public Text descroption;
        public Image skillIcon;
        public Text skillDescription;
        public ParamPanel[] parameters;


        public void UpdateInfo(InventoryPanelItem inventoryItem)
        {
            //InventoryItem inventoryItem = new InventoryItem("", null, "", "", "", 0, 0, null);
            itemIcon.sprite = inventoryItem.itemInfo.Icon;
            descroption.text = inventoryItem.itemInfo.Description;
            
            Vector3 pos = Vector3.zero;
            int index = 0;
            foreach (KeyValuePair<string, string> param in inventoryItem.itemInfo.Parametrs)
            {
                parameters[index].gameObject.SetActive(true);
                parameters[index].Init(param.Key, param.Value);
                index++;
            }
            while (index < parameters.Length)
            {
                parameters[index].gameObject.SetActive(false);
                index++;
            }

            if (inventoryItem.itemInfo.SkillIcon != null)
            {
                skillIcon.gameObject.SetActive(true);
                skillDescription.gameObject.SetActive(true);
                skillIcon.sprite = inventoryItem.itemInfo.SkillIcon;
                skillDescription.text = inventoryItem.itemInfo.SkillDescription;
            }
            else
            {
                skillIcon.gameObject.SetActive(false);
                skillDescription.gameObject.SetActive(false);
            }
            SetItemActions(inventoryItem.Actions);
        }


        // action buttons

        public Button[] actionButtons;
        public void SetItemActions ( Dictionary<string, System.Action> actions)
        {

            int index = 0;
            if(actions != null)
            {
                foreach(KeyValuePair<string, System.Action> action in actions)
                {
                    Debug.Log("action.Key --------------" +action.Key);
                    actionButtons[index].interactable = true;
                    actionButtons[index].onClick.AddListener(() => action.Value());
                    actionButtons[index].GetComponentInChildren<Text>().text = action.Key;
                    index++;
                }
            }
            while (index < actionButtons.Length)
            {
                actionButtons[index].interactable = false;
                actionButtons[index].GetComponentInChildren<Text>().text = "";
                index++;
            }
        }
    }
}
