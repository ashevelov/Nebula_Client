namespace Nebula.UI {
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class StationHUDView : BaseView {

        private MainCanvas canvas;

        void Start() {
            canvas = MainCanvas.Get;
        }

        public void OnCraftViewButtonClicked() {
            canvas.ToggleView(CanvasPanelType.SchemeCraftView);
        }

        public void OnStationViewButtonClicked() {
            canvas.ToggleView(CanvasPanelType.StationView);
        }

        public void OnInventoryViewButtonClicked() {
            canvas.ToggleView(CanvasPanelType.InventoryView);
        }

        public void OnShipInfoViewButtonClicked() {
            canvas.ToggleView(CanvasPanelType.ShipInfoView);
        }

        public void OnExitStationButtonClicked() {
            Operations.ExitWorkshop(G.Game);
        }
    }
}
