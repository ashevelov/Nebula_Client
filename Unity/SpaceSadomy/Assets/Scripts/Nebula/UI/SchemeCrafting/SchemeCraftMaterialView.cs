namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class SchemeCraftMaterialView : MonoBehaviour {

        public Image MaterialImage;
        public Text MaterialCount;
        public Text MaterialName;

        public void SetObject(string materialId, int count ) {
            if(string.IsNullOrEmpty(materialId)) {
                this.Clear();
                return;
            }

            var materialData = DataResources.Instance.OreData(materialId);
            if(materialData == null ) {
                Debug.LogErrorFormat("Material Data {0} not founded", materialId);
                return;
            }

            this.MaterialImage.overrideSprite = SpriteCache.OreSprite(materialId);

            int playerCount = PlayerCount(materialId);
            this.MaterialCount.text = string.Format("{0}/{1}", playerCount, count);
            if(playerCount >= count ) {
                this.MaterialCount.color = Color.green;
            } else {
                this.MaterialCount.color = Color.red;
            }

            this.MaterialName.text = StringCache.Get(materialData.Name);

        }

        private void Clear() {
            this.MaterialImage.overrideSprite = SpriteCache.TransparentSprite();
            this.MaterialCount.text = string.Empty;
            this.MaterialName.text = string.Empty;
        }

        private int PlayerCount(string materialId) {
            return G.Game.Inventory.ItemCount(Common.InventoryObjectType.Material, materialId);
        }
    }
}
