using UnityEngine;
using System.Collections;

namespace SpaceEffectScript
{

	public class rotate_for_asteroid : MonoBehaviour
	{
		public float ObjectRotationSpeed;
		public GameObject CenterOfRotation;
		public float AngleR;
		public float PointX;
		public float PointY;
		public float PointZ;

		void Start ()
		{
		
		}

		void Update ()
		{
			gameObject.transform.Rotate(0, ObjectRotationSpeed*Time.deltaTime, 0);
			gameObject.transform.RotateAround(CenterOfRotation.transform.position, new Vector3(PointX, PointY, PointZ), AngleR*Time.deltaTime);
		}
	}

}