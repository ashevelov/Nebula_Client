using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for handle different camera moving
/// </summary>
public abstract class CameraStrategy 
{
    private Transform cameraTransform;
    private Transform targetTransform;

    public CameraStrategy(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    public virtual void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public abstract void Initialize();
    public abstract void UpdateCamera();
    public abstract CameraStrategyType GetCameraStrategyType();

    public Transform CameraTransform()
    {
        return this.cameraTransform;
    }

    public Transform TargetTransform()
    {
        return this.targetTransform;
    }

    public bool HasTarget()
    {
        return this.targetTransform;
    }

}
