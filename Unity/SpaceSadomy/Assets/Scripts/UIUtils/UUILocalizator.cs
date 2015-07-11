using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UUILocalizator : MonoBehaviour {

    public string key;
	void Start () {
        Text text = GetComponent<Text>();
        if (text != null)
            text.text = Nebula.Resources.StringCache.Get(key);
	}
}
