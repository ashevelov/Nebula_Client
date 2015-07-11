using UnityEngine;
using System.Collections;
using Nebula.Mmo.Games;
using Nebula;

public class TestExitMenu : MonoBehaviour {

    private bool show = false;

    void Update() {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            show = !show;
        }
    }
    
    void OnGUI() {
        if(!show) { return; }

        Rect groupRect = new Rect((Screen.width - 200) * 0.5f, (Screen.height - 400) * 0.5f, 200, 400);
        GUI.BeginGroup(groupRect);
        GUI.Box(new Rect(0, 0, 200, 400), "");
        if(GUI.Button(new Rect(10, 10, 180, 30), "MENU")) {
            MmoEngine.Get.ExitMenu();

        }
        GUI.EndGroup();

    }
}
