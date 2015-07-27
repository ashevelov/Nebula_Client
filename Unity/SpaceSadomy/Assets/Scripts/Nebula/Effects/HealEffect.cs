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
	private Color mColor;

 	public void Init(Item item, Color _color, string res) 
	{
		mColor = _color;
		Material mat = new Material(Resources.Load(res) as Material);
	
		mat.SetColor("_TintColor",mColor);

		ShipModel shipModel   = item.GetShipModel();
		ArrayList shipModules =  shipModel.GetModules();
		mRenders = new ArrayList(shipModules.Count);
		Debug.Log("Heal arr"+mRenders.Count);
		for (int i=0;i<shipModules.Count;i++)
		{
			GameObject obj = shipModel.GetModule(i).GetModel().GetChildrenWithName("model");
			Renderer objR; 

			if ( obj != null)
				objR = shipModel.GetModule(i).GetModel().GetChildrenWithName("model").GetComponent<Renderer>();
			else
				objR = shipModel.GetModule(i).GetModel().GetComponent<Renderer>();
				
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

		if (mTimer<=1)
		{
			if (mColor.a>0)
			{
				mColor.a-=0.05f;
				if (mColor.a<=0) mColor.a=0;
			}
			
		}else
		{
			if (mColor.a<1)
			{
				mColor.a+=0.05f;
				if (mColor.a>=1) mColor.a=1;
			}
		}
		
		for (int i=0;i<mRenders.Count;i++)
		{
			Renderer objR = mRenders[i] as Renderer;

			objR.materials[1].mainTextureOffset = new Vector2(1,mTiles);
			objR.materials[1].SetColor("_TintColor",mColor);
		}

		if (mTimer>0)
			mTimer -= Time.deltaTime;
		else End();


	}

}

