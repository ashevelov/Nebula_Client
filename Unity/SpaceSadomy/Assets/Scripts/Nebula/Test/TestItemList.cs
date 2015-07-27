namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Game.Space;
    using System.Collections.Generic;
    using Nebula.UI;
    using Nebula.Client.Inventory;
    using Common;

    public class TestItemList : Singleton<TestItemList> {

        public GameObject sourceItemView;
        public Transform content;

        private List<TestItemView> mViews = new List<TestItemView>();
        private List<ItemChange> mChanges = new List<ItemChange>();

        void OnEnable() {
            Nebula.Events.PlayerInventoryUpdated += Events_PlayerInventoryUpdated;
            Nebula.Events.StationUpdated += Events_StationUpdated;
        }



        void OnDisable() {
            Nebula.Events.PlayerInventoryUpdated -= Events_PlayerInventoryUpdated;
            Nebula.Events.StationUpdated -= Events_StationUpdated;
        }

        private void Events_StationUpdated() {
            UpdateViews();
        }

        private void Events_PlayerInventoryUpdated() {
            UpdateViews();
        }



        public int currentPrice {
            get {
                return 50;
            }
        }


        private void UpdateViews() {

            mChanges.Clear();
            foreach(var it in GameData.instance.inventory.OrderedItems()) {
                if(!HasView(it)) {
                    Debug.LogFormat("ADD ITEM = {0} FROM INVENTORY = {1}", itemID(it), InventoryType.ship);
                    mChanges.Add(new ItemChange { ChangeType = ChangeType.ADD, inventoryType = InventoryType.ship, TargetObject = it });
                }
            }
            foreach(var it in GameData.instance.station.StationInventory.OrderedItems()) {
                if(!HasView(it)) {
                    mChanges.Add(new ItemChange { ChangeType = ChangeType.ADD, inventoryType = InventoryType.station, TargetObject = it });
                }
            }
            //foreach(var it in GameData.instance.station.Hold.OrderedItems) {
            //    if(!HasView(it)) {
            //        mChanges.Add(new ItemChange { ChangeType = ChangeType.ADD, inventoryType = InventoryType.station_hold, TargetObject = it });
            //    }
            //}

            foreach(var v in mViews ) {
                if(!HasObject(v.item, v.inventoryType)) {
                    mChanges.Add(new ItemChange { ChangeType = ChangeType.REMOVE, inventoryType = v.inventoryType, TargetObject = v.item });
                }
            }

            foreach(var change in mChanges ) {
                switch(change.ChangeType) {
                    case ChangeType.ADD:
                        {
                            GameObject inst = Instantiate(sourceItemView) as GameObject;
                            inst.transform.SetParent(content, false);
                            inst.GetComponent<TestItemView>().Set(change.TargetObject, change.inventoryType);
                            mViews.Add(inst.GetComponent<TestItemView>());
                            break;
                        }
                    case ChangeType.REMOVE:
                        {
                            string itID = itemID(change.TargetObject);
                            TestItemView targetView = mViews.Find(v => v.itemID == itID);
                            if(targetView != null ) {
                                mViews.Remove(targetView);
                                Destroy(targetView.gameObject);
                            }
                            break;
                        }
                }
            }
        }

        private string itemID (object obj) {
            if (obj is ClientInventoryItem)
            {
                return (obj as ClientInventoryItem).Object.Id;
            }
            //else if (obj is IStationHoldableObject)
            //{
            //    return (obj as IStationHoldableObject).Id;
            //}
            return string.Empty;
        }

        private bool HasView(object obj) {
            string objID = itemID(obj);
            foreach(var v in mViews ) {
                if(v.itemID == objID) {
                    return true;
                }
            }
            return false;
        }

        private bool HasObject(object obj, InventoryType invType ) {
            switch(invType) {
                case InventoryType.ship: {
                        var ii = obj as ClientInventoryItem;
                        return GameData.instance.inventory.HasItem(ii.Object.Type, ii.Object.Id);
                    }
                case InventoryType.station: {
                        var ii = obj as ClientInventoryItem;
                        return GameData.instance.station.StationInventory.HasItem(ii.Object.Type, ii.Object.Id);
                    }
                //case InventoryType.station_hold: {
                //        var si = obj as IStationHoldableObject;
                //        return GameData.instance.station.Hold.HasItem(si.Type, si.Id);
                //    }
                default: return false;
            }
        }

        public class ItemChange : ObjectChange<object> {

            public InventoryType inventoryType {
                get; set;
            }

            public string itemID {
                get {
                    if(TargetObject is ClientInventoryItem) {
                        return (TargetObject as ClientInventoryItem).Object.Id;
                    }
                    //else if(TargetObject is IStationHoldableObject) {
                    //    return (TargetObject as IStationHoldableObject).Id;
                    //}
                    return string.Empty;
                }
            }
        }


    }
}
