using UnityEngine;
using System.Collections;

public class HangarCamera : MonoBehaviour
{

	public Transform target;
	public float distance = 10.0f;
    public float minDistance = 1.5f;
    public float maxDistance = 500f;
    public float zoomSpeed = 1;

    private Camera _camera;
    private Vector3 deltaCameraPosition;

    void Start()
    {
        _camera = GetComponent<Camera>();
       
    }

	void Update()
	{
        distance -= (Input.GetAxis("Mouse ScrollWheel")) * zoomSpeed * Time.deltaTime;
        if (target) {

            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            deltaCameraPosition = transform.position - target.transform.position;
            transform.localPosition = deltaCameraPosition.normalized * distance;

        }
    }

	

}
