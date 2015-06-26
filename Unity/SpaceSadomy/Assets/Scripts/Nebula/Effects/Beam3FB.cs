namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;

    public class Beam3FB : MonoBehaviour {

        public LineRenderer laserRenderer;
        public LineRenderer laserRendererExt;
        public Vector2 texScrollSpeed;
        public float mMovingSpeed;
        public Vector2 minWidth;
        public Vector2 maxWidth;
        public float startWidthChangeInterval;
        public float endWidthChangeInterval;




        private GameObject mSource;
        private GameObject mTarget;
        private Vector3 mCurrentPosition;
        private float mInterval;
        private float mTimer;


        private bool mStarted;
        private bool mMoving;


        public void StartEffect(GameObject inSource, GameObject inTarget, float interval) {
            mSource = inSource;
            mTarget = inTarget;
            mCurrentPosition = mSource.transform.position;
            mInterval = interval;
            mTimer = interval;
            mStarted = true;
            mMoving = true;
        }


        void Update() {
            if (mStarted) {
                if(!mSource) { EndEffect(); return; }
                if(!mTarget) { EndEffect(); return;  }

                if (mMoving) {
                    Vector3 dir = (mTarget.transform.position - mSource.transform.position).normalized;
                    float dist = (mCurrentPosition - mTarget.transform.position).magnitude;
                    Vector3 nextPos = mCurrentPosition + mMovingSpeed * dir * Time.deltaTime;
                    float nextDist = (nextPos - mTarget.transform.position).magnitude;
                    if (nextDist > dist) {
                        mMoving = false;
                        SetStartPosition(mSource.transform.position);
                        SetEndPosition(mTarget.transform.position);
                    } else {
                        SetStartPosition(mSource.transform.position);
                        SetEndPosition(nextPos);
                        mCurrentPosition = nextPos;
                    }
                } else {
                    SetStartPosition(mSource.transform.position);
                    SetEndPosition(mTarget.transform.position);
                }

                //SetStartPosition(mSource.transform.position);
                //SetEndPosition(mTarget.transform.position);

                laserRenderer.material.mainTextureOffset = Time.time * texScrollSpeed;

                float currentStartWidth = Mathf.Lerp(minWidth.x, maxWidth.x, Mathf.PingPong(Time.time, startWidthChangeInterval) / startWidthChangeInterval );
                float currentEndWidth = Mathf.Lerp(minWidth.y, maxWidth.y, Mathf.PingPong(Time.time, endWidthChangeInterval) / endWidthChangeInterval);

                laserRenderer.SetWidth(currentStartWidth, currentEndWidth);

                mTimer -= Time.deltaTime;
                if(mTimer <= 0f) { EndEffect(); return; }
            }
        }

        private void SetStartPosition(Vector3 pos) {
            laserRenderer.SetPosition(0, pos);
            laserRendererExt.SetPosition(0, pos);
        }

        private void SetEndPosition(Vector3 pos) {
            laserRenderer.SetPosition(1, pos);
            laserRendererExt.SetPosition(1, pos);
        }


        private void EndEffect() {
            mStarted = false;
            mMoving = false;
            Destroy(gameObject);
        }
    }
}
