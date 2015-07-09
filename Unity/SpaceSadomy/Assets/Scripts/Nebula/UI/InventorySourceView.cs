namespace Nebula.UI {
    using Common;
    using Nebula.Client.Inventory;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class InventorySourceView : BaseView {

        public GameObject ItemViewPrefab;
        public GameObject ContentObject;
        public Text Title;



        private List<ClientInventoryItem> currentItems = new List<ClientInventoryItem>();
        private ClientInventoryItem selectedItem;
        private IInventoryItemsSource source;



        public override void Setup(object objData) {
            base.Setup(objData);
            if (objData == null) {
                Debug.LogError("InventorySourceView obj data must be not null");
                return;
            }
            if (!(objData is IInventoryItemsSource)) {
                Debug.LogErrorFormat("InventorySourceView must be of type {0}", typeof(IInventoryItemsSource).Name);
                return;
            }

           source = objData as IInventoryItemsSource;


            this.UpdateInventory();
            StopAllCoroutines();
            StartCoroutine(CorUpdateInventory());
        }

        private IEnumerator CorUpdateInventory() {
            while (true) {
                yield return new WaitForSeconds(Settings.INVENTORY_UPDATE_INTERVAL);
                this.UpdateInventory();
            }
        }

        public void UpdateInventory() {

            if(source == null ) {
                return;
            }

            var newItems = source.Items;
            List<InventoryView.ItemChangeEntry> changes = new List<InventoryView.ItemChangeEntry>();

            //first search new and modified items in new item list
            foreach (var item in newItems) {
                var foundedItem = this.currentItems.Find(it => it.Object.Id == item.Object.Id);
                if (foundedItem == null) {
                    changes.Add(new InventoryView.ItemChangeEntry { TargetState = InventoryView.ItemState.ADDED, Item = item });
                } else if (foundedItem.Count != item.Count) {
                    changes.Add(new InventoryView.ItemChangeEntry { TargetState = InventoryView.ItemState.MODIFIED, Item = item });
                }
            }

            //then search items not presented in new list ( its removed )
            foreach (var cItem in currentItems) {
                var foundedItem = newItems.Find(it => it.Object.Id == cItem.Object.Id);
                if (foundedItem == null) {
                    changes.Add(new InventoryView.ItemChangeEntry { TargetState = InventoryView.ItemState.REMOVED, Item = cItem });
                }
            }

            InventoryItemView[] currentItemViews = this.ContentObject.GetComponentsInChildren<InventoryItemView>();

            //the apply changes 
            foreach (var change in changes) {
                print("Handle change: {0}:{1}".f(change.TargetState, change.Item.Object.Id));

                switch (change.TargetState) {
                    case InventoryView.ItemState.ADDED:
                        this.AddItem(change.Item);
                        break;
                    case InventoryView.ItemState.MODIFIED:
                        this.ModifyItem(currentItemViews, change.Item);
                        break;
                    case InventoryView.ItemState.REMOVED:
                        this.RemoveItem(currentItemViews, change.Item);
                        break;
                }
            }
            this.Title.text = this.TitleString();

            //make new items as current items
            this.currentItems = newItems;
            if (this.currentItems.Count == 0)
            {
                OnCloseButtonClick();
            }
        }

        private string TitleString() {
            return "Container:";
        }

        private void AddItem(ClientInventoryItem item) {
            GameObject itemView = Instantiate(this.ItemViewPrefab) as GameObject;
            itemView.transform.SetParent(this.ContentObject.transform, false);
            itemView.GetComponent<InventoryItemView>().SetDataObject(item, (isOn) => {
                if (isOn) {
                    this.selectedItem = item;
                    Debug.LogFormat("Select item {0}", this.selectedItem.Object.Id);
                }
            });
        }

        private void ModifyItem(InventoryItemView[] sourceViews, ClientInventoryItem item) {
            foreach (var view in sourceViews) {
                if (view.DataObjectId() == item.Object.Id) {
                    view.SetDataObject(item);
                    break;
                }
            }
        }

        private void RemoveItem(InventoryItemView[] sourceViews, ClientInventoryItem item) {
            for (int i = 0; i < sourceViews.Length; i++) {
                if (sourceViews[i] == null) {
                    continue;
                }
                if (sourceViews[i].DataObjectId() == item.Object.Id) {
                    Destroy(sourceViews[i].gameObject);
                    sourceViews[i] = null;
                    break;
                }
            }
        }

        public void OnCloseButtonClick() {
            this.source = null;
            this.StopAllCoroutines();
            MainCanvas.Get.Destroy(CanvasPanelType.InventorySourceView);
        }

        //Handler for 'Take' button
        //Make request take selected item if any
        public void OnTakeButtonClick() {
            string errMsg = null;
            if(!this.CheckSelectedItem(out errMsg)) {
                Debug.LogError("OnTakeButtonClick(): " + errMsg);
                return;
            }

            switch (source.SourceType) {
                case ItemType.Asteroid: {
                        NRPC.RequestMoveAsteroidItemToInventory(source.Id, this.selectedItem.Object.Id, this.selectedItem.Object.Type.toByte());
                    }
                    break;
                case ItemType.Chest:{
                        NRPC.AddToInventory(source.Id, source.SourceType.toByte(), this.selectedItem.Object.Id);
                    }
                    break;
                default: {
                        Debug.LogErrorFormat("OnTakeButtonClick(): not realized for source type: {0}", source.SourceType);
                        break;
                    }
            }
        }

        //Handler for show Info button
        //Show info for selected item if any
        public void OnShowInfoButtonClick() {
            string errMsg = null;
            if(!this.CheckSelectedItem(out errMsg)) {
                Debug.LogError("OnShowInfoButtonClick(): " + errMsg);
                return;
            }

            MainCanvas.Get.Show(CanvasPanelType.ItemInfoView, new ItemInfoView.ItemContentData { IsOnPlayer = false, Data = this.selectedItem.Object });
        }

        private bool CheckSelectedItem(out string errorMessage) {
            errorMessage = string.Empty;

            if (this.source == null) {
                errorMessage = "source don't allow null";
                return false;
            }
            if (this.selectedItem == null) {
                errorMessage = "selected item don't allow null";
                return false;
            }
            return true;
        }

    }
}
