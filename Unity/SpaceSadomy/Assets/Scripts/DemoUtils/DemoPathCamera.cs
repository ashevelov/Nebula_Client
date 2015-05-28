using UnityEngine;
using System.Collections;

public class DemoPathCamera : MonoBehaviour {

    public Transform lookTarget;
    public float smoothing = 1.0f;
    public float reachRadius = 0.5f;

#if ENABLE_DEMO_PATH_CAMERA
    private bool active = false;
    private int pointIndex = 0;
    private DemoCameraPath cameraPath;
    private Vector3 currentVelocity = Vector3.zero;
#endif

    public void StartWithPath(Transform lookTarget, GameObject pathPrefab )
    {
#if ENABLE_DEMO_PATH_CAMERA
        if (false == this.active)
        {
            this.lookTarget = lookTarget;
            GameObject pathGameObject = Instantiate(pathPrefab) as GameObject;
            pathGameObject.transform.parent = lookTarget;
            pathGameObject.transform.localPosition = Vector3.zero;
            pathGameObject.transform.localRotation = Quaternion.identity;
            this.cameraPath = pathGameObject.GetComponent<DemoCameraPath>();
            this.cameraPath.Initialize();
            this.pointIndex = 0;
            this.active = true;
        }
#endif
    }

    public void Stop()
    {
#if ENABLE_DEMO_PATH_CAMERA
        if (this.active)
        {
            if(this.cameraPath)
            {
                Destroy(this.cameraPath.gameObject);
                this.cameraPath = null;
            }
            this.active = false;
        }
#endif
    }

    void Start()
    {
    }

    public void AdvancePoint()
    {
#if ENABLE_DEMO_PATH_CAMERA
        pointIndex++;
#endif
    }

    public bool Active
    {
        get
        {
#if ENABLE_DEMO_PATH_CAMERA
            return this.active;
#else
            return false;
#endif
        }
    }


    void LateUpdate()
    {
#if ENABLE_DEMO_PATH_CAMERA
        if (this.active)
        {
            Transform target = cameraPath.GetPoint(pointIndex);
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, smoothing);
            //transform.position = Vector3.Slerp(transform.position, target.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(lookTarget.position - transform.position);

            if (SU_SpaceSceneCamera.Get)
                SU_SpaceSceneCamera.Get.CameraUpdate();

            if(Vector3.Distance(transform.position, target.position) < this.reachRadius )
            {
                pointIndex++;
            }
        }
#endif
    }
}
