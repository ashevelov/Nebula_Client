namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Client.Inventory.Objects;
    using UnityEngine.UI;
    using Nebula.Client.Res;
    using Nebula.Client;
    using Nebula.Resources;

    public class SchemeSelectButton : MonoBehaviour {

        public Button SelectButton;
        public Text LevelText;
        public Image ColorTexture;

        private SchemeInventoryObjectInfo scheme;

        public void SetObject(SchemeInventoryObjectInfo scheme) {
            this.scheme = scheme;
            ResSchemeData schemeData;
            if(!DataResources.Instance.Schemes.TryGetScheme(scheme.Workshop, out schemeData)) {
                Debug.LogException(new NebulaException(string.Format("not founded scheme data for workshop {0}", scheme.Workshop)));
            }


            this.SelectButton.image.overrideSprite = SpriteCache.SchemeSprite(schemeData);
            this.LevelText.text = scheme.Level.ToString();
            this.ColorTexture.overrideSprite = SpriteCache.SpriteColor(scheme);
        }

        public void OnSchemeSelectButtonClick() {
            Events.EvtCraftSchemeSelected(scheme);
            MainCanvas.Get.Destroy(CanvasPanelType.SchemeSelectorView);
        }
    }
}
