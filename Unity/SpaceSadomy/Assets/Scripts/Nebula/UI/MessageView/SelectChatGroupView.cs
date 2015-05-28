using UnityEngine;
using System.Collections;

namespace Nebula.UI {
    using Common;
    using UToggle = UnityEngine.UI.Toggle;

    public class SelectChatGroupView : MonoBehaviour {

        private ChatGroup selectedChatGroup = ChatGroup.all;
        private System.Action hideHandler;

        public UToggle AllToggle;
        public UToggle ZoneToggle;
        public UToggle GroupToggle;
        public UToggle AllianceToggle;
        public UToggle AllianceAndZoneToggle;
        public UToggle PlayerToggle;
        public UToggle MeToggle;


        public void OnAllToggleChanged() {
            if (this.AllToggle.isOn) {
                this.SelectGroup(ChatGroup.all);
                this.Hide();
            }
        }

        public void OnZoneToggleChanged() {
            if (this.ZoneToggle.isOn) {
                this.SelectGroup(ChatGroup.zone);
                this.Hide();
            }
        }

        public void OnGroupToggleChanged() {
            if (this.GroupToggle.isOn) {
                this.SelectGroup(ChatGroup.group);
                this.Hide();
            }
        }

        public void OnAllianceToggleChanged() {
            if (this.AllianceToggle.isOn) {
                this.SelectGroup(ChatGroup.alliance);
                this.Hide();
            }
        }

        public void OnAllianceAndZoneToggleChanged() {
            if (this.AllianceAndZoneToggle.isOn) {
                this.SelectGroup(ChatGroup.zone_and_alliance);
                this.Hide();
            }
        }

        public void OnPlayerToggleChanged() {
            if (this.PlayerToggle.isOn) {
                this.SelectGroup(ChatGroup.whisper);
                this.Hide();
            }
        }

        public void OnMeToggleChanged() {
            if (this.MeToggle.isOn) {
                this.SelectGroup(ChatGroup.me);
                this.Hide();
            }
        }


        public ChatGroup SelectedChatGroup() {
            return this.selectedChatGroup;
        }

        private void SelectGroup(ChatGroup group) {
            this.selectedChatGroup = group;
        }

        public void Show(System.Action hideHandler) {
            if (false == this.gameObject.activeSelf) {
                this.gameObject.SetActive(true);
                this.hideHandler = hideHandler;
            }
        }

        public void Hide() {
            if(true == this.gameObject.activeSelf) {
                this.gameObject.SetActive(false);
                if (this.hideHandler != null) {
                    this.hideHandler();
                }
            }
        }

        public bool Visible() {
            return this.gameObject.activeSelf;
        }
    }
}
