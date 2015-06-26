namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;


    public class DamageDebuff : MonoBehaviour {

        public Vector3 scaleMin;
        public Vector3 scaleMax;
        public float yInterval;
        public float xzInterval;

        private float mYTimer;
        private float mXZTimer;

        void Update() {

            float yScale = Mathf.Lerp(scaleMin.y, scaleMax.y, Mathf.PingPong(Time.time, yInterval) / yInterval);
            float xScale = Mathf.Lerp(scaleMin.x, scaleMax.x, Mathf.PingPong(Time.time, xzInterval) / xzInterval);
            float zScale = Mathf.Lerp(scaleMin.z, scaleMax.z, Mathf.PingPong(Time.time, xzInterval) / xzInterval);
            transform.localScale = new Vector3(xScale, yScale, zScale);
        }
    }
}
