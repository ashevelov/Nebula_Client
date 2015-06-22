using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Client.Res;
using Common;
using Nebula.Resources;

namespace Nebula.UI {
    public class SelectRaceView : BaseView {

        public Toggle HumansToggle;
        public Toggle BorguzandsToggle;
        public Toggle CriptizidsToggle;
        public Image RaceImage;
        public Text RaceDescriptionText;

        private ClientRace selectedRace;


        private StringSubCache<string> stringRes = new StringSubCache<string>();

        void Start() {
            var raceRes = DataResources.Instance.ResRaces();

            this.HumansToggle.GetComponentInChildren<Text>().text 
                = stringRes.String(raceRes.Races()[Race.Humans].NameStringId, raceRes.Races()[Race.Humans].NameStringId).Trim();
            this.BorguzandsToggle.GetComponentInChildren<Text>().text 
                = stringRes.String(raceRes.Races()[Race.Borguzands].NameStringId, raceRes.Races()[Race.Borguzands].NameStringId).Trim();
            this.CriptizidsToggle.GetComponentInChildren<Text>().text 
                = stringRes.String(raceRes.Races()[Race.Criptizoids].NameStringId, raceRes.Races()[Race.Criptizoids].NameStringId).Trim();
            this.SelectRace(Race.Humans);
        }

        private void SelectRace(Race race) {
            this.selectedRace = DataResources.Instance.ResRaces().Races()[race];
            this.RaceImage.overrideSprite = SpriteCache.RaceSprite(this.selectedRace.Id);
            this.RaceDescriptionText.text = stringRes.String(this.selectedRace.DescriptionStringId, this.selectedRace.DescriptionStringId);
        }

        public void OnHumansToggleValueChanged() {
            if (this.HumansToggle.isOn) {
                this.SelectRace(Race.Humans);
            }
        }

        public void OnBorguzandsToggleValueChanged() {
            if (this.BorguzandsToggle.isOn) {
                this.SelectRace(Race.Borguzands);
            }
        }

        public void OnCriptizidsToggleValueChanged() {

            if (this.CriptizidsToggle.isOn) {
                this.SelectRace(Race.Criptizoids);
            }
        }



        public void OnNextButtonClicked() {
            print("OnNextButtonClicked()");
            if (this.selectedRace == null) {
                Debug.LogError("Selected race don't allow be null");
                return;
            }

            //move to select workshop
            MainCanvas.Get.Show(CanvasPanelType.SelectWorkshopView, this.selectedRace);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectRaceView);
        }

        public void OnBackButtonClicked() {
            MainCanvas.Get.Show(CanvasPanelType.SelectCharacterView, null);
            MainCanvas.Get.Show(CanvasPanelType.CreateCharacterOrPlayView, null);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectRaceView);
        }
    }
}
