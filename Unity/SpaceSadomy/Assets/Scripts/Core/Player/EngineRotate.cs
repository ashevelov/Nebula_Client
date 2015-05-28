using UnityEngine;
using System.Collections;

public class EngineRotate : MonoBehaviour {

    public Vector3 _oldRotate;
    public Vector3 _curentRotate;
    public Vector3 _endRotate;
    public float _powerTilt = 0.01f;
    public float _returnPower = 1;
    private bool hungar = false;

    void Start()
    {
        hungar = (Application.loadedLevelName == "Angar");
        _oldRotate = transform.eulerAngles;
    }

    void Update()
    {
        if (!hungar)
        {
            Vector3 eulerAngles = transform.eulerAngles;

            Vector3 delta = transform.eulerAngles - _oldRotate;

            delta.x = 0;
            delta.y = ClampAngle(delta.y, -50, 50);
            delta.z = ClampAngle(delta.z, -50, 50);

            //Debug.Log("delta = " + delta);
            _curentRotate += delta * _powerTilt * Time.deltaTime;
            _curentRotate -= _curentRotate * _returnPower * Time.deltaTime;
            transform.localEulerAngles = _curentRotate;
            _oldRotate = transform.eulerAngles;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180)
            angle += 360;
        if (angle > 180)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
