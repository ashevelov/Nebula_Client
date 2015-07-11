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

	public class SpeedEffect : MonoBehaviour
	{
		private ParticleSystem mPart;
		private ParticleSystem mPart2;

		private Renderer mRender;
		private Renderer mRender2;

		private BaseSpaceObject mBase;
		private float mTimer = 16;

 		public void Init(GameObject baseItem) 
		{
			mBase = baseItem.GetComponent<BaseSpaceObject>();
			mPart = GetComponent<ParticleSystem>();
			mPart.startSpeed = 0;

			mPart2 = gameObject.GetChildrenWithName("SkillSpeed2").GetComponent<ParticleSystem>();
			mPart2.startSpeed = 0;

			mRender =  gameObject.GetComponent<Renderer>();
			mRender2 = gameObject.GetChildrenWithName("SkillSpeed2").GetComponent<Renderer>();
		
		}

		private void End()
		{
			Destroy(this.gameObject);	
		}

		private float SpeedCutter(float _speed)
		{
			float speed =0;
			if (_speed>5) 
				speed = 5;
			else if (_speed != 0) 
				speed = 2;

			return speed;
		}

		public void Update()
		{
			float speed = SpeedCutter(mBase.Speed());	

			if (mTimer>0 && mTimer<1)
			{
				Color renderColor = mRender.material.GetColor("_TintColor");
				Color render2Color = mRender2.material.GetColor("_TintColor");

				if (render2Color.a>0)
				{
					render2Color.a-=0.01f;
					if (render2Color.a<=0) render2Color.a=0;
				}

				if (renderColor.a>0)
				{
					renderColor.a-=0.01f;
					if (renderColor.a<=0) renderColor.a=0;
				}

				mRender.material.SetColor("_TintColor", renderColor);
				mRender2.material.SetColor("_TintColor", render2Color);

			}

			if (mTimer>0)
			{
				mTimer-=Time.deltaTime;
			}else End();

			mPart2.startSpeed = speed*2;
			mPart.startSpeed = speed;
		}
	}
}
