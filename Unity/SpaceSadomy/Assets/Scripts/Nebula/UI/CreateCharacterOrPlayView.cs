using UnityEngine;
using System.Collections;
using Nebula.Mmo.Games;

namespace Nebula.UI {
    public class CreateCharacterOrPlayView : BaseView {

        public void OnPlayButtonClicked() {
            //G.Game.EnterWorld(G.Game.HomeWorldForCharacter(), G.Game.HomeWorldForCharacter());
            MmoEngine.Get.ConnectToNebulaGame();
            MainCanvas.Get.Destroy(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Destroy(CanvasPanelType.CreateCharacterOrPlayView);
        }

        public void OnDeleteButtonClicked() {
            if(MmoEngine.Get.SelectCharacterGame.PlayerCharacters.HasSelectedCharacter()) {
                SelectCharacterGame.DeleteCharacter(MmoEngine.Get.LoginGame.GameRefId,
                    MmoEngine.Get.SelectCharacterGame.PlayerCharacters.SelectedCharacterId);
            }
        }

        public void OnCreateCharacterButtonClicked() {
            MainCanvas.Get.Show(CanvasPanelType.SelectRaceView);
            MainCanvas.Get.Destroy(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Destroy(CanvasPanelType.CreateCharacterOrPlayView);
        }
    }
}
