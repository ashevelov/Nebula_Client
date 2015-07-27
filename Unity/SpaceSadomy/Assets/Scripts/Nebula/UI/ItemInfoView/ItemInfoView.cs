namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using Nebula.Client.Inventory;
    using Nebula.Client.Inventory.Objects;
    using Common;
    using Nebula.Client;

    public class ItemInfoView : BaseView {

        public GameObject WeaponContentPrefab;
        public GameObject SchemeContentPrefab;
        public GameObject MaterialContentPrefab;
        public GameObject ModuleContentPrefab;
        public ScrollRect Scroll;


        public override void Setup(object objData) {
            base.Setup(objData);
            if(objData == null ) {
                Debug.LogError("Obj Data in ItemInfoView is null");
                return;
            }

            if (Scroll.content != null) {
                Destroy(Scroll.content.gameObject);
                Scroll.content = null;
            }

            ItemContentData data = objData as ItemContentData;
            if(data == null ) {
                Debug.LogError("Data must be of type ItemContentData");
                return;
            }

            GameObject contentPrefab = null;

            if(data.Data is IInventoryObjectInfo) {
                if(data.Data is WeaponInventoryObjectInfo) {
                    contentPrefab = this.WeaponContentPrefab;
                } else if(data.Data is SchemeInventoryObjectInfo) {
                    contentPrefab = this.SchemeContentPrefab;
                } else if(data.Data is MaterialInventoryObjectInfo) {
                    contentPrefab = this.MaterialContentPrefab;
                }
            }
            //else if(data.Data is IStationHoldableObject) {
            //    if(data.Data is ClientShipModule) {
            //        contentPrefab = this.ModuleContentPrefab;
            //    }
            //}

            if( contentPrefab == null ) {
                Debug.LogError("Content prefab is null for type: {0}".f(data.Data.GetType().Name));
                return;
            }

            GameObject contentInstance = Instantiate(contentPrefab) as GameObject;
            contentInstance.GetComponent<BaseItemView>().SetObject(data);
            contentInstance.transform.SetParent(this.Scroll.transform, false);
            this.Scroll.content = contentInstance.GetComponent<RectTransform>();
            this.Scroll.verticalNormalizedPosition = 1.0f;
        }

        public class ItemContentData {
            public bool IsOnPlayer;
            public object Data;
        }

        public void OnCloseButtonClick() {
            MainCanvas.Get.Destroy(CanvasPanelType.ItemInfoView);
        }
    }
}