// FPSCounter.cs
// Nebula
//
// Created by Oleg Zheleztsov on Thursday, January 29, 2015 7:50:29 PM
// Copyright (c) 2015 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;
using Game.Space;

public class FPSCounter : Singleton<FPSCounter>
{
	public GUISkin Skin;

	private GUIStyle labelStyle;

    [SerializeField]
    private float updateInterval = 0.5f;

    private float accum = 0.0f;
    private float frames = 0.0f;
    private float timeleft = 0.0f;

    private float result = 0.0f;

    void Start()
    {
        this.timeleft = this.updateInterval;
		this.labelStyle = Skin.GetStyle("fps");
    }

    void Update()
    {
        this.timeleft -= Time.deltaTime;
        this.accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (this.timeleft <= 0.0f)
        {
            result = this.accum / this.frames;
            this.accum = 0.0f;
            this.frames = 0;
            this.timeleft = this.updateInterval;
        }
    }

	void OnGUI () {
		GUI.Label ( new Rect(10, 10, 200, 200), string.Format("FPS: {0:F1}", Result));
	}


    public float Result
    {
        get
        {
            return this.result;
        }
    }
}
