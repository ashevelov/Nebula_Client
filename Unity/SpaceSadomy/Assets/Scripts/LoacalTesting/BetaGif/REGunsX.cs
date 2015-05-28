using UnityEngine;
using System.Collections;

public class REGunsX : MonoBehaviour
{

    private float speed = 100;

	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            transform.localEulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.localEulerAngles -= new Vector3(0, 0, 1) * Time.deltaTime * speed;
        }
	}
}
