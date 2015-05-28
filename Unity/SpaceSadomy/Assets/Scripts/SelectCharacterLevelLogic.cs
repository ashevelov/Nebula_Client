using UnityEngine;
using System.Collections;
using Nebula.UI;
using Nebula;

public class SelectCharacterLevelLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (G.Game == null) {
            return;
        }
        NRPC.RequestUserInfo();

        if (MainCanvas.Get) {
            MainCanvas.Get.Show(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Show(CanvasPanelType.CreateCharacterOrPlayView);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
