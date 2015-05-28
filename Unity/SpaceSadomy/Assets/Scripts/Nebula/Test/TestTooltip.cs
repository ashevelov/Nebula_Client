// TestTooltip.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, November 5, 2014 4:47:25 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//


using UnityEngine;
using System.Collections;

public class TestTooltip : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {


	}

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 100), new GUIContent("content of box", "tooltip for box"));
        GUI.Button(new Rect(10, 150, 100, 100), new GUIContent("content of button", "tooltip of button very long, describe some actions which occurs when button pressed"));
        GUI.BeginGroup(new Rect(200, 0, 200, 200), new GUIContent("", "tooltip for group"));
        GUI.EndGroup();
        GUI.Label(new Rect(10, 300, 300, 300), GUI.tooltip);
    }
}
