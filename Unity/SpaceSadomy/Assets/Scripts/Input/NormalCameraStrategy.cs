using UnityEngine;
using System.Collections;
using Game.Space;
using Nebula;

public class NormalCameraStrategy : ZoomedCameraStrategy
{
    
    private const float CAMERA_Y_OFFSET = 3f;
    private const float CAMERA_Z_OFFSET = -7f;

    private Vector2 xyMoving;
    private Vector2 xyMovingSmoothed;

    private Vector2 xySpeed;
    private MinMax yMinMax;
    private float smoothTime;

    private float xSmoothSpeed;
    private float ySmoothSpeed;


    public NormalCameraStrategy(Transform cameraTransform, NormalCameraParameters cameraParameters)
        : base(cameraTransform, cameraParameters)
    {
        this.xySpeed = cameraParameters.XYMouseSpeed;
        this.yMinMax = cameraParameters.YAngleMinMax;
        this.smoothTime = cameraParameters.RotationSmoothTime;
    }

    public override void SetTarget(Transform targetTransform)
    {
        base.SetTarget(targetTransform);
        this.UpdateXYMoving();
        this.ComputeTransform();
    }

    public override void Initialize()
    {
        base.Initialize();
        xyMoving.x = this.CameraTransform().eulerAngles.y;
        xyMoving.y = this.CameraTransform().eulerAngles.x;
    }
    public override void UpdateCamera()
    {
        if(MapController.MapExist())
        {
            return;
        }
        if(this.HasTarget())
        {

            if (CrossPlatformInput.IsButton(1) || CrossPlatformInput.IsButton(0))
            {
                this.UpdateXYMoving();
            }
            this.UpdateTargetOrbitDistance();
            this.ComputeTransform();
        }
    }
    public override CameraStrategyType GetCameraStrategyType()
    {
        return CameraStrategyType.Normal;
    }



    private void UpdateXYMoving()
    {
        xyMoving.x += CrossPlatformInput.HorizontalAxis * XSpeed();
        xyMoving.y -= CrossPlatformInput.VerticalAxis * YSpeed();
    }

    private void ComputeTransform()
    {
        this.xyMoving.y = Utils.ClampAngle(this.xyMoving.y, YMin(), YMax());
        xyMovingSmoothed.x = Mathf.SmoothDamp(xyMovingSmoothed.x, xyMoving.x, ref xSmoothSpeed, this.SmoothTime());
        xyMovingSmoothed.y = Mathf.SmoothDamp(xyMovingSmoothed.y, xyMoving.y, ref ySmoothSpeed, this.SmoothTime());
        Quaternion rotation = Quaternion.Euler(xyMovingSmoothed.y, xyMovingSmoothed.x, 0);
        this.CameraTransform().rotation = rotation;
        Vector3 position = rotation * Vector3.zero + this.TargetTransform().position;
        this.UpdateCurrentOrbitDistance();
        this.CameraTransform().position = position + rotation * new Vector3(0f, CAMERA_Y_OFFSET, CAMERA_Z_OFFSET - this.CurrentOrbitDistance());
    }


    private float XSpeed()
    {
        return this.xySpeed.x;
    }

    private float YSpeed()
    {
        return this.xySpeed.y;
    }


    private float YMin()
    {
        return this.yMinMax.Min;
    }

    private float YMax()
    {
        return this.yMinMax.Max;
    }

    private float SmoothTime()
    {
        return this.smoothTime;
    }
}
