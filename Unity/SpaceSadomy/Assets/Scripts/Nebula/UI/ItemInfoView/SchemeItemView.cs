namespace Nebula.UI {
    using Common;
    using Nebula.Client.Inventory.Objects;
    using UnityEngine;
    using UnityEngine.UI;

    public class SchemeItemView : BaseItemView {

        public Text SchemeNameText;
        public Text SchemeWorkshopText;
        public Text SchemeDescriptionText;
        public Text SchemeLevelText;
        public Text TargetItemNameText;
        public GameObject CraftMaterialPrefab;
        public Transform CraftMaterialsParent;


        public override void SetObject(ItemInfoView.ItemContentData contentData) {

            SchemeInventoryObjectInfo scheme = contentData.Data as SchemeInventoryObjectInfo;

            this.SchemeNameText.text = StringCache.Get(scheme.Name);
            if(scheme.Color != Common.ObjectColor.white) {
                this.SchemeNameText.color = Utils.GetColor(scheme.Color);
            }

            this.SchemeLevelText.text = scheme.Level.ToString();
            if(G.Game.PlayerInfo.Level  < scheme.Level) {
                this.SchemeLevelText.color = Color.red;
            } else {
                this.SchemeLevelText.color = Color.green;
            }

            string workshopName = StringCache.Workshop(scheme.Workshop);
            string raceName = StringCache.Race(DataResources.Instance.ResRaces().RaceForWorkshop(scheme.Workshop));
            string fullName = "{0}({1})".f(workshopName, raceName);
            if(G.Game.PlayerInfo.Workshop != scheme.Workshop) {
                this.SchemeWorkshopText.color = Color.red;
            } else {
                this.SchemeWorkshopText.color = Color.white;
            }
            this.SchemeWorkshopText.text = fullName;
            this.SchemeDescriptionText.text = StringCache.Get("SCHEME_DESC");

            var module = DataResources.Instance.ModuleData(scheme.TargetTemplateId);
            if(module == null ) {
                Debug.LogError("Not founded module {0}".f(scheme.TargetTemplateId));
                return;
            }
            this.TargetItemNameText.text = StringCache.Get(module.NameId);

            foreach(var pMat in scheme.CraftMaterials) {
                var craftMaterialInfo = Instantiate(this.CraftMaterialPrefab) as GameObject;
                craftMaterialInfo.GetComponent<CraftMaterialInfo>().SetObject(pMat.Key, pMat.Value);
                craftMaterialInfo.transform.SetParent(this.CraftMaterialsParent, false);
            }
        }
    }
}
