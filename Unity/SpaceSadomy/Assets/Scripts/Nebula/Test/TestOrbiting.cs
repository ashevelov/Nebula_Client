namespace Nebula.Test {
    using Common;
    using UnityEngine;

    public class TestOrbiting : MonoBehaviour {

        public Transform centerTransform;
        public Transform rotateObject;
        public float phiSpeed = 0.01f;
        public float thetaSpeed = 0.01f;
        public float speed = 10.0f;
        public float radius = 10;


        private bool started = false;
        private SphericalCoord sc;

        void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                started = !started;
                if (started) {
                    Vector3 relativePos = rotateObject.position - centerTransform.position;
                    sc = Geometry.Cartesian2Spherical(new CVec(relativePos.x, relativePos.y, relativePos.z));
                    sc.R = radius;
                }
            }

            if (started) {
                sc.Phi += phiSpeed * Time.deltaTime;
                sc.Thteta += thetaSpeed * Time.deltaTime;
                //sc.R = (rotateObject.position - centerTransform.position).magnitude;
                CVec vec = Geometry.Spherical2Cartesian(sc);
                Vector3 targetPos = new Vector3(centerTransform.position.x + vec.x, centerTransform.position.y + vec.y, centerTransform.position.z + vec.z);
                Vector3 dir = (targetPos - rotateObject.position).normalized;
                rotateObject.position += dir * speed * Time.deltaTime;
            }
        }
    }
}
