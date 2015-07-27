namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class TestFilterGroup : MonoBehaviour {

        public Toggle parentToggle;
        public Toggle[] toggles;

        void OnEnable() {
            OnParentToggleValueChanged();
        }

        public void OnParentToggleValueChanged() {
            if (parentToggle.isOn) {
                foreach (var t in toggles) {
                    t.interactable = true;
                }
            } else {
                foreach (var t in toggles) {
                    t.interactable = false;
                }
            }
        }
    }
}
