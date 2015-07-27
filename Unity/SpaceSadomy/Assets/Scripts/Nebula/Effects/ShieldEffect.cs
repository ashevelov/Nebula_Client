using Common;
using Nebula;
using Game.Space;
using Nebula.UI;
using System.Collections;
using UnityEngine;
using ServerClientCommon;
using Nebula.Mmo.Games;
using Nebula.Test;
using Nebula.Resources;
using Nebula.Mmo.Items;
using Nebula.Effects;
namespace Nebula.Effects
{

	public class ShieldEffect : MonoBehaviour
	{
		const float MaxLimit = 0.1f;
		const float MinList = 0.02f;

		private float mStr = 0;

		private Renderer mRender;
		private bool 	mHit = false;
		private float 	mRadius = 3;
		private Color 	mColor;


		public void Hit(Transform parent, Transform trg)
		{ 
			mHit = true; 
			ShieldExp.Init(parent,trg, GetRadius());
		}

		public float GetRadius() { return mRadius; }

 		public void Init(float _radius, Color _color) 
		{
			mRender =  gameObject.GetComponent<Renderer>();
			gameObject.transform.localScale *= _radius;

			mRadius = _radius;

			if (mRender == null)
				Debug.Log("erropr");
			else
			{
				mColor = _color;
				mRender.material.SetColor("_TintColor", mColor);
			}

			Destroy(gameObject, 6);

		}


		public void Update()
		{
			if (mStr<MinList)
			{
				mStr+=0.002f;
				if (mStr>=MinList) mStr = MinList;
			}

			if (!mHit)
			{
				if (mStr > MinList)
				{
					mStr -= 0.005f;
					if (mStr <= MinList) mStr = MinList;
				}
				
			}else
			{
				if (mStr < MaxLimit)
				{
					mStr += 0.005f;
					if (mStr >= MaxLimit) {mHit=false; mStr = MaxLimit;}
				}

			}

			mColor.a = mStr;
			mRender.material.SetColor("_TintColor", mColor);
		}
	}
}
