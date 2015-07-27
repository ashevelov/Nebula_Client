using UnityEngine;
using System.Collections;
using Client.UIP;
using System.Collections.Generic;
using Nebula.Client.Auction;
using Client.UIC;
using Client.UIP.Implementation;
using UnityEngine.UI;

namespace Client.UIC.Implementation
{
    public class StorePanel : MonoBehaviour, IStorePanel
    {

        public InputField minPrice;
        public int GetMinPrice()
        {
            int p = StringToInt(minPrice.text);
            minPrice.text=p.ToString();
            return p;
        }

        public InputField maxPrice;
        public int GetMaxPrice()
        {
            int p = StringToInt(maxPrice.text);
            maxPrice.text = p.ToString();
            return p;
        }

        public InputField searchName;
        public string GetSearchName()
        {
            return searchName.text;
        }

        private int StringToInt(string text)
        {
            string p = "0";
            foreach (char c in text)
            {
                if (char.IsNumber(c))
                {
                    p += c;
                }
            }
            int prc = int.Parse(p);
            return prc;
        }

        public event System.Action<AuctionFilter> AddFilterHendler;
        public event System.Action<AuctionFilter> RemoveFilterHendler;
        private void AddFilter(AuctionFilter filter)
        {
            if (AddFilterHendler != null)
                AddFilterHendler(filter);
        }
        private void RemoveFilter(AuctionFilter filter)
        {
            if (RemoveFilterHendler != null)
                RemoveFilterHendler(filter);
        }

        void Start()
        {
            AddFilterHendler += (af) => { Debug.Log("AddFilter " + af.key); };
            RemoveFilterHendler += (af) => { Debug.Log("RemoveFilterHendler " + af.key); };
            foreach (var filterToggle in GetComponentsInChildren<FilterToggle>())
            {
                filterToggle.AddFilterHendler += AddFilter;
                filterToggle.RemoveFilterHendler += RemoveFilter;
            }

            ItemsScroll.updateinfo = UpdateInfo;
        }
        void OnDisable()
        {
            foreach (var filterToggle in GetComponentsInChildren<FilterToggle>())
            {
                filterToggle.AddFilterHendler -= AddFilter;
                filterToggle.RemoveFilterHendler -= RemoveFilter;
            }
        }


        InventoryItemsScroll _itemsScroll;
        InventoryItemsScroll ItemsScroll
        {
            get
            {
                if (_itemsScroll == null)
                    _itemsScroll = GetComponentInChildren<InventoryItemsScroll>();
                return _itemsScroll;
            }
        }

        public event System.Action UpdateItemsHendler;
        public void UpdateItems(List<IInventoryItem> items)
        {
            ItemsScroll.RemoveAllItems();
            foreach (var item in items)
            {
                ItemsScroll.AddItem(item);
            }
        }
        public void SetPageCount(int i)
        {

        }
        public int GetCurrentPage()
        {
            return 1;
        }

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
            SetItemActions(inventoryItem.Actions, inventoryItem.id);
        }


        // action buttons

        public Button[] actionButtons;
        public void SetItemActions(Dictionary<string, System.Action<string>> actions, string ItemID)
        {

            int index = 0;
            if(actions != null)
            {
                if (actions.ContainsKey("del"))
                {
                    UpdateButton(actions, ItemID, index, "icons/delete11", "del");
                    index++;
                }
                if (actions.ContainsKey("move"))
                {
                    UpdateButton(actions, ItemID, index, "icons/move1", "move");
                    index++;
                }
                if (actions.ContainsKey("equip"))
                {
                    UpdateButton(actions, ItemID, index, "icons/equip1", "equip");
                    index++;
                }
                if (actions.ContainsKey("craft"))
                {
                    UpdateButton(actions, ItemID, index, "icons/craft1", "craft");
                    index++;
                }
                if (actions.ContainsKey("buy"))
                {
                    Debug.Log("buy");
                    UpdateButton(actions, ItemID, index, "icons/buy1", "buy");
                    index++;
                }
                if (actions.ContainsKey("edit"))
                {
                    Debug.Log("edit");
                    UpdateButton(actions, ItemID, index, "icons/edit_price", "edit");
                    index++;
                }
                
            }
            while (index < actionButtons.Length)
            {
                actionButtons[index].interactable = false;
                actionButtons[index].GetComponentInChildren<Text>().text = "";
                UpdateButtonIcon(index, "transparent");
                index++;
            }
        }

        private void UpdateButton(Dictionary<string, System.Action<string>> actions, string ItemID, int index, string iconPath, string actionKey)
        {
            actionButtons[index].interactable = true;
            actionButtons[index].onClick.RemoveAllListeners();
            actionButtons[index].onClick.AddListener(() => actions[actionKey](ItemID));
            UpdateButtonIcon(index, iconPath);
            
        }

        private void UpdateButtonIcon(int index, string iconPath)
        {
            Image[] childImages = actionButtons[index].GetComponentsInChildren<Image>();
            foreach (var eventIcon in childImages)
            {
                if (eventIcon.gameObject != actionButtons[index].gameObject)
                {
                    eventIcon.sprite = Nebula.Resources.SpriteCache.GetObject(iconPath);
                }
            }
        }
    }
}
