
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Mmo.Items;
using Nebula.Resources;
using System.Collections.Generic;

namespace Nebula.UI {

	public class SkillMessage: MonoBehaviour 
	{
		private float mSpeed = 10.0f;
		private float mVelY = 0;
		private float mTimer = 2;
		private Vector3 mOffset = new Vector3(0,0,0);

		private string mObjId;
		private BaseSpaceObject mScreenItem;
		private Text mText;
		private float mDoubleScale = 0;

		public void SetDoubleScale() { mDoubleScale = 0.1f; }
		public void SetOffset(Vector3 off){ mOffset = off;}
		public void SetId(string id){mObjId = id;}
		public void SetTextComp(Text _text){ mText = _text;}
		public void SetScreenItem(BaseSpaceObject i) {mScreenItem = i;}

		private static Dictionary<string,ArrayList> mMsgCountByObj = new  Dictionary<string,ArrayList>();

		public float CalcVel()
		{
			float count = (mMsgCountByObj[mObjId].Count -  mMsgCountByObj[mObjId].IndexOf(this))*8;

			return count*mSpeed;
		}

		public void FixedUpdate()
		{
			if (mScreenItem != null)
			{ 
				transform.position = Camera.main.WorldToScreenPoint(mScreenItem.transform.position) + mOffset;
				transform.Translate(0,mVelY,0);
			}
		}

		public void Update()            
		{
			if (mScreenItem != null)
			{ 
				float vel = CalcVel();

				mVelY += vel*Time.deltaTime;

				if (mDoubleScale != 0)
				{
					if (mText.rectTransform.localScale.x >= 2)
						mDoubleScale*=-1;

					Vector3 _scale = mText.rectTransform.localScale;
					_scale.x+=mDoubleScale;
					_scale.y+=mDoubleScale;

					mText.rectTransform.localScale = _scale;

					if (mText.rectTransform.localScale.x <= 1)
					{
						mText.rectTransform.localScale = Vector3.one;
						mDoubleScale = 0;
					}
				}
			}

			if (mTimer>0)
				mTimer-=Time.deltaTime;
			else
				End();
		}

		private void End() 
		{
			if (mMsgCountByObj.ContainsKey(mObjId))
			{
				mMsgCountByObj[mObjId].Remove(this);

				Debug.LogWarning("REMOVE MSG "+mObjId+"Count "+mMsgCountByObj[mObjId].Count);

				if ( mMsgCountByObj[mObjId].Count == 0)
				{
					mMsgCountByObj.Remove(mObjId);
					Debug.LogWarning("REMOVE MSG "+mObjId);
				}
			}

			Destroy(this.gameObject);
		}

		private IEnumerator TimedFade(float timer)
		{
			yield return new WaitForSeconds(timer);
			mText.CrossFadeAlpha(0,1,true);
		}

		public static void AddMessage(string msg, BaseSpaceObject item, Color _color,bool isDoubleScale = false)
		{	 
			GameObject textClone = MainCanvas.Instantiate(PrefabCache.Get("Effects/DmgTextInfo"));
			textClone.transform.SetParent(MainCanvas.Get.gameObject.transform);

			Text _text = textClone.GetComponent<Text>(); 
			_text.text = msg;
			_text.color = _color;

			_text.rectTransform.localScale = Vector3.one;//.Scale(new Vector3(1,1,1));

			SkillMessage _class = textClone.GetComponent<SkillMessage>();

			_class.SetScreenItem(item);
			_class.SetTextComp(_text);

			if (isDoubleScale)
				_class.SetDoubleScale();

			textClone.transform.position = Camera.main.WorldToScreenPoint(item.transform.position);

			item.StartCoroutine(_class.TimedFade(1));

			_class.SetId(item.GetItemID());

			if (mMsgCountByObj.ContainsKey(item.GetItemID()))
			{
			/*	SkillMessage _prev = mMsgCountByObj[item.GetItemID()][0] as SkillMessage;

				if ( Vector3.Distance(_class.transform.position, _prev.transform.position )<32)
				{
					Vector2 rand = Random.insideUnitCircle*64;
					_class.SetOffset(new Vector3(rand.x,rand.y));
				}
*/
				mMsgCountByObj[item.GetItemID()].Add(_class);

				Debug.LogWarning("ADD MSG "+item.GetItemID()+"count: "+mMsgCountByObj[item.GetItemID()].Count);
			}
			else
			{
				Debug.LogWarning("ADD MSG"+item.GetItemID());

				ArrayList newArray = new ArrayList();
				newArray.Add(_class);

				mMsgCountByObj.Add(item.GetItemID(),newArray);
			}
		}   
   }
}
