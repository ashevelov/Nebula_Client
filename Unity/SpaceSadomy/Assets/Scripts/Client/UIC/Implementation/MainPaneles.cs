using UnityEngine;
using System.Collections;
using Nebula.UI;

public class MainPaneles : MonoBehaviour {

    public GameObject inventory;

    public void ShowPanel()
    {
        MainCanvas.Get.Destroy(CanvasPanelType.ControlHUDView);
        MainCanvas.Get.Destroy(CanvasPanelType.TargetObjectView);
        MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
    }

    public void HidePanel()
    {
        MainCanvas.Get.Show(CanvasPanelType.ControlHUDView);
        MainCanvas.Get.Show(CanvasPanelType.TargetObjectView);
        MainCanvas.Get.Show(CanvasPanelType.ChatView);
    }
}
