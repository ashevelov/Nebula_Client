namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Common;
    using UnityEngine.UI;
    using Nebula.Mmo.Games;
    using Nebula.Client.Inventory;

    public class TestItemView : MonoBehaviour {

        public Text itemName;

        private object mItem;
        private InventoryType mInventoryType;


        public void Set(object inItem, InventoryType inInventoryType ) {
            mItem = inItem;
            mInventoryType = inInventoryType;

            if(mItem is ClientInventoryItem) {
                itemName.text = (mItem as ClientInventoryItem).Object.Type.ToString().ToUpper();
            }
            //else if(mItem is IStationHoldableObject) {
            //    itemName.text = (mItem as IStationHoldableObject).Type.ToString().ToUpper();
            //}
        }

        public void OnPutButtonClick() {
            SelectCharacterGame.Instance().PutToAuction(itemID, mInventoryType, itemCount, TestItemList.Get.currentPrice);
        }

        public string itemID {
            get {
                return (mItem as ClientInventoryItem).Object.Id;
                    //(mItem as IStationHoldableObject).Id;
            }
        }


        private int itemCount {
            get {
                return (mItem is ClientInventoryItem) ?
                    (mItem as ClientInventoryItem).Count :
                    1;
            }
        }

        public InventoryType inventoryType {
            get {
                return mInventoryType;
            }
        }

        public object item {
            get {
                return mItem;
            }
        }
    }
}
