namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Client.Inventory.Objects;
    using System.Collections.Generic;
    using Nebula.Client.Inventory;
    using Nebula.Resources;

    public class SchemeSelectorView : BaseView {

        public GameObject SchemeSelectButtonPrefab;
        public Transform Content;

        public override void Setup(object objData) {

            Debug.LogFormat("Player workshop: {0}", GameData.instance.playerInfo.Workshop);

            base.Setup(objData);

            ShipModelSlotType slotType = (ShipModelSlotType)objData;
            Dictionary<string, ClientInventoryItem> typedItems;

            if(!GameData.instance.inventory.Items.ContainsKey(InventoryObjectType.Scheme)) {
                Debug.LogWarning("Not found scheme items");
                return;
            } else {
                typedItems = GameData.instance.inventory.GetTypedDict(InventoryObjectType.Scheme);
            }

            List<ClientInventoryItem> slottedItems = new List<ClientInventoryItem>();
            foreach(var pItem in typedItems) {
                var scheme = pItem.Value.Object as SchemeInventoryObjectInfo;
                var module = DataResources.Instance.ModuleData(scheme.TargetTemplateId);

                if( module.SlotType == slotType && module.Workshop == GameData.instance.playerInfo.Workshop) {
                    slottedItems.Add(pItem.Value);
                }
            }

            foreach(var sItem in slottedItems ) {
                GameObject buttonObject = Instantiate(this.SchemeSelectButtonPrefab) as GameObject;
                buttonObject.GetComponent<SchemeSelectButton>().SetObject(sItem.Object as SchemeInventoryObjectInfo);
                buttonObject.transform.SetParent(this.Content, false);
            }
        }

        public void OnCloseButtonClick() {
            MainCanvas.Get.Destroy(CanvasPanelType.SchemeSelectorView);
        }
    }
}