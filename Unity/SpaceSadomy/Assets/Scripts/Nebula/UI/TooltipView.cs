namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Common;

    public class TooltipView : BaseView {

        public ScrollRect Scroll;
        public Text TitleText;
        public Text TipText;

        public override void Setup(object objData) {
            base.Setup(objData);

            if(objData == null ) {
                Debug.LogError("Init data for tooltip view must be not null");
                return;
            }

            if(objData.GetType() != typeof(TooltipData)) {
                Debug.LogError("Init data for tooltip must be of type: {0}".f(typeof(TooltipData).Name));
                return;
            }

            var data = objData as TooltipData;

            if(string.IsNullOrEmpty(data.TitleId)) {
                this.TitleText.text = string.Empty;
            } else {
                this.TitleText.text = StringCache.Get(data.TitleId).FormatBraces();
            }

            if (string.IsNullOrEmpty(data.ContentId)) {
                this.TipText.text = string.Empty;
            } else {
                this.TipText.text = StringCache.Get(data.ContentId).FormatBraces();
            }

            this.Scroll.verticalNormalizedPosition = 1.0f;
        }

        public class TooltipData {
            public string TitleId;
            public string ContentId;
        }

        public void OnCloseButtonClick() {
            MainCanvas.Get.Destroy(CanvasPanelType.TooltipView);
        }
    }
}
