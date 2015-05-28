using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

public class BetaHangarShip : MonoBehaviour {

	public float speed = 10;
	
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			transform.eulerAngles -= new Vector3(0, Input.GetAxis("Mouse X") * Time.deltaTime * speed, 0);
		}
	}
}
