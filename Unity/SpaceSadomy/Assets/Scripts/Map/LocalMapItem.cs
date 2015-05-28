using UnityEngine;
using System.Collections;

public class LocalMapItem : MonoBehaviour {

    public Texture2D icon;
    public Color color;

    void Start()
    {
        if (icon != null)
        {
            GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material.mainTexture = icon;
            GetComponent<Renderer>().material.color = color;
        }
    }
}
