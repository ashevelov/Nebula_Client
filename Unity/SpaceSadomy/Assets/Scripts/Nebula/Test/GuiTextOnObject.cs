//// GuiTextOnObject.cs
//// Nebula
//// 
//// Created by Oleg Zhelestcov on Sunday, November 16, 2014 2:59:44 PM
//// Copyright (c) 2014 KomarGames. All rights reserved.
////

//using UnityEngine;
//using System.Collections;
//using Game.Space;
//using Nebula;

//public class GuiTextOnObject : MonoBehaviour, ISpaceObjectGUI
//{
//    public Vector2 pixelOffset;
//    public string text = string.Empty;
//    public Color textColor = Color.white;

//    private GUIStyle textStyle;
//    private Rect textRect;

//    void OnEnable()
//    {
//        if (G.UI)
//        {
//            textStyle = G.UI.GetSkin("game").GetStyle("font_middle_center");
//            textRect = Utils.WorldPos2ScreenRect(transform.position, Vector2.zero);
//            G.UI.AddSpaceObjectGui(this);
//        }
//    }

//    void OnDisable()
//    {
//        if(G.UI)
//            G.UI.RemoveSpaceObjectGui(this);
//    }


//    #region ISpaceObjectGUI
//    public void Draw()
//    {
//        GUI.Label(textRect, text.Color(textColor), textStyle);
//    }

//    public void Update()
//    {
//        this.textRect = Utils.WorldPos2ScreenRect(transform.position, Vector2.zero);
//        this.textRect = this.textRect.addOffset(pixelOffset);
//    }

//    public GameObject GetObject()
//    {
//        return gameObject;
//    } 
//    #endregion
//}
