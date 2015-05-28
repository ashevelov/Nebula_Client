using UnityEngine;
using System.Collections;

public class SpriteToCamera : MonoBehaviour {

    private Camera _camera;

	void Update () {
        if (_camera != null)
        {
            transform.LookAt(_camera.transform);
        }
        else
        {
            _camera = GameObject.Find("MapCamera").GetComponent<Camera>();
        }
	}
}
