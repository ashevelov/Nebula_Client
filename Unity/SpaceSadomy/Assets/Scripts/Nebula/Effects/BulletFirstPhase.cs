namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;
	
	public class BulletFirstPhase : MonoBehaviour {

		private float mDelaySecondPhase;
		protected BulletSecondPhase mSecondPhase; 
		
		public void StartPhase(float startDelayForSecondPhase, BulletSecondPhase second,float timerEnd,GameObject targ)
		{
			mDelaySecondPhase 	= startDelayForSecondPhase;
			mSecondPhase 		= second;
			mSecondPhase.SetTarget(targ);
			mSecondPhase.gameObject.SetActive(false);

			StartCoroutine(TimedEnd(timerEnd));
		}
		
		public virtual void Update()   
		{

			if (mSecondPhase != null && mSecondPhase.GetTarget() == null)
			{
				mSecondPhase.gameObject.SetActive(true);
				gameObject.SetActive(false);
				mSecondPhase.EndEffect();
				return ;
			}


			if (mDelaySecondPhase <= 0 && mSecondPhase!= null && !mSecondPhase.isStarted())
			{
				mSecondPhase.transform.position = transform.position;
				mSecondPhase.transform.rotation = transform.rotation;

				mSecondPhase.gameObject.SetActive(true);
				gameObject.SetActive(false);

				mSecondPhase.StartPhase();
			}

			if (mDelaySecondPhase > 0)
				mDelaySecondPhase -= Time.deltaTime;
		}

		public float GetDelayToSecondPhase() {return mDelaySecondPhase;}

		private IEnumerator TimedEnd(float timer)
		{
			yield return new WaitForSeconds(timer);
			Destroy(this.gameObject);

		}
	}
}
