namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Nebula.Client.Inventory;
    using UAction = UnityEngine.Events.UnityAction<bool>;
    using UnityEngine.UI;

    public class InventoryItemView : MonoBehaviour {

        public UnityEngine.UI.Toggle ActionButton;
        public UnityEngine.UI.Text CountText;
        public UnityEngine.UI.Image ColorImage;
        public UnityEngine.UI.Image IconImage;

        private ClientInventoryItem dataObject;

        //set data object for this item view and setup controls
        public void SetDataObject(ClientInventoryItem item, UAction action = null) {
            this.dataObject = item;

            if (this.dataObject == null || this.dataObject.Has == false ) {
                //set sprite to missedStri
                IconImage.overrideSprite = SpriteCache.MissedSprite();
                this.ActionButton.interactable = false;
                this.CountText.text = string.Empty;
                this.ColorImage.gameObject.SetActive(false);
            } else {
                IconImage.overrideSprite = SpriteCache.SpriteForItem(this.dataObject.Object);
                this.ActionButton.interactable = true;
                this.CountText.text = this.dataObject.Count.ToString();

                if((this.dataObject.Object as IColorInfo).HasColor() ) {
                    if ((this.dataObject.Object as IColorInfo).MyColor() != Common.ObjectColor.white) {
                        this.ColorImage.overrideSprite = SpriteCache.SpriteColor(this.dataObject.Object);
                    } else {
                        this.ColorImage.gameObject.SetActive(false);
                    }
                } else {
                    this.ColorImage.gameObject.SetActive(false);
                }

                this.ActionButton.onValueChanged.RemoveAllListeners();

                var group = this.GetComponentInParent<ToggleGroup>();
                this.ActionButton.group = group;
                /*
                this.ActionButton.onClick.AddListener(() => {
                    //select item
                    Debug.Log("Select item");
                });*/
                if(action != null) {
                    this.ActionButton.onValueChanged.AddListener(action);
                }
            }
        }

        //return data object id
        public string DataObjectId() {
            if(this.dataObject == null || (this.dataObject.Has == false)) {
                return string.Empty;
            }
            return dataObject.Object.Id;
        }

        public ClientInventoryItem DataObject() {
            return this.dataObject;
        }
    }
}
