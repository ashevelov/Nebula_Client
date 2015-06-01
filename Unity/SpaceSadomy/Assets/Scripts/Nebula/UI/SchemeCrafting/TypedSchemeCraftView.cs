namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using Nebula.Client.Inventory.Objects;
    using UnityEngine.UI;
    using Nebula.Client.Res;
    using Common;

    public class TypedSchemeCraftView : MonoBehaviour {

        public ShipModelSlotType SlotType;
        public Button SelectSchemeButton;
        public Image SchemeImage;
        public SchemeCraftMaterialView[] MaterialView;
        public Button CraftButton;
        public Text SchemeName;
        public Text SchemeLevel;

        private SchemeInventoryObjectInfo scheme;

        void OnEnable() {
            Events.ObjectTransformedAndMovedToHold += Events_ObjectTransformedAndMovedToHold;
            Events.PlayerInventoryUpdated += Events_PlayerInventoryUpdated;
        }



        void OnDisable() {
            Events.ObjectTransformedAndMovedToHold -= Events_ObjectTransformedAndMovedToHold;
            Events.PlayerInventoryUpdated -= Events_PlayerInventoryUpdated;
        }

        public void SetObject(SchemeInventoryObjectInfo scheme) {
            this.scheme = scheme;
            if(scheme == null ) {
                this.Clear();
                return;
            }

            this.CraftButton.interactable = true;
            ResSchemeData schemeData;
            if(!DataResources.Instance.Schemes.TryGetScheme(scheme.Workshop, out schemeData)) {
                Debug.LogErrorFormat("Not found scheme data for {0}", scheme.Workshop);
                return;
            }
            this.SchemeImage.overrideSprite = SpriteCache.SchemeSprite(schemeData);

            int materialViewIndex = 0;
            foreach(var pMaterial in scheme.CraftMaterials) {
                if(materialViewIndex < this.MaterialView.Length) {
                    this.MaterialView[materialViewIndex++].SetObject(pMaterial.Key, pMaterial.Value);
                }
            }

            for(;materialViewIndex < this.MaterialView.Length; materialViewIndex++) {
                this.MaterialView[materialViewIndex].SetObject(string.Empty, 0);
            }

            this.SchemeName.text = scheme.Name;
            this.SchemeLevel.text = scheme.Level.ToString();
        }

        private void Clear() {
            this.scheme = null;
            this.SchemeImage.overrideSprite = SpriteCache.TransparentSprite();
            foreach(var materialView in this.MaterialView) {
                materialView.SetObject(string.Empty, 0);
            }
            this.CraftButton.interactable = false;
            this.SchemeName.text = string.Empty;
            this.SchemeLevel.text = string.Empty;
        }

        public void OnCraftButtonClick() {
            if(this.scheme == null ) {
                return;
            }

            if (PlayerHasMaterials()) {
                ActionProgressView.DataObject data = new ActionProgressView.DataObject {
                    ActionText = "Crafting...",
                    Duration = 1f,
                    CompleteAction = () => {
                        if (MainCanvas.Get.Exists(CanvasPanelType.SchemeCraftView)) {
                            NRPC.TransformInventoryObjectAndMoveToStationHold(scheme.Type, scheme.Id);
                        }
                    }
                };

                MainCanvas.Get.Show(CanvasPanelType.ActionProgressView, data);
            } else {
                G.Game.Engine.GameData.Chat.PastLocalMessage("You don't has required materials for crafting modules...");
            }
        }

        private void Events_ObjectTransformedAndMovedToHold(string objId) {
            if(this.scheme != null && this.scheme.Id == objId ) {
                this.SetObject(null);
            }
        }

        private void Events_PlayerInventoryUpdated() {
            if(this.scheme == null ) {
                return;
            }
            if(!G.Game.Inventory.HasItem(this.scheme.Type, this.scheme.Id)) {
                this.SetObject(null);
            }
        }

        private bool PlayerHasMaterials() {
            bool hasAll = true;
            foreach(var pMaterial in scheme.CraftMaterials) {
                if(G.Game.Inventory.ItemCount( InventoryObjectType.Material, pMaterial.Key) < pMaterial.Value ) {
                    hasAll = false;
                    break;
                }
            }
            return hasAll;
        }


        public void OnSelectSchemeButtonClick() {
            MainCanvas.Get.Show(CanvasPanelType.SchemeSelectorView, this.SlotType);
        }
    }
}
