using UnityEngine;
using System.Collections;

public class Plasma : MonoBehaviour {


    public Transform target;
    public float speed = 1000;
    public float hitDistance = 50;

    private Vector3 _derection;
	public ParticleSystem hitParticle;
	public ParticleSystem startParticle;
	public ParticleSystem moveParticle;
	public bool isHited = true;

    public static GameObject Init(Transform target, bool isHited, Transform parent, string path = "Prefabs/Effects/PlasmaLight")
    {
        GameObject go = Resources.Load(path) as GameObject;
        if (go != null)
        {
            GameObject inst = Instantiate(go) as GameObject;
            Plasma plasma = inst.GetComponent<Plasma>();
            if (plasma != null)
            {
                plasma.target = target;
			}
			plasma.transform.position = parent.position;
			//plasma.transform.position += Random.insideUnitSphere;
            plasma.startParticle.transform.parent = parent;
            //plasma.moveParticle.transform.parent = parent;
			plasma.hitParticle.gameObject.SetActive(false);
			plasma.moveParticle.gameObject.SetActive(false);
			plasma.isHited = isHited;
            Destroy(plasma.startParticle.gameObject, 2);
            return inst;

        }
        Debug.Log("hm... plasma = null");
        return null;
    }

	void Start () {
		
	}

	private float _dischargeTime = 0.5f;
	private bool fire =false;
    private bool destroy = false;
	void FixedUpdate () {

		if(fire)
		{
	        _derection = transform .position + ( transform.forward * 1000000);
	        if (target != null)
	        {
	            _derection = target.position;
				if(!isHited)
				{
					target = null;
					Destroy(gameObject , 8);
				}
	        }
            else if (!destroy)
            {
                hitParticle.enableEmission = true;
                moveParticle.enableEmission = false;
                Destroy(gameObject, 10);
                destroy = true;
            }

			//if(!destroy)
            {
                transform.LookAt(_derection);
		        transform.position += transform.forward * speed * Time.fixedDeltaTime;
			}

	        if (Vector3.Distance(transform.position, _derection) < hitDistance)
			{
				hitParticle.gameObject.SetActive(true);
				moveParticle.gameObject.SetActive(false);
	            Destroy(gameObject, 2);
	            destroy = true;
	        }

		}
		else
		{
			if(_dischargeTime >0)
			{
				_dischargeTime -= Time.deltaTime;
			}
			else
			{
				moveParticle.gameObject.SetActive(true);
                if (startParticle != null)
                    transform.position = startParticle.transform.position;
				fire = true;
			}
		}

	}
}
