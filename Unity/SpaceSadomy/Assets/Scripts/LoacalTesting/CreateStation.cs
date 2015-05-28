using UnityEngine;
using System.Collections;
using Game.Space;
using Game.Space.UI;
using Nebula;

[ExecuteInEditMode]
public class CreateStation : MonoBehaviour {

	[System.Serializable]
	public class MyWin
	{
		public GUIElem group;
		public GUIElem close;
		public GUIElem stationBtn;
		public GUIElem planetaryBtn;
		public GUIElem labBtn;
		public GUIElem shipBtn;
		public GUIElem createBtn;
	}public MyWin myWin;

	[System.Serializable]
	public class GUIElem
	{
		public Rect rect;
		public GUIStyle style;
	}

	private GameObject station = null;

	private bool _visible = false;

	void Update()
	{
		Utils.SaveMatrix();
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if( Physics.Raycast( ray, out hit ) )
			{
				if( hit.transform == transform )
				{
					_visible = true;
				}
			}
		}
		if(Input.GetMouseButtonDown(1))
		{
			_visible = false;
		}
		Utils.RestoreMatrix();
	}
	
	void OnGUI()
	{
		if(_visible)
		{
			GUI.BeginGroup(myWin.group.rect, myWin.group.style);

			if(GUI.Button(myWin.close.rect, "", myWin.close.style))
			{
				_visible = false;
			}

			if(GUI.Button(myWin.stationBtn.rect, "Station", myWin.planetaryBtn.style))
			{
				if(station != null)
				{
					Destroy(station);
					station = null;
				}
				station = Instantiate(Resources.Load("Demo/Station_Empty") as GameObject) as GameObject;
				station.transform.position = transform.position;
			}
			
			if(GUI.Button(myWin.planetaryBtn.rect, "Planetary", myWin.planetaryBtn.style))
			{
				if(station != null)
				{
					Destroy(station);
					station = null;
				}
				station = Instantiate(Resources.Load("Demo/Station_ship_Empty") as GameObject) as GameObject;
				station.transform.position = transform.position;
			}
			
			if(GUI.Button(myWin.labBtn.rect, "Laboratory", myWin.planetaryBtn.style))
			{
				if(station != null)
				{
					Destroy(station);
					station = null;
				}
				station = Instantiate(Resources.Load("Demo/Laboratory_Empty") as GameObject) as GameObject;
				station.transform.position = transform.position;
			}
			
			if(GUI.Button(myWin.shipBtn.rect, "Hangar ", myWin.planetaryBtn.style))
			{
				if(station != null)
				{
					Destroy(station);
					station = null;
				}
				station = Instantiate(Resources.Load("Demo/Hangar_Empty") as GameObject) as GameObject;
				station.transform.position = transform.position;
			}
			
			if(GUI.Button(myWin.createBtn.rect, "Create", myWin.createBtn.style))
			{
				//ActionProgress.Setup(3,()=>{

				//string name = station.name;
				//name = name.Replace("Empty", "Full");
				//name = name.Replace("(Clone)", "");
				//if(station != null)
				//{
				//	Destroy(station);
				//	station = null;
				//}
				//Debug.Log(name);
				//station = Instantiate(Resources.Load("Demo/"+name) as GameObject) as GameObject;
				//station.transform.position = transform.position;
				//});
				//_visible = false;
			}

			GUI.EndGroup();
		}
		
	}
}
