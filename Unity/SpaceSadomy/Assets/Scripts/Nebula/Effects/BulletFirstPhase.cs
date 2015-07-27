namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;
	
	public class BulletFirstPhase : MonoBehaviour {

		private float mDelaySecondPhase;
		protected BulletSecondPhase mSecondPhase; 
		private GameObject mEffectContainer;
		private bool mEffectManualDisable = true;

		private MyTools.ActionMethod onEndPhaseEvent = new MyTools.ActionMethod();


		public void SetOnEndPhaseEvent(System.Action eventID)
		{
			onEndPhaseEvent.Add(eventID);
		}


		public void SetEffect(GameObject effect, bool manualDisable = true)
		{
			mEffectContainer = effect;
			mEffectManualDisable = manualDisable; 
		}

		public GameObject GetEffect()
		{
			return mEffectContainer;
		}

		public void SetEnable(bool b)
		{
			enabled = b;
			if (mEffectManualDisable == true && mEffectContainer != null)
				mEffectContainer.SetActive(b);
		}

		public void StartPhase(float startDelayForSecondPhase, BulletSecondPhase second,float timerEnd,GameObject targ)
		{
			mDelaySecondPhase 	= startDelayForSecondPhase;
			mSecondPhase 		= second;
			mSecondPhase.SetTarget(targ);
			mSecondPhase.SetEnable(false);

			StartCoroutine(TimedEnd(timerEnd));
		}
		
		public virtual void Update()   
		{

			if (mSecondPhase != null && mSecondPhase.GetTarget() == null)
			{
				if (!mSecondPhase.isStarted())
				{
					onEndPhaseEvent.Run();

					SetEnable(false);
					mSecondPhase.StartPhase();
				}

				mSecondPhase.EndEffect();
				return ;
			}


			if (mDelaySecondPhase <= 0 && mSecondPhase!= null && !mSecondPhase.isStarted())
			{
				onEndPhaseEvent.Run();

				SetEnable(false);
				mSecondPhase.StartPhase();
			}

			if (mDelaySecondPhase > 0)
				mDelaySecondPhase -= Time.deltaTime;
		}

		public float GetDelayToSecondPhase() {return mDelaySecondPhase;}

		private IEnumerator TimedEnd(float timer)
		{
			yield return new WaitForSeconds(timer);
			if (mEffectContainer != null)
				Destroy(this.mEffectContainer);
		}
	}
}
