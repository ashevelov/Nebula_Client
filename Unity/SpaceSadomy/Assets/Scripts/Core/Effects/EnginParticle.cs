using UnityEngine;
using System.Collections;

public class EnginParticle : MonoBehaviour {

	private ParticleSystem[] _particles;
	private float[] _pariclesStartSpeeds;
	
	private float _deltaSpeed;
	private Vector3 _oldPosition;
	private float shipSpeed;

	void Start () {
		_particles = GetComponentsInChildren<ParticleSystem>();
		_pariclesStartSpeeds = new float[_particles.Length];
		for(int i=0; i < _particles.Length; i++)
		{
			_pariclesStartSpeeds[i] = _particles[i].startSpeed;
		}
		_oldPosition = transform.position;

	}


	void Update () {
		if(_particles != null && _particles.Length != 0)
		{
			_deltaSpeed = Vector3.Distance(transform.position, _oldPosition);
			_oldPosition = transform.position;
			_deltaSpeed = (_deltaSpeed * Time.deltaTime) *5;
			shipSpeed += _deltaSpeed;
			for(int i=0; i < _particles.Length; i++)
			{
				if(_particles[i] == null)
				{
					Destroy(this);
					break;
				}
				_particles[i].startSpeed = _pariclesStartSpeeds[i] * (shipSpeed / 0.5f);
				if(shipSpeed > 1.5f)
				{
					_particles[i].startSpeed = _pariclesStartSpeeds[i]*2;
				}
				if(shipSpeed < 0.1f)
				{
					_particles[i].enableEmission = false;
				}
				else
				{
					_particles[i].enableEmission = true;
				}
			}

			if(shipSpeed < 0.02f)
			{
				shipSpeed = 0;
			}
			shipSpeed -= shipSpeed * Time.deltaTime ;

		}
	}
}
