using UnityEngine;
using System.Collections;
using Nebula.UI;

namespace Client.UIC.Implementation
{
    public class MainPaneles : MonoBehaviour
    {

        public GameObject inventory;
		public GameObject tutor;

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

		const string TUTTOR_KEY = "TUTTOR_KEY";
		void Start()
		{
			if(PlayerPrefs.GetInt(TUTTOR_KEY, 0) == 0)
			{
				PlayerPrefs.SetInt(TUTTOR_KEY, 1);
				tutor.SetActive(true);
				ShowPanel();
				gameObject.SetActive(false);
			}
		}
    }
}
