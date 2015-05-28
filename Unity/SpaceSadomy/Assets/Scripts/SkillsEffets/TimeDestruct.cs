// TimeDestruct.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Friday, November 14, 2014 12:19:29 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;

public class TimeDestruct : MonoBehaviour 
{

    public float lifeTime = 3.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
	
}
