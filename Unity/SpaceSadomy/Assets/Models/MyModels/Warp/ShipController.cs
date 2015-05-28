using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour{

    public float speed;
    public float maneuverability;

	public GameObject missile;
	public Transform target;
    
    public Rigidbody rbody;

    public Vector3 trackPos;
    public Transform[] _laserTurrls;

    private bool fire = false;

    void FixedUpdate()
    {
        rbody.velocity += transform.forward * speed * Time.deltaTime;

        rbody.velocity -= rbody.velocity / 100;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(trackPos), maneuverability);


        if (Input.GetKeyDown("w"))
            speed++;
        if (Input.GetKeyDown("s"))
            speed--;

        if (Input.GetMouseButtonDown(0))
        {
            var rey = Camera.main.ScreenPointToRay(Input.mousePosition);
            trackPos = rey.direction;
        }

		if (Input.GetKeyUp(KeyCode.Space) && !fire)
		{
            fire = true;
            StartCoroutine(Fire(0.3f));
        }
    }

    private IEnumerator Fire(float time)
    {
        for (int i = 0; i < _laserTurrls.Length; i++)
        {
            GameObject go = Instantiate(missile, _laserTurrls[i].position, transform.rotation) as GameObject;

            go.GetComponent<Missile>().SetTarget(target, false);
            //PulseLaser.Init(_laserTurrls[i], target);
            yield return new WaitForSeconds(time);
        }
        fire = false;
    }
}
