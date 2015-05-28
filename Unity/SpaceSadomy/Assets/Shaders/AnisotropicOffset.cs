using UnityEngine;
using System.Collections;

public class AnisotropicOffset : MonoBehaviour {

	public float offsetSpeed = 1.0f;
	private Material mat;

	// Use this for initialization
	void Start () {
		mat = this.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

		float offset = Mathf.Repeat(Time.time * offsetSpeed, 2) - 1;
		mat.SetFloat("_AnisoOffset", offset);
	}
}
