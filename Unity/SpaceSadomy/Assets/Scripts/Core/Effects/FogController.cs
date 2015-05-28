using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour {

    public float speed = 100;
    private ParticleSystem _particle;
    private Vector3 _oldPos;
    private float _emissionRate;
    private float _addEmissionRate;

	void Start () {
        _particle = GetComponent<ParticleSystem>();
        _emissionRate = _particle.emissionRate;
        _oldPos = transform.parent.position;
	}
	void Update () {
        float deltaSpeed = Vector3.Distance(_oldPos, transform.parent.position);
        Vector3 deltaPos = transform.parent.position - _oldPos;
        transform.localPosition = -deltaPos * 10;
        _oldPos = transform.parent.position;
        _addEmissionRate += (deltaSpeed * Time.deltaTime * speed);
        _addEmissionRate -= _addEmissionRate/2 *Time.deltaTime;
        _particle.emissionRate = _emissionRate + _addEmissionRate;
	}
}

