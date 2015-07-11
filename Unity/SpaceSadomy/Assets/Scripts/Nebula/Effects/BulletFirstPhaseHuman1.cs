namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;

	public class BulletFirstPhaseHuman1: BulletFirstPhase
	{
		private float mDelayToLook;
		private float mSpeed = 4;

		public void SetSpeed(float speed)		{ mSpeed = speed; }
		public void SetDelayToLook(float delay) { mDelayToLook = delay; }

		public override void Update()
		{

			if (mSecondPhase.GetTarget() != null)
			{
				if (GetDelayToSecondPhase() <= mDelayToLook)
				{
					Vector3 forw = (mSecondPhase.GetTarget().transform.position - gameObject.transform.position).normalized;
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(forw), 2.0f * Time.deltaTime);
					transform.position += forw  * mSpeed *  Time.deltaTime;
				}else
				{
					transform.Translate(new Vector3(0,0,mSpeed * Time.deltaTime));
				}
			}

			base.Update();
		}

	}
}
