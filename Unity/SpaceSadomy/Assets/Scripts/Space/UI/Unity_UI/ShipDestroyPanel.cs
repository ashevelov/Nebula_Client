using UnityEngine;
using System.Collections;
using Common;
using Nebula.UI;

public class ShipDestroyPanel : Nebula.UI.BaseView
{

    public void ToStation()
    {
        G.Game.TryEnterWorkshop(WorkshopStrategyType.Angar);
        MainCanvas.Get.Destroy(CanvasPanelType.ShipDestroyView);
    }
}
