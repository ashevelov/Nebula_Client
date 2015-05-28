namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;

    public class StationView : BaseView {

        public void OnCloseButtonClicked () {
            MainCanvas.Get.Destroy(CanvasPanelType.StationView);
        }
    }
}
