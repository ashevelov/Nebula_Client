namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Nebula.Client;
    using UnityEngine.Events;

    public class ModuleView : MonoBehaviour {

        public Toggle ModuleSelectToggle;
        public Text LevelText;
        public Image ModuleIcon;

        private ClientShipModule module;

        public void SetObject(ClientShipModule module, UnityAction<bool> toggleAction) {
            this.module = module;

            //set level of module
            this.LevelText.text = module.level.ToString();

            //set color of level according to player level
            if(G.Game.PlayerInfo.Level >= module.level) {
                this.LevelText.color = Color.green;
            } else {
                this.LevelText.color = Color.red;
            }

            //set module icon
            this.ModuleIcon.overrideSprite = SpriteCache.SpriteModule(module.templateId);

            //clear all listeners setted previously
            this.ModuleSelectToggle.onValueChanged.RemoveAllListeners();

            //add new listener
            if(toggleAction != null) {
                this.ModuleSelectToggle.onValueChanged.AddListener(toggleAction);
            }
        }

        public ClientShipModule Module() {
            return module;
        }
    }
}
