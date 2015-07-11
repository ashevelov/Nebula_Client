using UnityEngine;
using System.Collections;

public class SettingsPanelProcess : MonoBehaviour {

    public void ApplicationQuit()
    {
        Debug.Log("Application.Quit");
        Application.Quit();
    }

    public void SelectCharacters()
    {
        Debug.Log("ExitMenu");
        MmoEngine.Get.ExitMenu();
    }
}
