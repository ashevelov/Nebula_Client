using UnityEngine;
using System.Collections;
using Nebula.Mmo.Games;

namespace Nebula.UI {
    public class SelectCharacterView : BaseView {
        
        public const float UPDATE_TOGGLES_INTERVAL = 0.5f;

        public CharacterToggle[] CharacterToggles;
        private float updateTogglesTimer;

        void Start() {
            SelectCharacterGame.GetCharacters(MmoEngine.Get.LoginGame.GameRefId);
            this.updateTogglesTimer = UPDATE_TOGGLES_INTERVAL;
        }

        void OnEnable() {
            Events.PlayerCharactersReceived += Events_PlayerCharactersReceived;
        }

        void OnDisable() {
            Events.PlayerCharactersReceived -= Events_PlayerCharactersReceived;
        }

        void Update() {
            this.updateTogglesTimer -= Time.deltaTime;
            if (this.updateTogglesTimer <= 0.0f) {
                this.updateTogglesTimer += UPDATE_TOGGLES_INTERVAL;
                //update toggles here
                UpdateView();
            }
        }


        private void UpdateView() {

            if (G.Game == null) {
                return;
            }

            var playerCharacters = MmoEngine.Get.SelectCharacterGame.PlayerCharacters.Characters;

            if (playerCharacters == null) {
                return;
            }
            for (int i = 0; i < this.CharacterToggles.Length; i++) {
                if (i < playerCharacters.Count) {
                    this.CharacterToggles[i].SetData(playerCharacters[i]);
                } else {
                    this.CharacterToggles[i].SetData(null);
                }
            }
        }

        private void Events_PlayerCharactersReceived() {
            UpdateView();
        }
    }
}
