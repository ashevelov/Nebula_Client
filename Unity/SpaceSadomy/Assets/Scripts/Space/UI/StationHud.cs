//using UnityEngine;
//using System.Collections;

//namespace Game.Space.UI
//{
//    public class StationHud
//    {
//        private UIManager uiManager;
//        private UIContainerEntity root;
//        private UIButton craftModuleButton;
        

//        public StationHud()
//        {
//            this.uiManager = G.UI;
//            this.root = this.uiManager.GetLayout("station_hud") as UIContainerEntity;
//            this.craftModuleButton = this.root.GetChildrenEntityByName("craft_module") as UIButton;
//            this.craftModuleButton.RegisterHandler(OnCraftModule);
//            UIEntity inventoryHold = UIManager.Get.GetLayout("inventory_hold");

//            (this.root.GetChildrenEntityByName("exit_station") as UIButton).RegisterHandler(OnExitStation);
//            (this.root.GetChildrenEntityByName("station_hold") as UIButton).RegisterHandler(OnShowHold);
//        }

//        void OnCraftModule(UIEvent evt)
//        {
//            this.uiManager.ModuleCraftingView.Show(!this.uiManager.ModuleCraftingView.Visible);
//        }

//        void OnShowHold(UIEvent evt)
//        {
//            if (this.uiManager.InventoryHold.Visible == false)
//            {
//                this.uiManager.InventoryHold.Visible = true;
//            }
//        }

//        void OnExitStation(UIEvent evt)
//        {
//            G.Game.TryExitWorkshop();
//        }

//        public bool Visible
//        {
//            get
//            {
//                return this.root.Visible;
//            }
//        }

//        public void SetVisible(bool val)
//        {
//            this.root.SetVisibility(val);
//        }
//    }
//}

