namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Nebula.Client.Auction;
    using Common;


    //test for server auction item
    public class TestAuctionItemView : MonoBehaviour {
        public Text vendorLogin;
        public Text itemName;
        public Text price;
        public Text itemCount;
        public Toggle selectionToggle;

        private AuctionItem mItem;

        public void Set(AuctionItem item) {
            mItem = item;
            vendorLogin.text = item.login;
            itemName.text = item.objectInfo.selectName();
            price.text = string.Format("Credits={0}", item.price);
            itemCount.text = string.Format("Count={0}", item.count);
        }

        public void OnSelectionToggleSelected() {
            if(selectionToggle.isOn) {
                Debug.LogFormat("selected item = {0}", mItem.objectInfo.selectName());
            }
        }

        public void OnBuyItemClicked() {
            Debug.LogFormat("On buy item = {0} clicked", mItem.storeItemId);
        } 

        public AuctionItem item {
            get {
                return mItem;
            }
        }
    }
}
