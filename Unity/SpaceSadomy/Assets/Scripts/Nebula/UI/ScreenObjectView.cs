using Game.Network;
namespace Nebula.UI {

    using UnityEngine;
    using System.Collections;
    using Common;
    using Nebula.Resources;
    using Nebula.Mmo.Items;

    public class ScreenObjectView : MonoBehaviour {

        private Transform parent;
        private BaseSpaceObject parentBaseSpaceObject;
        private Item item;


        private RectTransform selfRectTransform;
        private RectTransform mainCanvasRectTransform;
        private Canvas canvas;

        private bool startCalled;
        private Camera cam;
        private GameObject targetSelectionView;

        void Start() {
            this.selfRectTransform = GetComponent<RectTransform>();
            this.mainCanvasRectTransform = MainCanvas.Get.ObjectScreenPanel();
            transform.SetParent(mainCanvasRectTransform, false);
            canvas = MainCanvas.Get.GetComponent<Canvas>();
            cam = Camera.main;
            this.startCalled = true;
        }


        public void Setup(BaseSpaceObject bso) {
            this.parent = bso.transform;
            this.parentBaseSpaceObject = bso;
            this.item = bso.Item;

            Debug.Log("Set sprite for item type name: {0}".f(bso.GetItemTypeName()));
            this.name = bso.GetItemTypeName();

            var image = this.GetComponent<UnityEngine.UI.Image>();
            image.overrideSprite = SpriteCache.ItemType(bso.GetItemTypeName());

            //set green color for event
            if (bso.GetItemTypeName() == ItemTypeName.EVENT) {
                image.color = Color.green;
            } else {

                image.color = Utils.RaceColor(item.Race);
            }

            
        }

        private bool AllOk() {
            return (startCalled) && (parent != null);
        }


        void Update() {
            if (AllOk() == false) {
                return;
            }

            float ang = Vector3.Angle(cam.transform.forward, parent.position - cam.transform.position);

            if (ang < 90) {
                this.selfRectTransform.anchoredPosition = cam.WorldToScreenPoint(parent.position) * 1.0f / canvas.scaleFactor;
            } else {
                this.selfRectTransform.anchoredPosition = new Vector2(-1000, -1000);
            }

            HandleTarget();
        }

        private void HandleTarget() {
            if(G.PlayerItem == null ) {
                return;
            }
            if( G.PlayerItem.Target.HasTarget && (G.PlayerItem.Target.TargetId == this.item.Id)) {
                if(targetSelectionView == null ) {
                    targetSelectionView = Instantiate(MainCanvas.Get.GetMiscPrefab("TargetView")) as GameObject;
                    targetSelectionView.transform.SetParent(this.selfRectTransform, false);
                }
            } else {
                if(targetSelectionView != null ) {
                    Destroy(targetSelectionView);
                    targetSelectionView = null;
                }
            }
        }

        public void OnSelectionButtonClick() {
            if(G.PlayerItem == null ) {
                return;
            }
            G.PlayerItem.RequestTarget(this.item.Id, this.item.Type, true);
        }

    }

}

