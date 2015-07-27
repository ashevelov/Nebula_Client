namespace Nebula.Test.Gizmo {
    using UnityEngine;
    using System.Collections;

    [ExecuteInEditMode]
    public class WorldPointGizmo : MonoBehaviour {

        public string text;
        public Color color = Color.green;
        public float radius = 1;


        void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);
           
        }
    }
}
