using UnityEngine;
using System.Collections;
using System;
using Game.Space;

public class TouchCameraStrategy : CameraStrategy {

    private TouchCameraParameters cameraParameters;

    private float x;
    private float y;
    private float lastTouchDistance;
    private float curTouchDistance;
    private float cameraDistance;


    public TouchCameraStrategy(Transform cameraTransform, TouchCameraParameters cameraParameters )
        : base (cameraTransform) {
        this.cameraParameters = cameraParameters;
    }

    public override void SetTarget(Transform targetTransform) {
        base.SetTarget(targetTransform);
        this.SetupVarialbles();
    }

    public override void Initialize() {
        if(HasTarget () ) {
            this.SetupVarialbles();
        }
    }

    public override void UpdateCamera() {

        if (!HasTarget()) {
            return;
        }

        int count = Input.touchCount;
        for (int i = 0; i < count; i++ ) {
            var touch = Input.GetTouch(i);
            this.x += touch.deltaPosition.x * this.cameraParameters.RotateSpeed.x;
            this.y -= touch.deltaPosition.y * this.cameraParameters.RotateSpeed.y;
        }

        this.y = this.ClampAngle(this.y);

        var rotation = Quaternion.Euler(y, x, 0.0f);

        if (Input.touchCount >= 2) {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);
            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) {
                this.curTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                if (curTouchDistance < lastTouchDistance) {
                    this.cameraDistance += Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * this.cameraParameters.ChangeDistanceSpeed;
                } else {
                    this.cameraDistance -= Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * this.cameraParameters.ChangeDistanceSpeed;
                }
                this.cameraDistance = Mathf.Clamp(this.cameraDistance, this.cameraParameters.DistanceLimits.Min, this.cameraParameters.DistanceLimits.Max);
                this.lastTouchDistance = this.curTouchDistance;
            }
        }


		this.CameraTransform().rotation = Quaternion.Slerp(this.CameraTransform().rotation, rotation, Mathf.Clamp01(Time.deltaTime * 20)); //rotation;

        var position = CameraTransform().rotation * (new Vector3(0f, 0f, -this.cameraDistance)) + TargetTransform().position;


        this.CameraTransform().position = position;
    }

    public override CameraStrategyType GetCameraStrategyType() {
        return CameraStrategyType.Touch;
    }

    private void SetupVarialbles() {
        x = CameraTransform().rotation.eulerAngles.y;
        x = CameraTransform().rotation.eulerAngles.x;
        //this.cameraDistance = Mathf.Clamp(Vector3.Distance(CameraTransform().position, TargetTransform().position),
            //cameraParameters.DistanceLimits.Min, cameraParameters.DistanceLimits.Max);
		this.cameraDistance = 25;
    }

    private float ClampAngle(float ang) {
        if(ang > 360f ) {
            ang -= 360f;
        } else if (ang < 0f ) {
            ang += 360f;
        }
        return ang;
    }
}
