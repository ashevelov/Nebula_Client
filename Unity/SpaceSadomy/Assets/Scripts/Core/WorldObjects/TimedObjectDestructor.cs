using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour {

    public float lifeTime = 4.0f;

    void Start()
    {
        Invoke("DestroyNow", lifeTime);
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }
}
