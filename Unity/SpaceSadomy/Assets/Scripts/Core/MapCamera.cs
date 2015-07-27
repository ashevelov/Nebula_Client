using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour {


    public Transform target;
    public float distance = 10.0f;
    public float minDistance = 1.5f;

    public float xSpeed = .2f;
    public float ySpeed = .05f;

    public bool newTarget = true;


    public float touchSensitivity = 3f;

    private float sensitivityZoom = 0.3f;

    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private float x = 0.0f;
    private float y = 0.0f;


    private Vector3 deltaCameraPosition;


    private bool moveToDefaultPusitionInIsland = false;


    private Vector2 oldMpos = new Vector2();
    private Vector2 prevDist = new Vector2();
    private bool resetzoom = true;

    private Vector2 deltaPositionEndTouch;



    public Vector3 sDefaultRot;
    public Vector3 sDefaultPos;
    public Vector3 sMaxPos;


    private GameObject targetObj;

    public Camera _camera;
    public SU_SpaceSceneCamera _su_camera;
    public bool su_cameraLoad = false;

    private float MoveSpeed = 2f;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (su_cameraLoad)
        {
            if (_su_camera == null)
            {
                GameObject tempGO = GameObject.FindGameObjectWithTag("SpaceScene_Camera");
                if (tempGO != null)
                    _su_camera = tempGO.GetComponent<SU_SpaceSceneCamera>();
            }
        }
    }

    private Vector3 _startPos;
    private Vector3 _oldMousePos;
    private bool targetStar = true;
    private Transform _moveTarget = null;
	
	private float lastTouchDistance;
	private float curTouchDistance;
	private float cameraDistance;
	private float ChangeDistanceSpeed = 0.01f;
	//public MinMax DistanceLimits = new MinMax(1.5f, 500.0f);

    void FixedUpdate()
    {
#if !UNITY_EDITOR && !UNITY_STANDALONE

        if (Input.touchCount ==1 && newTarget)
		{
			Touch tch = Input.touches[0];
			if(tch.phase == TouchPhase.Began)
			{
				_startPos = tch.position;
				RaycastHit hit;
				Ray ray = _camera.ScreenPointToRay(tch.position);
				if (Physics.Raycast(ray, out hit))
					//if( hit.transform.name == "island" )
				{
					targetObj = hit.transform.gameObject;
					//SetTargetIslands(hit.transform);
				}
			}

			if (tch.phase == TouchPhase.Ended && newTarget)
			{
				RaycastHit hit;
				Ray ray = _camera.ScreenPointToRay(tch.position);
				if (Physics.Raycast(ray, out hit))
					//if( hit.transform.name == "island" )
				{
					if (targetObj == hit.transform.gameObject && target != hit.transform)
					{
						//target = targetObj.transform;
						if (hit.transform.GetComponent<SpriteToCamera>() != null)
						{
							_moveTarget = targetObj.transform;
							targetStar = true;
						}
					}
				}
			}

		}
		if (Input.touchCount >= 2) {
			var touch0 = Input.GetTouch(0);
			var touch1 = Input.GetTouch(1);
			if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) {
				curTouchDistance = Vector2.Distance(touch0.position, touch1.position);
				if (curTouchDistance < lastTouchDistance) {
					distance -= Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * ChangeDistanceSpeed;
				}
				if (curTouchDistance > lastTouchDistance) {
					distance += Vector2.Distance(touch0.deltaPosition, touch1.deltaPosition) * ChangeDistanceSpeed;
				}
				//this.distance = Mathf.Clamp(this.distance, this.DistanceLimits.Min, this.DistanceLimits.Max);
				lastTouchDistance = curTouchDistance;
			}
		}
#else
		if (Input.GetMouseButtonDown(0))
		{
			_startPos = Input.mousePosition;
			RaycastHit hit;
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				//if( hit.transform.name == "island" )
			{
				targetObj = hit.transform.gameObject;
				//SetTargetIslands(hit.transform);
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit hit;
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				//if( hit.transform.name == "island" )
			{
				if (targetObj == hit.transform.gameObject && target != hit.transform)
				{
					//target = targetObj.transform;
					if (hit.transform.GetComponent<SpriteToCamera>() != null)
					{
						_moveTarget = targetObj.transform;
						targetStar = true;
					}
				}
			}
		}
		
		distance -= (Input.GetAxis("Mouse ScrollWheel")) * distance * Time.deltaTime * 3;
#endif
		/*
        if (Input.GetMouseButton(0))
        {
            _moveTarget = null;
            if (targetStar)
            {
                if (Vector3.Distance(Input.mousePosition, _startPos) > 10)
                {
                    _oldMousePos = Input.mousePosition;
                    targetStar = false;
                    //target = null;
                }
            }
            else
            {
                target.rotation = transform.rotation;
                target.position = target.TransformPoint((_oldMousePos - Input.mousePosition).normalized * Time.deltaTime * MoveSpeed * distance);
                _oldMousePos = Input.mousePosition;
            }

        }
        else
        {
*/
            targetStar = true;
            if (_moveTarget != null)
            {
                target.position = Vector3.MoveTowards(target.position, targetObj.transform.position, Time.deltaTime * Vector3.Distance(target.position, targetObj.transform.position));
            }
        //}




        if (target)
        {
            if (Input.GetMouseButtonDown(0))
            {
                oldMpos = Input.mousePosition;
                deltaPositionEndTouch = new Vector2(0.0f, 0.0f);
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 f = Input.mousePosition;
                deltaPositionEndTouch = f - oldMpos;
                oldMpos = f;
                moveToDefaultPusitionInIsland = false;
            }


            deltaPositionEndTouch.x *= (xSpeed * (2048.0f / (float)Screen.width));
            deltaPositionEndTouch.y *= (ySpeed * (1536.0f / (float)Screen.height));

            x += deltaPositionEndTouch.x;
            y -= deltaPositionEndTouch.y;

            if (x < 0)
            {
                x += 360;
            }
            if (x > 360)
            {
                x -= 360;
            }

            if (distance < minDistance)
                distance = minDistance;
            if (distance > 500)
                distance -= Time.deltaTime * 300;

            y = ClampAngle(y, yMinLimit, yMaxLimit);


            transform.localRotation = Quaternion.Euler(y, x, 0);
            transform.localPosition = (Quaternion.Euler(y, x, 0)) * new Vector3(0.0f, 0.0f, -distance) + target.position + deltaCameraPosition;
        }

        deltaCameraPosition -= deltaCameraPosition * Time.deltaTime;

        if (Mathf.Abs(deltaCameraPosition.x) < 0.2f)
            deltaCameraPosition.x = 0f;
        if (Mathf.Abs(deltaCameraPosition.y) < 0.2f)
            deltaCameraPosition.y = 0f;
        if (Mathf.Abs(deltaCameraPosition.z) < 0.2f)
            deltaCameraPosition.z = 0f;



        if (su_cameraLoad)
            _su_camera.CameraUpdate();
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }


    public void SetTarget(Transform star)
    {
        target.transform.position = star.position;
        _moveTarget = null;
    }
}
