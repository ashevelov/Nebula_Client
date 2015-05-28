using UnityEngine;
using System.Collections;

public class TapTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private string text = string.Empty;
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.touches[0];
            if (touch.tapCount > 1) {
                text = string.Format("multitap detected, tap count: {0} at touch index {1}", touch.tapCount, i);
                Debug.Log(text);
            }
        }
	}

    void OnGUI() {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
    }
}
