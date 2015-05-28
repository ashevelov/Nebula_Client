using UnityEngine;
using System.Collections;

namespace Nebula.UI {
    public class CreateCharacterOrPlayView : BaseView {

        public void OnPlayButtonClicked() {
            G.Game.EnterWorldWithSelectedCharacter();
            MainCanvas.Get.Destroy(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Destroy(CanvasPanelType.CreateCharacterOrPlayView);
        }

        public void OnDeleteButtonClicked() {
            if (G.Game.UserInfo.HasSelectedCharacter()) {
                NRPC.DeleteCharacter(G.Game.UserInfo.SelectedCharacterId);
            }
        }

        public void OnCreateCharacterButtonClicked() {
            MainCanvas.Get.Show(CanvasPanelType.SelectRaceView);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Destroy(CanvasPanelType.CreateCharacterOrPlayView);
        }
    }
}
