using UnityEngine;
using System.Collections;

public class TouchOrbitZoom : MonoBehaviour {

    public float MinDistance = 10.0f;
    public float MaxDistance = 20.0f;
    public float ChangeDistanceSpeed = 1.0f;
    public float XSpeed = 10;
    public float YSpeed = 10;

    public Transform Target;

    private float x;
    private float y;
    private float lastTouchDistance;
    private float curTouchDistance;
    private float cameraDistance;

    void Start() {
        x = transform.rotation.eulerAngles.y;
        y = transform.rotation.eulerAngles.x;
        this.cameraDistance = this.MinDistance;
    }

    void Update () {
        float count = Input.touchCount;
        for (int i = 0; i < count; i++ ) {
            Touch tc = Input.GetTouch(i);
            this.x += tc.deltaPosition.x * XSpeed;
            this.y -= tc.deltaPosition.y * YSpeed;
        }

        var rotation = Quaternion.Euler(y, x, 0.0f);

        if( Input.touchCount >= 2 ) {
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);
            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) {
                this.curTouchDistance = Vector2.Distance(touch0.position, touch1.position);
                if ( curTouchDistance > lastTouchDistance ) {
                    this.cameraDistance += Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * this.ChangeDistanceSpeed;
                } else {
                    this.cameraDistance -= Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * this.ChangeDistanceSpeed;
                }
                this.cameraDistance = Mathf.Clamp(this.cameraDistance, this.MinDistance, this.MaxDistance);
                this.lastTouchDistance = this.curTouchDistance;
            }
        }

        var position = rotation * (new Vector3(0f, 0f, -this.cameraDistance)) + this.Target.position;

        this.transform.rotation = rotation;
        this.transform.position = position;
    }
}
