using UnityEngine;
using System.Collections;

public class TimedObjectDestructorManual : MonoBehaviour {
    public void DestroyWithDelay(float delay)
    {
        Invoke("DestroyNow", delay);
    }

    private void DestroyNow()
    {
        Destroy(gameObject);
        
    }
}
