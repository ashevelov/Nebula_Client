namespace Nebula.UI {
    using Common;
    using Nebula.Client.Inventory;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class InventoryView : BaseView {

        public GameObject ItemViewPrefab;
        public GameObject ContentObject;
        public UnityEngine.UI.Text Title;
        public Button RemoveButton;
        public InventoryType InventoryType = InventoryType.ship;
        public Button MoveButton;



        private List<ClientInventoryItem> currentItems = new List<ClientInventoryItem>();
        private ClientInventoryItem selectedItem;

        void Start() {
            this.SelectItem(null);
            this.UpdateInventory();
            StartCoroutine(CorUpdateInventory());

            if(this.InventoryType == InventoryType.ship) {
                if(G.Game.CurrentStrategy != GameState.NebulaGameWorldEntered) {
                    if(this.MoveButton != null ) {
                        Destroy(this.MoveButton.gameObject);
                    }
                } else {
                    if(this.MoveButton != null ) {
                        this.MoveButton.onClick.RemoveAllListeners();
                        this.MoveButton.onClick.AddListener(() => {
                            if(this.selectedItem == null ) {
                                return;
                            }
                            NRPC.MoveItemFromInventoryToStation(this.selectedItem.Object.Type, this.selectedItem.Object.Id);
                            this.SelectItem(null);
                        });
                    }
                }
            }
        }

        private IEnumerator CorUpdateInventory() {
            while(true) {
                yield return new WaitForSeconds(Settings.INVENTORY_UPDATE_INTERVAL);
                this.UpdateInventory();
            }
        }

        void OnEnable() {
            if (this.InventoryType == InventoryType.station) {
                Events.StationUpdated += Events_StationUpdated;
            } else if(this.InventoryType == InventoryType.ship) {
                Events.PlayerInventoryUpdated += Events_PlayerInventoryUpdated;
            }
        }



        void OnDisable() {
            if (this.InventoryType == InventoryType.station) {
                Events.StationUpdated -= Events_StationUpdated;
            } else if (this.InventoryType == InventoryType.ship) {
                Events.PlayerInventoryUpdated -= Events_PlayerInventoryUpdated;
            }
            StopAllCoroutines();
        }

        private void Events_StationUpdated() {
            UpdateInventory();
        }

        private void Events_PlayerInventoryUpdated() {
            Events_StationUpdated();
        }

        void UpdateInventory() {

            if (this.InventoryType == InventoryType.ship) {
                if (G.Inventory() == null) { return; }
            }

            List<ClientInventoryItem> newItems = null;

            if (this.InventoryType == InventoryType.ship) {
                newItems = G.Inventory().OrderedItems();
            } else {
                newItems = G.Game.Station.StationInventory.OrderedItems();
            }

            
            List<ItemChangeEntry> changes = new List<ItemChangeEntry>();

            //first search new and modified items in new item list
            foreach (var item in newItems) {
                var foundedItem = this.currentItems.Find(it => it.Object.Id == item.Object.Id);
                if (foundedItem == null) {
                    changes.Add(new ItemChangeEntry { TargetState = ItemState.ADDED, Item = item });
                } else if( foundedItem.Count != item.Count ) {
                    changes.Add(new ItemChangeEntry { TargetState = ItemState.MODIFIED, Item = item });
                }
            }

            //then search items not presented in new list ( its removed )
            foreach (var cItem in currentItems) {
                var foundedItem = newItems.Find(it => it.Object.Id == cItem.Object.Id);
                if (foundedItem == null) {
                    changes.Add(new ItemChangeEntry { TargetState = ItemState.REMOVED, Item = cItem });
                }
            }

            InventoryItemView[] currentItemViews = this.ContentObject.GetComponentsInChildren<InventoryItemView>();

            //the apply changes 
            foreach (var change in changes) {
                print("Handle change: {0}:{1}".f(change.TargetState, change.Item.Object.Id));

                switch(change.TargetState) {
                    case ItemState.ADDED:
                        this.AddItem(change.Item);
                        break;
                    case ItemState.MODIFIED:
                        this.ModifyItem(currentItemViews, change.Item);
                        break;
                    case ItemState.REMOVED:
                        this.RemoveItem(currentItemViews, change.Item);
                        break;
                }  
            }
            this.Title.text = this.TitleString();

            //make new items as current items
            this.currentItems = newItems;
        }

        private ClientInventory TargetInventory() {
            if(this.InventoryType == InventoryType.ship) {
                return G.Game.Inventory;
            } else {
                return G.Game.Station.StationInventory;
            }
        }

        private string TitleString() {
            bool isFull = TargetInventory().SlotsUsed >= TargetInventory().MaxSlots;
            string usedString = TargetInventory().SlotsUsed.ToString();
            string maxString = TargetInventory().MaxSlots.ToString();

            string countString = isFull ? string.Format("{0}/{1}", usedString, maxString).Color("red") :
                string.Format("{0}/{1}", usedString, maxString).Color("green");
            return string.Format("Inventory: {0}", countString);
        }

        private void AddItem(ClientInventoryItem item) {
            GameObject itemView = Instantiate(this.ItemViewPrefab) as GameObject;
            itemView.transform.SetParent(this.ContentObject.transform, false);
            itemView.GetComponent<InventoryItemView>().SetDataObject(item, (isSelected) => {
                if(isSelected ) {
                    this.SelectItem(itemView.GetComponent<InventoryItemView>().DataObject());
                    Debug.LogFormat("Select item {0} in inventory", this.selectedItem.Object.Id);
                }
            });
        }

        private void ModifyItem(InventoryItemView[] sourceViews, ClientInventoryItem item) {
            foreach(var view in sourceViews) {
                if(view.DataObjectId() == item.Object.Id) {
                    view.SetDataObject(item);
                    break;
                }
            }
        }

        private void RemoveItem(InventoryItemView[] sourceViews, ClientInventoryItem item) {
            for(int i = 0; i < sourceViews.Length; i++) {
                if(sourceViews[i] == null) {
                    continue;
                }
                if(sourceViews[i].DataObjectId() == item.Object.Id) {
                    Destroy(sourceViews[i].gameObject);
                    sourceViews[i] = null;
                    break;
                }
            }
        }

        public void OnCloseButtonClick() {
            MainCanvas.Get.Destroy(CanvasPanelType.InventoryView);
        }

        public void OnShowInfoButtonClick() {

            if(this.selectedItem == null ) {
                Debug.LogWarning("No selected data");
                return;
            }

            ItemInfoView.ItemContentData contentData = new ItemInfoView.ItemContentData {
                 IsOnPlayer = false,
                 Data = this.selectedItem.Object
            };

            MainCanvas.Get.Show(CanvasPanelType.ItemInfoView, contentData);
        }

        public enum ItemState { ADDED, REMOVED, MODIFIED }
        public class ItemChangeEntry {
            public ItemState TargetState;
            public ClientInventoryItem Item;
        }

        public static void UpdateView() {
            if(MainCanvas.Get == null ) {
                return;
            }

            if(MainCanvas.Get.Exists(CanvasPanelType.InventoryView)) {
                MainCanvas.Get.GetView(CanvasPanelType.InventoryView).GetComponentInChildren<InventoryView>().UpdateInventory();
            }
        }

        public void SelectItem(ClientInventoryItem item) {
            this.selectedItem = item;
            if(item == null ) {
                this.RemoveButton.interactable = false;
            } else {
                this.RemoveButton.interactable = true;
            }
        }

        public void OnRemoveButtonClicked() {
            if (this.selectedItem != null) {
                NRPC.DestroyInventoryItem(this.InventoryType, this.selectedItem.Object.Type, this.selectedItem.Object.Id);
            }
            this.SelectItem(null);
        }

        //Called when move item to ship button clicked
        public void OnMoveToShipButtonClicked() {
            if(this.selectedItem == null) {
                Debug.LogWarning("No selected item");
                return;
            }


            NRPC.MoveItemFromStationToInventory(this.selectedItem.Object.Type, this.selectedItem.Object.Id);
            this.SelectItem(null);
        }
    }
}