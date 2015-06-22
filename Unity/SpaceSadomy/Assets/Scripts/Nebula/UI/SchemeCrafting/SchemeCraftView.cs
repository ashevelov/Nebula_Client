namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Nebula.Resources;

    public class SchemeCraftView : BaseView {

        public TypedSchemeCraftView[] TypedSchemeCraftViews;

        public override void Setup(object objData) {
            base.Setup(objData);

            foreach(var view in this.TypedSchemeCraftViews) {
                view.SetObject(null);
            }

        }

        void OnEnable() {
            Events.CraftSchemeSelected += Events_CraftSchemeSelected;
        }

        void OnDisable() {
            Events.CraftSchemeSelected -= Events_CraftSchemeSelected;
        }

        private void Events_CraftSchemeSelected(Client.Inventory.Objects.SchemeInventoryObjectInfo obj) {
            var moduleData = DataResources.Instance.ModuleData(obj.TargetTemplateId);
            if(moduleData == null ) {
                Debug.LogErrorFormat("Not found module data {0}", obj.TargetTemplateId);
                return;
            }

            foreach(var view in this.TypedSchemeCraftViews) {
                if(view.SlotType == moduleData.SlotType) {
                    view.SetObject(obj);
                    return;
                }
            }
        }

        public void OnCloseButtonClick() {
            MainCanvas.Get.Destroy(CanvasPanelType.SchemeCraftView);
        }

        public void OnCraftItEasyButton() {
            NRPC.CraftItEasy();
        }
    }
}
