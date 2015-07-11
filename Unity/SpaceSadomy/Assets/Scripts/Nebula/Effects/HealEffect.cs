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


public class HealEffect : MonoBehaviour
{

	float mTiles = 1;
	ArrayList mRenders;
	float mTimer = 2.5f;

 	public void Init(Item item) 
	{
		Material mat = new Material(Resources.Load("Effects/SkillHeal") as Material);
		Color _col = mat.GetColor("_TintColor");
		_col.a = 0;
		mat.SetColor("_TintColor",_col);

		ShipModel shipModel   = item.GetShipModel();
		ArrayList shipModules =  shipModel.GetModules();
		mRenders = new ArrayList(shipModules.Count);

		for (int i=0;i<shipModules.Count;i++)
		{
			Renderer objR = shipModel.GetModule(i).GetModel().GetChildrenWithName("model").GetComponent<Renderer>();
				
			Material[] newMat = new Material[objR.materials.Length+1];
				
			objR.materials.CopyTo(newMat, 0);
			newMat[1] = mat;
			objR.materials = newMat;

			mRenders.Add(objR);
		}

	}

	private void End()
	{	
		for (int i=0;i<mRenders.Count;i++)
		{
			Renderer objR = mRenders[i] as Renderer;
			Material[] newMat = new Material[objR.materials.Length-1];

			newMat[0] = objR.materials[0];
			objR.materials = newMat;
		}


		Destroy(this.gameObject);
	}

	void Update()
	{
		mTiles += 0.01f;

		if (mTiles>=10) 
		{
			mTiles =0;
		}

		for (int i=0;i<mRenders.Count;i++)
		{
			Renderer objR = mRenders[i] as Renderer;

			objR.materials[1].mainTextureOffset = new Vector2(1,mTiles);


			if (mTimer<=1)
			{
				Color _col = objR.materials[1].GetColor("_TintColor");
				if (_col.a>0)
				{
					_col.a-=0.05f;
					if (_col.a<=0) _col.a=0;
				}
				objR.materials[1].SetColor("_TintColor",_col);

			}else
			{
				Color _col = objR.materials[1].GetColor("_TintColor");
				if (_col.a<1)
				{
					_col.a+=0.05f;
					if (_col.a>=1) _col.a=1;
				}
				objR.materials[1].SetColor("_TintColor",_col);
			}
		}

		if (mTimer>0)
			mTimer -= Time.deltaTime;
		else End();


	}

}

