namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;

    public class Beam : MonoBehaviour {

        public Transform scaledPart;

        private float mTimer;
        private bool mStarted;
        private GameObject mSource;
        private GameObject mTarget;
        private float mSpeed = 20f;
        private bool mMoving = false;


        public void StartEffect(GameObject source, GameObject target, float time) {
            mSource = source;
            mTarget = target;
            mTimer = time;
            mMoving = true;
            mStarted = true;
        }

        void Update() {

            if (mStarted) {
                if (!mSource) { EndEffect(); return; }
                if (!mTarget) { EndEffect(); return; }

                if (!mMoving) {
                    mTimer -= Time.deltaTime;
                }

                Vector3 forw = (mTarget.transform.position - mSource.transform.position).normalized; 
                if (mMoving) {
                    float distBefore = (transform.position - mTarget.transform.position).magnitude;
                    Vector3 nextPos = transform.position + forw * mSpeed;
                    float distNext = (nextPos - mTarget.transform.position).magnitude;
                    if(distNext > distBefore) {
                        transform.position = mTarget.transform.position;
                        mMoving = false;
                    } else {
                        transform.position += forw * mSpeed;
                    }
                }

                Vector3 scale = scaledPart.localScale;
                scale.y = targetDistance * 0.2f;
                scaledPart.localScale = scale;

                transform.forward = forw;

                if (mTimer <= 0f) {
                    EndEffect();
                    return;
                }
            }
        }

        private void EndEffect() {
            mStarted = false;
            Destroy(gameObject);
        }

        private float targetDistance {
            get {
                return Vector3.Distance(mTarget.transform.position, mSource.transform.position);
            }
        }
    }
}
