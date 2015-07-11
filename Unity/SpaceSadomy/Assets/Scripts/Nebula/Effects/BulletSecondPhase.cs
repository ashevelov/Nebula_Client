namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;
	using Nebula.Resources;

	public enum BulletType
	{
		explosion,
		magnito
	};

    public class BulletSecondPhase : MonoBehaviour {

		private bool  mStarted = false;
        private GameObject mSource;
        private  GameObject mTarget;
		protected Vector3 mTargetOffset;
        private float mSpeed = 1.0f;
        private bool mMoving = false;
		private BulletType 	 mBulletType;

		private ShieldEffect mTargetBuf;

		public bool 	  isStarted()				 { return mStarted; }
		public void 	SetTargetOffset(Vector3 offset){ mTargetOffset = offset;}
		public void		  SetTarget(GameObject targ) 
		{
			mTarget = targ; 
			BaseSpaceObject baseObj = targ.GetComponent<BaseSpaceObject>();
			mTargetBuf = baseObj.GetShield();
		}

		public GameObject GetTarget()				 { return mTarget; }
		public float 	  GetSpeed()			     { return mSpeed; }

		public void Init(GameObject source, BulletType _type = BulletType.explosion)
		{
			mSource = source;
			mMoving = true;
			mStarted = false;
			mBulletType = _type;
		}
		
		public void StartPhase() 
		{
			mStarted = true;
		}
		/*
		private bool RaycastHit(out RaycastHit hit)
		{

			Vector3 movementThisStep = transform.position - mPreviousPosition; 
			float movementSqrMagnitude = movementThisStep.sqrMagnitude;
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);

			Debug.Log("magn "+movementMagnitude);
			if (Physics.Raycast(transform.position, transform.forward, out hit, movementMagnitude))
			{
				Debug.Log("Hitted "+hit.transform.name.ToString());
				return true;
			}

			return false;
		}*/

		public virtual void FixedUpdate() 
		{
		
			if (mStarted) 
			{
                if (!mSource) { EndEffect(); return; }
                if (!mTarget) { EndEffect(); return; }
				 
                if (mMoving) 
				{
					Vector3 TargetP = mTarget.transform.position + mTargetOffset ;
					Vector3 forw = (TargetP - mSource.transform.position).normalized; 

					transform.LookAt(TargetP);
                    transform.position += forw * mSpeed/* * Time.deltaTime*/;
 					transform.forward = forw;

					float targetMax = 5;

					if (mTargetBuf)
					{
						targetMax = mTargetBuf.GetRadius();
						Debug.Log("buf detect "+targetMax);
					}

					if (targetDistance <= targetMax)
					{
						//RaycastHit hit;
					
				
					/*	if ((RaycastHit(out hit) && hit.transform.name == mTarget.name ) ||
						    mCollider.bounds.Contains(transform.position)
						    )
						{*/
							//transform.position = mTarget.transform.position;
							mMoving = false;
							//Debug.Log("Good "+hit.distance);
							EndEffect();
					//	}
					}
				}

            }
        }

        public void EndEffect() {

			if (mTargetBuf)
				mTargetBuf.Hit(transform,mTargetBuf.gameObject.transform);

            mStarted = false;

			Quaternion rot = transform.rotation;
			rot.z*=-1;

			GameObject obj;

			if (mBulletType == BulletType.explosion)
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/BulletExplosion"), transform.position, rot) as GameObject;
			else
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/BulletExplosionElectro"), transform.position, rot) as GameObject;

			obj.AddComponent<TimedObjectDestructorManual>().DestroyWithDelay(3.5f);

            Destroy(gameObject);
			Destroy(this.transform.parent.gameObject);
        }

		private float targetDistance {
            get {
                return Vector3.Distance(mTarget.transform.position, mSource.transform.position);
            }
        }



	}

}
