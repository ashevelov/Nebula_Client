using UnityEngine;
using System.Collections;

namespace Nebula.UI {
    public class SelectCharacterView : BaseView {
        
        public const float UPDATE_TOGGLES_INTERVAL = 0.5f;

        public CharacterToggle[] CharacterToggles;
        private float updateTogglesTimer;

        void Start() {
            this.updateTogglesTimer = UPDATE_TOGGLES_INTERVAL;
        }

        void Update() {
            this.updateTogglesTimer -= Time.deltaTime;
            if (this.updateTogglesTimer <= 0.0f) {
                this.updateTogglesTimer += UPDATE_TOGGLES_INTERVAL;
                //update toggles here

                if (G.Game == null) {
                    return;
                }

                var playerCharacters = G.Game.UserInfo.Characters;

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
        }
    }
}
