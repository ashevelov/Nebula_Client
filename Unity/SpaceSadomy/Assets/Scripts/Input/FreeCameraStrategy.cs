using UnityEngine;
using System.Collections;

public class FreeCameraStrategy : CameraStrategy 
{

    private float sensitivityX = 35.0f;
    private float sensitivityY = 35.0f;

    private float minimumX = -360.0f;
    private float maximumX = 360.0f;

    private float minimumY = -89.0f;
    private float maximumY = 89.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Quaternion originalRotation;

    private float maxSpeed = 10.0f;

    private float accelerationAmount = 1.0f;

    private float accelerationRatio = 3.0f;

    private float slowDownRation = 0.2f;

    private float flySpeed = 10.0f;
    private bool shift;
    private bool ctrl;

    private float xVel;
    private float yVel;
    private float zVel;
    private float smoothTime = 0.5f;

    public FreeCameraStrategy(Transform cameraTransform) : 
        base(cameraTransform)
    {

    }

    public override void Initialize()
    {
        this.originalRotation = this.CameraTransform().localRotation;

    }

    public override CameraStrategyType GetCameraStrategyType()
    {
        return CameraStrategyType.Free;
    }

    public override void UpdateCamera()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            this.shift = true;
            this.flySpeed *= accelerationRatio;
            this.flySpeed = Mathf.Clamp(this.flySpeed, 0.0f, this.maxSpeed);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            this.shift = false;
            this.flySpeed /= this.accelerationRatio;
            this.flySpeed = Mathf.Clamp(this.flySpeed, 0.0f, this.maxSpeed);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            this.ctrl = true;
            this.flySpeed *= slowDownRation;
            this.flySpeed = Mathf.Clamp(this.flySpeed, 0.0f, this.maxSpeed);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            this.ctrl = false;
            this.flySpeed /= this.slowDownRation;
            this.flySpeed = Mathf.Clamp(this.flySpeed, 0.0f, this.maxSpeed);
        }

        float vert = Input.GetAxis("Vertical");
        if (vert != 0.0f)
        {
            CameraTransform().Translate(Vector3.forward * this.flySpeed * vert * Time.smoothDeltaTime);
        }
        float horz = Input.GetAxis("Horizontal");
        if (horz != 0.0f)
        {
            CameraTransform().Translate(Vector3.right * this.flySpeed * horz * Time.smoothDeltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            CameraTransform().Translate(Vector3.up * this.flySpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            CameraTransform().Translate(Vector3.down * this.flySpeed);
        }

        this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX * Time.deltaTime;
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY * Time.deltaTime;

        this.rotationX= this.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
        this.rotationY = this.ClampAngle(this.rotationY, this.minimumY, this.maximumY);

        curRotationX = Mathf.SmoothDamp(curRotationX, rotationX, ref curRotationXVel, 2);
        curRotationY = Mathf.SmoothDamp(curRotationY, rotationY, ref curRotationYVel, 2);

        this.curRotationX = this.ClampAngle(this.curRotationX, this.minimumX, this.maximumX);
        this.curRotationY = this.ClampAngle(this.curRotationY, this.minimumY, this.maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(this.curRotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(this.curRotationY, -Vector3.right);

        Quaternion qTarget = this.originalRotation * xQuaternion * yQuaternion;

        CameraTransform().localRotation = Quaternion.Slerp(CameraTransform().localRotation, qTarget, Time.deltaTime * 3);
    }

    private float curRotationX;
    private float curRotationY;
    private float curRotationXVel;
    private float curRotationYVel;

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
