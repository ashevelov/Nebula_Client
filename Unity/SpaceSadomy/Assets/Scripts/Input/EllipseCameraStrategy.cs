using UnityEngine;
using System.Collections;
using Game.Space;
using Common;

public class EllipseCameraStrategy : ZoomedCameraStrategy 
{
    private float orbitDegreesPerSec = 90.0f;
    private Vector3 upVector = Vector3.up;

    private Vector3 smoothVel;
    private float smoothTime = 0.3f;

    public EllipseCameraStrategy(Transform cameraTransform, EllipseCameraParameters cameraParameters)
        : base(cameraTransform, cameraParameters)
    {
        this.orbitDegreesPerSec = cameraParameters.OrbitSpeed;
        this.upVector = cameraParameters.UpVector;
    }

    public override void Initialize()
    {
        base.Initialize();
        UpdateCamera();
    }

    public override void UpdateCamera()
    {
        if (this.HasTarget())
        {
            this.UpdateTargetOrbitDistance();
            this.UpdateCurrentOrbitDistance();
            this.Orbit();
        }
    }

    public override CameraStrategyType GetCameraStrategyType()
    {
        return CameraStrategyType.Ellipse;
    }

    public override void SetTarget(Transform targetTransform)
    {
        base.SetTarget(targetTransform);
        this.UpdateCamera();
    }

    private void Orbit()
    {
        Vector3 newPosition = this.TargetTransform().position + (this.CameraTransform().position - this.TargetTransform().position).normalized * this.CurrentOrbitDistance();
        this.CameraTransform().position = Vector3.SmoothDamp(this.CameraTransform().position, newPosition, ref smoothVel, smoothTime);
        this.CameraTransform().RotateAround(this.TargetTransform().position, this.upVector, orbitDegreesPerSec * Time.deltaTime);
        this.CameraTransform().LookAt(this.TargetTransform());
        Debug.Log("current ellipse camera orbit distance: {0}".f(this.CurrentOrbitDistance()));
    }

}
