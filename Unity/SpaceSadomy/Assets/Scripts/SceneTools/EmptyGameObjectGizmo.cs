using UnityEngine;
using System.Collections;

public class EmptyGameObjectGizmo : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
