// RotateAroundLocalZ.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, November 12, 2014 11:31:02 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;

public class RotateAroundLocalZ : MonoBehaviour {

    public float rotationSpeed = 1.0f;

    private Transform cachedTransform;

    void Start()
    {
        //cache transform for optimization Speed
        this.cachedTransform = transform;
    }

	void Update () 
    {
        cachedTransform.Rotate(Vector3.forward, this.rotationSpeed * Time.deltaTime);
	}
}
