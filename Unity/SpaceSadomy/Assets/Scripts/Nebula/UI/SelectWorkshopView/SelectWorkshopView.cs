using UnityEngine;
using System.Collections;
using Common;
using UnityEngine.UI;
using Nebula.Client.Res;
using Nebula.Mmo.Games;

namespace Nebula.UI {
    public class SelectWorkshopView : BaseView {

        public Text WorkshopDescription;
        public UnityEngine.UI.Button CreateCharacterButton;
        public UnityEngine.UI.Button BackButton;
        public WorkshopSelectControls[] WorkshopViews;
        
        private ClientRace raceRes;
        private Workshop selectedWorkshop;

        private StringSubCache<string> stringRes = new StringSubCache<string>();

        public override void Setup(object objData) {
            base.Setup(objData);
            this.raceRes = objData as ClientRace;
            Debug.Log("Selected race is {0}".f(this.raceRes.Id));

            for (int i = 0; i < this.WorkshopViews.Length; i++) {
                if (this.WorkshopViews[i].TargetRace == this.raceRes.Id) {
                    this.WorkshopViews[i].WorkshopView.SetActive(true);
                } else {
                    this.WorkshopViews[i].WorkshopView.SetActive(false);
                }
            }

            var targetControls = this.TargetControls();
            this.selectedWorkshop = GetSelectedWorkshop(targetControls);

            for (int i = 0; i < targetControls.WorkshopToggles.Length; i++) {
                var workshopToggleInfo = targetControls.WorkshopToggles[i];
                ClientRaceWorkshop workshopRes;
                
                if (!this.raceRes.TryGetWorkshop(workshopToggleInfo.WorkshopId, out workshopRes)) {
                    Debug.LogError("Not founded workshop resource for: {0}".f(this.selectedWorkshop));
                    return;
                }
                workshopToggleInfo.WorkshopToggle.GetComponentInChildren<Text>().text = stringRes.String(workshopRes.NameStringId, workshopRes.NameStringId);
                workshopToggleInfo.WorkshopToggle.onValueChanged.AddListener((val) => {
                    if (val) {
                        this.SelectWorkshop(workshopToggleInfo.WorkshopId);
                    }
                });
            }

            this.SelectWorkshop(this.selectedWorkshop);
        }

        private Workshop GetSelectedWorkshop(WorkshopSelectControls controls) {
            foreach (var c in controls.WorkshopToggles) {
                if (c.WorkshopToggle.isOn) {
                    return c.WorkshopId;
                }
            }
            return Workshop.DarthTribe;
        }

        public void OnCreateCharacterButtonClicked() {
            //NRPC.AddCharacter(this.raceRes.Id, this.selectedWorkshop);
            SelectCharacterGame.CreateCharacter(MmoEngine.Get.LoginGame.GameRefId, raceRes.Id, selectedWorkshop, "");
            MainCanvas.Get.Show(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Show(CanvasPanelType.CreateCharacterOrPlayView);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectWorkshopView);
        }

        public void OnBackButtonClicked() {
            MainCanvas.Get.Show(CanvasPanelType.SelectRaceView);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectWorkshopView);
        }

        private void SelectWorkshop(Workshop workshop) {
            this.selectedWorkshop = workshop;
            ClientRaceWorkshop workshopRes;
            if (!this.raceRes.TryGetWorkshop(this.selectedWorkshop, out workshopRes)) {
                Debug.LogError("Not founded workshop resource for: {0}".f(this.selectedWorkshop));
                return;
            }
            this.WorkshopDescription.text = stringRes.String(workshopRes.DescriptionStringId, workshopRes.DescriptionStringId);
        }

        private WorkshopSelectControls TargetControls() {
            for (int i = 0; i < this.WorkshopViews.Length; i++) {
                if (this.WorkshopViews[i].TargetRace == this.raceRes.Id) {
                    return this.WorkshopViews[i];
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class WorkshopSelectControls {
        public Race TargetRace;
        public GameObject WorkshopView;
        public WorkshopSelectToggle[] WorkshopToggles;
    }

    [System.Serializable]
    public class WorkshopSelectToggle {
        public Workshop WorkshopId;
        public Toggle WorkshopToggle;
    }
}