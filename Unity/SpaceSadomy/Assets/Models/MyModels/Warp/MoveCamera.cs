using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

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


	private Vector2 oldMpos=new Vector2();
//	private Vector2 prevDist=new Vector2();
	private bool resetzoom=true;

	private Vector2 deltaPositionEndTouch;



	public Vector3 sDefaultRot;
	public Vector3 sDefaultPos;
	public Vector3 sMaxPos;


	private GameObject targetObj;

    public Camera _camera;
    private SU_SpaceSceneCamera _su_camera;
    public bool su_cameraLoad = false;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (su_cameraLoad)
        {
            GameObject tempGO = GameObject.FindGameObjectWithTag("SpaceScene_Camera");
            if (tempGO != null)
                _su_camera = tempGO.GetComponent<SU_SpaceSceneCamera>();
        }
    }

	void FixedUpdate()
	{

        if (Input.GetMouseButtonDown(0) && newTarget)
			{
				RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				if( Physics.Raycast( ray, out hit ) )
					//if( hit.transform.name == "island" )
				{
					targetObj = hit.transform.gameObject;
					//SetTargetIslands(hit.transform);
				}
			}
        if (Input.GetMouseButtonUp(0) && newTarget)
			{
				RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				if( Physics.Raycast( ray, out hit ) )
					//if( hit.transform.name == "island" )
				{
                    if (targetObj == hit.transform.gameObject && target != hit.transform)
                    {
                        target = targetObj.transform;
                    }
                    //    SetTargetIslands( hit.transform );
				}
				targetObj = null;
			}



        distance -= (Input.GetAxis("Mouse ScrollWheel")) * distance;



        if ( target )
		{
			if (Input.GetMouseButtonDown(1))
			{
				oldMpos=Input.mousePosition;
				deltaPositionEndTouch=new Vector2( 0.0f, 0.0f );
			}
			if (Input.GetMouseButton(1))
			{
				Vector2 f = Input.mousePosition;
				deltaPositionEndTouch=f-oldMpos;
				oldMpos=f;
				moveToDefaultPusitionInIsland = false;
			}

			
			deltaPositionEndTouch.x*=( xSpeed*( 2048.0f/( float )Screen.width ) );
			deltaPositionEndTouch.y*=( ySpeed*( 1536.0f/( float )Screen.height ) );
			
			x += deltaPositionEndTouch.x;
			y -= deltaPositionEndTouch.y;
			
			if( x < 0 )
			{
				x += 360;
			}
			if( x > 360 )
			{
				x -= 360;
			}

            if (distance < minDistance)
                distance = minDistance;
			if( distance > 500 )
				distance -= Time.deltaTime * 300;
			
			y = ClampAngle( y, yMinLimit, yMaxLimit );
            //if (Input.GetMouseButton(1))
            //{
            //    x = 0;
            //    y = 16;
            //}
			
			//deltaCameraPosition = oldCameraPosition - transform.position;

			
			transform.localRotation = Quaternion.Euler( y, x, 0 );
			transform.localPosition = ( Quaternion.Euler( y, x, 0 ) ) * new Vector3( 0.0f, 0.0f, -distance ) + target.position + deltaCameraPosition;
		}
		
		deltaCameraPosition -= deltaCameraPosition * Time.deltaTime;
		
		if( Mathf.Abs( deltaCameraPosition.x ) < 0.2f )
			deltaCameraPosition.x = 0f;
		if( Mathf.Abs( deltaCameraPosition.y ) < 0.2f )
			deltaCameraPosition.y = 0f;
		if( Mathf.Abs( deltaCameraPosition.z ) < 0.2f )
			deltaCameraPosition.z = 0f;




        if (su_cameraLoad)
        _su_camera.CameraUpdate();
		//deltaPositionEndTouch -= deltaPositionEndTouch * 2 * Time.deltaTime;
	}

    //public void SetTargetIslands(Transform island)
    //{
    //    target = island;	
    //    targetIslandId = target.GetComponent<Island> ().islandID;
    //    moveToDefaultPusitionInIsland = island.GetComponent<Island>().defoultRotation;
		
    //    deltaCameraPosition = transform.position - ((Quaternion.Euler (y, x, 0)) * new Vector3 (0.0f, 0.0f, -distance) + target.position);// + deltaCameraPosition);
    //}

	
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

}
