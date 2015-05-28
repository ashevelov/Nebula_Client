using Nebula;
using UnityEngine;

public class Missile : MonoBehaviour {

    public float turnSpeed = 1.0f;
    public float forwardSpeed = 15.0f;
    public float lifeTime = 10.0f;
    public float accelerationTime = 2.0f;
    public float startSpeed = 0;
    public GameObject smoke;

    private Transform target;
    private float lifeTimer;
    private float currentSpeed;
    private float acceleration;
	private Vector3[] trackPositions;
	private int trackIndex = 0;
	private float distance;
    public GameObject explositionPrefab;

    public int deviationCount = 3;

    private bool _shieldExp = false;
    private bool isTorpedo = false;

    private string[] explosionPath = new string[]
    {
        "Prefabs/Effects/Explosion08",
        //"Prefabs/Effects/EaseExp"
    };

    private string RandomExplosionPath()
    {
        return this.explosionPath[Random.Range(0, this.explosionPath.Length - 1)];
    }

    private GameObject RandomExplosionPrefab()
    {
        return PrefabCache.Get(RandomExplosionPath());
    }

    public void SetTarget(Transform target, bool isTorpedo, float startSpeed = 20, bool shield = false)
    {
        this.isTorpedo = isTorpedo;

        if (!target) {
            TimeoutExplode();
            return;
        }

        this.target = target;
        this.currentSpeed = 50.0f;
        this.acceleration = forwardSpeed / accelerationTime;
        _shieldExp = shield;
        currentSpeed = startSpeed;
		Vector3 delta = target.position - transform.position;
        Vector3 temp_delta = delta / (deviationCount);
		Vector3 deviation = temp_delta / 60;
		distance = Vector3.Distance(transform.position, target.position);

        trackPositions = new Vector3[deviationCount];

		for(int i = 0; i<trackPositions.Length; i++)
		{
            if (i == 0)
            {
                trackPositions[i] = transform.position + (temp_delta * (i + 1) + Random.insideUnitSphere * currentSpeed);
            }
			else
            if(i<trackPositions.Length -1)
			{
                trackPositions[i] = transform.position + (temp_delta * (i + 1) + Random.insideUnitSphere * currentSpeed);
			}
			else
			{
				trackPositions[i] = target.position;
			}
		}
    }

    void Update()
    {
        if (trackPositions != null)
        {
            //if (trackIndex == trackPositions.Length - 1)
            {
                turnSpeed += (Time.deltaTime * (currentSpeed / distance));
            }
        }

        if (target)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(((trackIndex == trackPositions.Length - 1) ? target.position : trackPositions[trackIndex])
                                                                                              - transform.position), turnSpeed * Time.deltaTime);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
            currentSpeed += this.acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, forwardSpeed);
            if (Vector3.Distance(transform.position, trackPositions[trackIndex]) < forwardSpeed / 2)
            {
                if (trackIndex < trackPositions.Length - 1)
                {
                    trackIndex++;
                }
            }
            if ((trackIndex == trackPositions.Length - 1) && Vector3.Distance(transform.position, target.position) < forwardSpeed / 10)
            {
                if (_shieldExp && (target.GetComponents<NetworkTransformInterpolation>().Length > 0))
                {
                    ShieldExp.Init(transform, target);
                }
                TimeoutExplode();
            }
        }
        else
        {
            TimeoutExplode();
        }
        lifeTimer += Time.deltaTime;
        //if (lifeTimer >= lifeTime)
        //{
        //    TimeoutExplode();
        //}
    }

    private void TimeoutExplode()
    {
        //GameObject explositionPrefab = PrefabCache.Get("Prefabs/Effects/newExplosion");
        if (explositionPrefab)
        {
            GameObject inst = null;
            if (!isTorpedo)
            {
                inst = Instantiate(RandomExplosionPrefab(), transform.position, transform.rotation) as GameObject;
            }
            else
            {
                inst = Instantiate(explositionPrefab, transform.position, transform.rotation) as GameObject;
            }
			Destroy(inst, 4);
        }
        smoke.transform.parent = null;
        smoke.GetComponent<ParticleSystem>().enableEmission = false;
        smoke.AddComponent<TimedObjectDestructorManual>().DestroyWithDelay(4.0f);

        target = null;
        Destroy(gameObject);
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if (target)
        {
            if (collision.collider.name == target.name)
            {
                TimeoutExplode();
                //take dmg
            }
        }
    }*/

    /*
    void OnTriggerEnter(Collider collider)
    {
        if (target)
        {
            //BaseSpaceObject colliderSpaceObject = collider.GetComponent<BaseSpaceObject>();
            //BaseSpaceObject targetSpaceObject = target.GetComponent<BaseSpaceObject>();

            if (collider.name == target.name)
            {
                TimeoutExplode();
            }
        }
    }*/

}
