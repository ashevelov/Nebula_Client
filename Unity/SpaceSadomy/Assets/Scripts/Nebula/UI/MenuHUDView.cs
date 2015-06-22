using UnityEngine;
using System.Collections;
using Common;

namespace Nebula.UI {
    public class MenuHUDView : BaseView {

        public GameObject hud;

        public void OnMapButton() {
            if (MapController.ShowHide())
            {
                hud.SetActive(false);
                MainCanvas.Get.Destroy(CanvasPanelType.ControlHUDView);
            }
            else
            {
                hud.SetActive(true);
                MainCanvas.Get.Show(CanvasPanelType.ControlHUDView);
            }
        }

        public void OnInventoryButton() { 
            //show hide inventory
            if(MainCanvas.Get == null ) {
                return;
            }
            if(MainCanvas.Get.Exists(CanvasPanelType.InventoryView)) {
                MainCanvas.Get.Destroy(CanvasPanelType.InventoryView);
            } else {
                MainCanvas.Get.Show(CanvasPanelType.InventoryView);
            }
        }

        public void OnShipInfoButton() {
            if (MainCanvas.Get == null)
            {
                return;
            }
            if (MainCanvas.Get.Exists(CanvasPanelType.ShipInfoView))
            {
                MainCanvas.Get.Destroy(CanvasPanelType.ShipInfoView);
            }
            else
            {
                MainCanvas.Get.Show(CanvasPanelType.ShipInfoView);
            }
        }

        public void OnJournalButton() { 
            //show hide journal
        }

        public void OnAngarButton() { 
           //enter to angar here
            if (G.Game == null) {
                return;
            }
            G.Game.EnterWorkshop(WorkshopStrategyType.Angar);
        }

        public void OnHelpButton() { 
            //show hide help here
        }

        //Called when Chat Button tapped
        public void OnChatButton() {
            if (MainCanvas.Get.Exists(CanvasPanelType.ChatView)) {
                MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
            } else {
                MainCanvas.Get.Show(CanvasPanelType.ChatView);
            }
        }

        public void OnGroupButtonClicked() {
            MainCanvas.Get.ToggleView(CanvasPanelType.GroupView);
        }
    }
}
