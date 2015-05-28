using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Game.Space;
using Common;
using Nebula.Client;

namespace Nebula.UI {
    public class CharacterToggle : MonoBehaviour {
        public Text CharacterName;
        public Image CharacterRaceImage;
        public Text CharacterRace;
        public Text CharacterWorkshop;
        public Text CharacterLevel;
        public UnityEngine.UI.Toggle SelectionToggle;

        private ClientPlayerCharacter character;

        public void SetData(ClientPlayerCharacter character) {
            this.character = character;

            if (character == null) {
                this.gameObject.SetActive(false);
                return;
            } else {
                if (!this.gameObject.activeSelf) {
                    this.gameObject.SetActive(true);
                }
            }

            this.CharacterName.text = this.character.CharacterName;
            this.CharacterRaceImage.overrideSprite = SpriteCache.RaceSprite((Race)this.character.Race);
            this.CharacterRace.text = StringCache.Race((Race)this.character.Race);
            this.CharacterWorkshop.text = StringCache.Workshop((Workshop)this.character.HomeWorkshop);
            this.CharacterLevel.text = this.character.CharacterLevel.ToString();

            if (G.Game == null) {
                return;
            }

            if (G.Game.IsSelectedCharacter(this.character.CharacterId)) {
                this.SelectionToggle.isOn = true;
            } else {
                this.SelectionToggle.isOn = false;
            }
        }

        public void OnSelectionToggleValueChanged() {
            if (G.Game == null) {
                return;
            }
            if (!this.SelectionToggle.isOn) {
                return;
            }

            if (this.character == null) {
                return;
            }

            if (!G.Game.IsSelectedCharacter(this.character.CharacterId)) {
                NRPC.SelectCharacter(this.character.CharacterId, G.Game.LoginInfo.loginName, G.Game.LoginInfo.password);
            } else if (G.Game.IsSelectedCharacter(this.character.CharacterId) && (!this.SelectionToggle.isOn)) {
                this.SelectionToggle.isOn = true;
            } else if (!G.Game.IsSelectedCharacter(this.character.CharacterId) && (this.SelectionToggle.isOn)) {
                this.SelectionToggle.isOn = false;
            }
        }

    }
}