using UnityEngine;
using System.Collections;

namespace Nebula.Server {

    [AddComponentMenu("Server/Objects/Event")]
    public class EVENT : MonoBehaviour {
        public string eventID;
        public float cooldown;
        public float radius;
        public string description;

        void OnDrawGizmos() {
            Color cg = Color.yellow;
            cg.a = 0.3f;
            Gizmos.color = cg;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }

    
}
