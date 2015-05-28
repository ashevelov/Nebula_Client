// MoveFromSourceToTargetBySpeed.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, November 12, 2014 11:42:48 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;

public class MoveFromSourceToTargetBySpeed : MonoBehaviour {

    public float speed = 40.0f;
    public GameObject onDestroyPrefab;

    private bool started = false;

    private float movingTimer;

    private Transform source;
    private Transform target;

    private Transform cachedTransform;
    private Vector3 targetLastPosition;

    public void Move(Transform source, Transform target )
    {
        this.cachedTransform = transform;
        this.source = source;
        this.target = target;
        this.movingTimer = 0f;

        if(this.speed == 0f )
        {
            this.speed = 1f;
        }
        targetLastPosition = transform.position;
        this.started = true;
    }
	
	void Update () 
    {
        if (this.started)
        {
            if ( (!source) || (!target) )
            {
                this.started = false;
                if (onDestroyPrefab)
                {
                    Instantiate(onDestroyPrefab, targetLastPosition, transform.rotation);
                }
                Destroy(gameObject);
                return;
            }
            targetLastPosition = target.position;

            float dist = Vector3.Distance(source.position, target.position);
            Vector3 direction = (target.position - source.position).normalized;
            float movingTime = dist / speed;

            if(movingTime == 0f )
            {
                this.started = false;
                Destroy(gameObject);
                return;
            }

            this.movingTimer += Time.deltaTime;

            float normalizedTimer = this.movingTimer / movingTime;

            if(normalizedTimer >= 1f )
            {
                normalizedTimer = 1f;
                this.started = false;
            }

            cachedTransform.position = Vector3.Lerp(source.position, target.position, normalizedTimer);
            cachedTransform.rotation = Quaternion.LookRotation(direction);

            if(false == this.started )
            {
                if (onDestroyPrefab)
                {
                    Instantiate(onDestroyPrefab, targetLastPosition, transform.rotation);
                }
                Destroy(gameObject);
            }
        }
	}
}
