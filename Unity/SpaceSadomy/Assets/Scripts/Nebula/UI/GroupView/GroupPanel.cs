namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class GroupPanel : MonoBehaviour {

        public Toggle CurrentGroupToggle;
        public Toggle SearchGroupToggle;
        public GameObject CurrentGroupView;
        public GameObject SearchGroupView;
        public Button CloseButton;

        public void OnCurrentGroupToggle() {
            if (this.CurrentGroupToggle.isOn) {
                this.CurrentGroupView.SetActive(true);
                this.SearchGroupView.SetActive(false);
            }
        }

        public void OnSearchGroupToggle() {
            if(this.SearchGroupToggle.isOn) {
                this.SearchGroupView.SetActive(true);
                this.CurrentGroupView.SetActive(false);
            }
        }

        public void OnCloseButton() {
            MainCanvas.Get.Destroy(CanvasPanelType.GroupView);
        }
    }
}
