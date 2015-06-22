namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using UnityEngine.UI;
    using Nebula.Client.Inventory.Objects;
    using Nebula.Resources;

    public class MaterialItemView : BaseItemView {

        public Text NameText;
        public Text DescriptionText;

        public override void SetObject(ItemInfoView.ItemContentData contentData) {

            MaterialInventoryObjectInfo material = contentData.Data as MaterialInventoryObjectInfo;

            var data = DataResources.Instance.OreData(material.TemplateId);

            this.NameText.text = StringCache.Get(material.Name);
            this.DescriptionText.text = StringCache.Get(data.Description);


        }
    }
}
