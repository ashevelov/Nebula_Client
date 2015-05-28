using UnityEngine;
using System.Collections;

public class Shield2 : MonoBehaviour {

    public float _speed;
    private Material _material;
    private float _timer;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _timer = 0.0f;
    }

    void Update()
    {
        _timer += Time.deltaTime * _speed;
        _material.SetFloat("_Offset", Mathf.Repeat(_timer, 1.0f));
    }
}
