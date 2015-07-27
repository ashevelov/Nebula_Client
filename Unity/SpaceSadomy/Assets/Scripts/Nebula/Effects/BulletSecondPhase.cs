namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;
	using Nebula.Resources;

	public enum BulletType
	{
		explosion,
		magnito,
		gravi,
		poison,
		poison2
	};

    public class BulletSecondPhase : MonoBehaviour {

		private bool  mStarted = false;
        private  GameObject mTarget;
		protected Vector3 mTargetOffset;

        private float mSpeed = 1.0f;
        private bool mMoving = false;
		private BulletType 	 mBulletType;

		private ShieldEffect mTargetBuf;

		private GameObject mLine;
		private GameObject mEffectContainer;

		public bool 	  isStarted()				 { return mStarted; }
		public void 	SetTargetOffset(Vector3 offset){ mTargetOffset = offset;}
		public void		  SetTarget(GameObject targ) 
		{
			mTarget = targ; 
			BaseSpaceObject baseObj = targ.GetComponent<BaseSpaceObject>();
			mTargetBuf = baseObj.GetShield();
		}

		public GameObject GetLine(){return mLine;}

		public void InitLine(bool active = false)
		{
			mLine = gameObject.GetChildrenWithName("Line");
			if (mLine != null)
				mLine.SetActive(active);
		}

		public GameObject GetTarget()				 { return mTarget; }
		public float 	  GetSpeed()			     { return mSpeed; }

		public void SetEnable(bool b)
		{
			enabled = b;
			if (mEffectContainer != null)
			{
				mEffectContainer.SetActive(b);
			}
		}
		public void SetEffect(GameObject effect)
		{
			mEffectContainer = effect;
		}

		public void Init(bool isMissed,BulletType _type = BulletType.explosion,bool LineActive = false)
		{
			mMoving = true;
			mStarted = false;
			mBulletType = _type;

			InitLine(LineActive);

			if (isMissed)
				SetTargetOffset(new Vector3(Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10)));
		}
		
		public void StartPhase() 
		{
			mStarted = true;
			SetEnable(true);
			SetLookToTarget();
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
		private void SetLookToTarget()
		{
			if (mTarget == null)
				return;

			Vector3 TargetP = mTarget.transform.position + mTargetOffset ;
			Vector3 forw = (TargetP - transform.position).normalized; 
			
			transform.LookAt(TargetP);
			transform.forward = forw;
		}
		
		public virtual void FixedUpdate() 
		{
		
			if (mStarted) 
			{
                if (!mTarget) { EndEffect(); return; }
				 
                if (mMoving) 
				{

					SetLookToTarget();

					transform.Translate(new Vector3(0,0,mSpeed),Space.Self);

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
			else if (mBulletType == BulletType.magnito)
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/BulletExplosionElectro"), transform.position, rot) as GameObject;
			else  if (mBulletType == BulletType.gravi)
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/BulletExplosionGravi"), transform.position, rot) as GameObject;
			else  if (mBulletType == BulletType.poison)
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/bulletExplosionPoison"), transform.position, rot) as GameObject;
			else  if (mBulletType == BulletType.poison2)
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/bulletExplosionPoison2"), transform.position, rot) as GameObject;
			else
				obj = MonoBehaviour.Instantiate(PrefabCache.Get("Effects/BulletExplosion"), transform.position, rot) as GameObject;

			obj.AddComponent<TimedObjectDestructorManual>().DestroyWithDelay(3.5f);

			if (mEffectContainer != null)
				mEffectContainer.SetActive(false);

			MeshRenderer mesh = GetComponent<MeshRenderer>();
			if (mesh!= null)
				mesh.enabled = false;

            Destroy(gameObject,0.5f);
			Destroy(this.transform.parent.gameObject,0.5f);
        }

		protected float targetDistance {
            get {
                return Vector3.Distance(mTarget.transform.position+mTargetOffset, transform.position);
            }
        }



	}

}
