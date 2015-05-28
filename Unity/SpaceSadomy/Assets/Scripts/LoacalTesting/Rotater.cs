using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    Vector3 randomPosition;
	void Update () {

        if (Vector3.Distance(transform.position, randomPosition) < 1)
        {
            randomPosition = Random.insideUnitSphere * 10;
        }

        Vector3 delta = randomPosition - transform.position;

        transform.eulerAngles += new Vector3(1, 1, 1) * Time.deltaTime;
        transform.position += delta.normalized * Time.deltaTime;
	}
}
