using UnityEngine;
using System.Collections;

public class TestObjectWithRigidBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("start rigidbody");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("collider entered " + other.name);
    }

    void OnTriggerExit(Collider other)
    {
        print("collider exited " + other.name);
    }
}
