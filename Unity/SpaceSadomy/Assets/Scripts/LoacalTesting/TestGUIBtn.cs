using UnityEngine;
using System.Collections;
using Game.Space;
using Nebula;

[ExecuteInEditMode]
public class TestGUIBtn : MonoBehaviour
{

    public GUISkin skin;
    public string text;
    public Texture2D icon;

    private Rect _rect;
    public Color color;

    void Update()
    {
        _rect.x = transform.position.x;
        _rect.y = transform.position.y;
        _rect.width = transform.lossyScale.x;
        _rect.height = transform.lossyScale.y;
    }


    void OnGUI()
    {
        Utils.SaveMatrix();
        GUI.color = color;
        GUI.depth = (int)transform.position.z;
        GUI.Button(_rect, "", skin.button);
        GUI.color = new Color(1, 1, 1, color.a * 2);
        GUI.Label(_rect, text, skin.label);
        if (icon != null)
        {
            GUI.DrawTexture(_rect, icon);
        }
        Utils.RestoreMatrix();
    }
}
