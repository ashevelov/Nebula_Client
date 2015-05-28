// EventSpaceObject.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Tuesday, December 2, 2014 4:29:33 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;
using Game.Space;
using Nebula.Client;

public class EventSpaceObject : MonoBehaviour 
{

    private ClientWorldEventInfo eventInfo;

    public void SetWorldEvent(ClientWorldEventInfo eventInfo )
    {
        this.eventInfo = eventInfo;
    }

    public ClientWorldEventInfo EventInfo
    {
        get
        {
            return this.eventInfo;
        }
    }
    void OnEnable()
    {

    }

    void OnDisable()
    {
    }
}
