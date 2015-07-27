namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Client.Auction;
    using UnityEngine.UI;

    public class TestWorkshopFilter : MonoBehaviour {

        public Workshop workshop;

        public void OnValueChanged() {
            var filter = new WorkshopFilter { workshop = workshop };
            if (GetComponent<Toggle>().isOn) {
                GameData.instance.auction.SetFilter(filter);
            } else {
                GameData.instance.auction.RemoveFilter(filter);
            }
        }
    }
}
