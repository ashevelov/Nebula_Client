using UnityEngine;
using System.Collections;

public class LoginShipBot : MonoBehaviour {

    public float range = 2;
    public float speed = 5;
    public float maneuverability = 5;

    private Rigidbody _rg = null;


    public Vector3 _derection;
    
	void Update () {

        if (_rg == null)
        {
            _rg = GetComponent<Rigidbody>();
        }
        if (Vector3.Distance(transform.position, _derection) < 10f)
        {
            _derection = Random.insideUnitSphere * range;
        }


        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_derection - transform.position), maneuverability);
        _rg.velocity += transform.forward * speed * Time.deltaTime;
        _rg.velocity -= _rg.velocity / 2 * Time.deltaTime;
        //transform.position += transform.forward * Speed * Time.deltaTime;

	}

}
