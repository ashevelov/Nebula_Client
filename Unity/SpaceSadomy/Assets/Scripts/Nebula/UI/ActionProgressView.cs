namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class ActionProgressView : BaseView {

        public Slider Slider;
        public Text ActionText;

        private DataObject data;
        private float timer;
        private bool started;


        public class DataObject {
            public System.Action CompleteAction;
            public string ActionText;
            public float Duration;
        }



        public override void Setup(object objData) {
            base.Setup(objData);

            if(objData == null ) {
                MainCanvas.Get.Destroy(CanvasPanelType.ActionProgressView);
                return;
            }

            if(!(objData is DataObject)) {
                MainCanvas.Get.Destroy(CanvasPanelType.ActionProgressView);
                return;
            }

            data = objData as DataObject;

            this.Slider.value = 0f;
            if(string.IsNullOrEmpty(data.ActionText)) {
                this.ActionText.text = string.Empty;
            } else {
                this.ActionText.text = data.ActionText;
            }

            this.timer = 0f;
            this.started = true;
        }

        void Update () {
            if(!started) {
                return;
            }

            timer += Time.deltaTime;
            this.Slider.value = Mathf.Clamp01(timer / data.Duration);

            if(timer >= data.Duration) {
                if(data.CompleteAction != null ) {
                    data.CompleteAction();
                }
                this.started = false;
                MainCanvas.Get.Destroy(CanvasPanelType.ActionProgressView);
            }
        }
    }
}
