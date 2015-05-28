using UnityEngine;
using System.Collections;

public class LocalGuns : MonoBehaviour {

    private LineRenderer _lineDirection;

    void Start()
    {
        _lineDirection = GetComponent<LineRenderer>();
    }

	void Update () {
        _lineDirection.SetPosition(0, transform.position);
        _lineDirection.SetPosition(1, transform.position + (transform.forward * 10000));
	}
}
