namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Common;

    public class CraftMaterialInfo : MonoBehaviour {

        public Image IconImage;
        public Text NameText;
        public Text CountText;

        public void SetObject(string materialId, int materialCount ) {

            var material = DataResources.Instance.OreData(materialId);

            if(material == null ) {
                Debug.LogError("Not founded material {0}".f(materialId));
                return;
            }

            this.IconImage.overrideSprite = SpriteCache.OreSprite(materialId);
            this.NameText.text = StringCache.Get(material.Name);
            this.CountText.text = StringCache.Get("COUNT_FMT").f(materialCount);
        }
    }
}
