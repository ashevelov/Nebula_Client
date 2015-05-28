using UnityEngine;
using System.Collections;

public class GunMover : MonoBehaviour {

    private GameObject _target;
    private float _tracking = 1f;

	void Start () {
	    
	}

    public void SetTarget( GameObject target)
    {
        _target = target;
    }

	void FixedUpdate () {
        if (_target != null)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_target.transform.position), _tracking);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
	}
}
