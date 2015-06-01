using UnityEngine;
using System.Collections;
using Nebula.UI;
using Nebula;
using Nebula.Mmo.Games;

public class SelectCharacterLevelLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine(CorRequestCharacters());

        if (MainCanvas.Get) {
            MainCanvas.Get.Show(CanvasPanelType.SelectCharacterView);
            MainCanvas.Get.Show(CanvasPanelType.CreateCharacterOrPlayView);
        }
	}

    IEnumerator CorRequestCharacters() {
        while(MmoEngine.Get.SelectCharacterGame.CurrentStrategy != GameState.SelectCharacterConnected) {
            yield return null;
        }
        Debug.Log(string.Format("get characters request").Color("green"));
        SelectCharacterGame.GetCharacters(MmoEngine.Get.LoginGame.GameRefId);
    }
	
}
