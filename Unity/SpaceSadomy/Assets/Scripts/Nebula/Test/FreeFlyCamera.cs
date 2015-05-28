using UnityEngine;
using System.Collections;

[AddComponentMenu("Space/Free Fly Camera")]
public class FreeFlyCamera : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2}

    private RotationAxes axes = RotationAxes.MouseXAndY;

    private float sensitivityX = 15.0f;
    private float sensitivityY = 15.0f;

    private float minimumX = -360.0f;
    private float maximumX = 360.0f;

    private float minimumY = -89.0f;
    private float maximumY = 89.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Quaternion originalRotation;

    //[SerializeField]
    private float maxSpeed = 10.0f;

    //[SerializeField]
    private float accelerationAmount = 1.0f;

    //[SerializeField]
    private float accelerationRatio = 3.0f;

    //[SerializeField]
    private float slowDownRation = 0.2f;

    private float flySpeed = 1.0f;
    private bool shift;
    private bool ctrl;

    [SerializeField]
    private bool cameraActive = false;

    public void Init(Vector2 sensitivity, float flySpeed)
    {
        this.sensitivityX = sensitivity.x;
        this.sensitivityY = sensitivity.y;
        this.flySpeed = flySpeed;
    }


    void Start()
    {
        this.originalRotation = transform.localRotation;
        this.cameraActive = false;
        this.axes = RotationAxes.MouseXAndY;
    }

    public void SetCameraActive(bool val)
    {
        this.cameraActive = val;
        this.originalRotation = transform.localRotation;
    }

    public bool CameraActive
    {
        get
        {
            return this.cameraActive;
        }
    }

    private float xVel;
    private float yVel;
    private float zVel;
    private float smoothTime = 0.5f;

    void Update()
    {
        if (this.cameraActive)
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
                transform.Translate(Vector3.forward * this.flySpeed * vert);
            }
            float horz = Input.GetAxis("Horizontal");
            if (horz != 0.0f)
            {
                transform.Translate(Vector3.right * this.flySpeed * horz);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.up * this.flySpeed);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.down * this.flySpeed);
            }

            //print(this.axes.ToString() + "x: " + this.rotationX + "  y: " + this.rotationY);

            if (this.axes == RotationAxes.MouseXAndY)
            {
                this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
                this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;

                this.rotationX = this.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
                this.rotationY = this.ClampAngle(this.rotationY, this.minimumY, this.maximumY);

                Quaternion xQuaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(this.rotationY, -Vector3.right);

                //transform.localRotation = this.originalRotation * xQuaternion * yQuaternion;


                Quaternion qTarget = this.originalRotation * xQuaternion * yQuaternion;;
                
                /*
                float x = Mathf.SmoothDampAngle(transform.localRotation.x, qTarget.eulerAngles.x, ref xVel, smoothTime);
                float y = Mathf.SmoothDampAngle(transform.localRotation.y, qTarget.eulerAngles.y, ref yVel, smoothTime);
                float z = Mathf.SmoothDampAngle(transform.localRotation.z, qTarget.eulerAngles.z, ref zVel, smoothTime);
                transform.localRotation = Quaternion.Euler(x, y, z);
                 * */
                transform.localRotation = Quaternion.Slerp(transform.localRotation, qTarget, Time.deltaTime * 3);
            }
            else if (this.axes == RotationAxes.MouseX)
            {
                this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
                this.rotationX = this.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
                Quaternion xQuaterion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
                transform.localRotation = this.originalRotation * xQuaterion;
            }
            else
            {
                this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
                this.rotationY = this.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
                Quaternion yQuaternion = Quaternion.AngleAxis(-this.rotationY, Vector3.right);
                transform.localRotation = this.originalRotation * yQuaternion;
            }
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
