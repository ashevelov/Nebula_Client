using UnityEngine;
using System.Collections;

public class Missile2 : MonoBehaviour {

    public enum Phase { GoToStartPos, GoToTarget }

    public float lifeTime = 10.0f;
    public float startSpeed = 20.0f;
    public float standardSpeed = 100.0f;
    public float forwardLength = 10.0f;
    public float orthogonalLength = 4.0f;
    public float acceleration = 100.0f;
    public float angleSpeed = 10.0f;

    public GameObject explosion;
    public GameObject smoke;

    private bool started = false;
    private BaseSpaceObject source;
    private Transform target;

    private Vector3 startRandomDirection;
    private Phase phase;
    private float lifeTimer;
    private float waitTimer;
    private float currentSpeed;

    public void Emit(BaseSpaceObject source, Transform target)
    {
        this.source = source;
        this.target = target;
        this.startRandomDirection = Random.onUnitSphere;
        this.startRandomDirection = startRandomDirection - source.transform.forward * Vector3.Dot(startRandomDirection, source.transform.forward);
        this.waitTimer = 0;

        this.phase = Phase.GoToStartPos;
        this.lifeTimer = 0;
        this.started = true;
    }

    void Update()
    {
        if (started)
        {
            if (this.phase == Phase.GoToStartPos)
            {
                transform.rotation = Quaternion.LookRotation(this.GetStartDestination() - transform.position);
                transform.position += this.GetStartSpeed() * transform.forward * Time.deltaTime;
                if (Vector3.Distance(transform.position, GetStartDestination()) < (this.GetStartSpeed() / 15))
                {
                    this.currentSpeed = this.GetStartSpeed();
                    this.phase = Phase.GoToTarget;
                }
            }
            else if (this.phase == Phase.GoToTarget)
            {
                if (!target)
                {
                    this.Explode();
                    return;
                }
                this.currentSpeed += this.acceleration * Time.deltaTime;
                this.currentSpeed = Mathf.Clamp(this.currentSpeed, 0, this.standardSpeed);
                transform.rotation = Quaternion.Slerp( transform.rotation,  Quaternion.LookRotation(this.target.position - transform.position), this.angleSpeed * Time.deltaTime);
                transform.position += this.currentSpeed * transform.forward * Time.deltaTime;
                if (Vector3.Distance(transform.position, target.position) < (standardSpeed / 15))
                {
                    this.Explode();
                }
                this.waitTimer += Time.deltaTime;
            }

            lifeTimer += Time.deltaTime;
            if (lifeTimer > lifeTime)
            {
                this.Explode();
            }
        }
    }

    private Vector3 GetStartDestination()
    {
        if (source)
            return source.transform.position + source.transform.forward * forwardLength + startRandomDirection * orthogonalLength;
        else
        {
            return Vector3.zero;
        }
    }

    private float GetStartSpeed()
    {
        return this.source.Speed() + this.startSpeed;
    }

    private void Explode()
    {
        this.started = false;

        if (explosion)
        {
            var explosionInstance = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            Destroy(explosionInstance, 4);
        }

        smoke.transform.parent = null;
        smoke.GetComponent<ParticleSystem>().enableEmission = false;
        smoke.AddComponent<TimedObjectDestructorManual>().DestroyWithDelay(4);
        target = null;
        Destroy(gameObject);
    }
}
