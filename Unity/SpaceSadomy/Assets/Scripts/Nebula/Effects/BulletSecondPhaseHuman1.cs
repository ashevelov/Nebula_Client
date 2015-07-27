namespace Nebula.Effects {
    using UnityEngine;
    using System.Collections;

	public class BulletSecondPhaseHuman1: BulletSecondPhase
	{
		private float mAngel = 0;

		public override void FixedUpdate()
		{	
			if (GetTarget() != null)
			{
				float radius = targetDistance/4;
				float x = radius*Mathf.Sin(mAngel*Mathf.Deg2Rad);
				float y = radius*Mathf.Cos(mAngel*Mathf.Deg2Rad);
				
				Vector3 offset = new Vector3(x,y,0);

				if (mAngel<360)
					mAngel+=10;
				else
					mAngel = 0;
		
				if (mTargetOffset != new Vector3(0,0,0))
					SetTargetOffset(offset);
				else
					SetTargetOffset(mTargetOffset+offset);
			}

			base.FixedUpdate();
		}

	}
}
