using UnityEngine;
using System.Collections;

public class NLaser : MonoBehaviour {

    public Transform target;
    public Transform source;
    public float speed;
    public float life;
    public GameObject impactEffect;

    private bool started = false;

    public void StartShot(Transform source, Transform target)
    {
        this.source = source;
        this.target = target;
        Destroy(gameObject, life);
        this.started = true;
    }

    void Update()
    {
        if (this.started)
        {
            if (!target || !source)
            {
                this.started = false;
                Destroy(gameObject);
                return;
            }

            var direction = (target.position - source.position).normalized;

            float curDistance = Vector3.Distance(transform.position, target.position);
            Vector3 move = direction * speed * Time.deltaTime;
            Vector3 nextPosition = transform.position + move;
            float nextDistance = Vector3.Distance(nextPosition, target.position);
            if (nextDistance > curDistance)
            {
                transform.position = target.position;
                this.started = false;
                Instantiate(impactEffect, target.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                transform.position = nextPosition;
            }
        }
    }
}
