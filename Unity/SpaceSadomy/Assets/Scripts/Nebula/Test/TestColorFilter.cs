namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Common;
    using Nebula.Client.Auction;

    public class TestColorFilter : MonoBehaviour {

        public ObjectColor color;

        public void OnValueChanged() {
            ColorFilter filter = new ColorFilter { color = color };
            if (GetComponent<Toggle>().isOn ) {
                GameData.instance.auction.SetFilter(filter);
            } else {
                GameData.instance.auction.RemoveFilter(filter);
            }
        }
    }
}
