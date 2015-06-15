using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UUIToggleChild : MonoBehaviour {

    public GameObject content;
    private Toggle toggle;

	void Start () {
        toggle = GetComponent<Toggle>();
	}

    public void ChildSetActive()
    {
        content.SetActive(toggle.isOn);
    }
}
