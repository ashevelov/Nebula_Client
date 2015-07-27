namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Common;
    using UnityEngine.UI;
    using Nebula.Client.Auction;

    public class TestObjectTypeFilter : MonoBehaviour {
        public AuctionObjectType objectType;

        public void OnValueChanged() {
            AuctionFilter filter = new ObjectTypeFilter { objectType = objectType };

            if (GetComponent<Toggle>().isOn) {            
                GameData.instance.auction.SetFilter(filter);
            } else {
                GameData.instance.auction.RemoveFilter(filter);
            }
        }
    }
}
