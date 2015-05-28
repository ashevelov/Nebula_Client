using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Space;
using System.Xml.Serialization;
using System.IO;
using Game.Space.UI;
using Nebula;

public class MapController : MonoBehaviour {

    public List<StarSystem> _starSystems = new List<StarSystem>();
    public List<LocalMapItem> _localMapItems = new List<LocalMapItem>();
    public Camera _camera;
    private List<string> _route = new List<string>();
    private string _routeEndPos = string.Empty;

	private Transform screenPanel;

    //private GUIStyle switchButtonStyle;

	private StarSystem curentSS = null;

    private GameObject mObj;


    void Awake()
    {
        //SaveToXml();
        LoadOfXml();

        curentSS = _starSystems.Find((s) => { return s._id == DataResources.Instance.ZoneForId(MmoEngine.Get.Game.World.Name).Scene(); });

        if (curentSS != null)
        {
            mObj = GameObject.Instantiate(PrefabCache.Get("Prefabs/map/MyShip_map")) as GameObject;
            //		mObj.transform.parent = curentSS.GetGO().transform;
            mObj.transform.position = curentSS.pos + Vector3.right;
        }
		
		if (curentSS != null)
        {
            _camera.GetComponent<MapCamera>().SetTarget( curentSS.GetGO().transform );
        }
        else
        {
            _camera.GetComponent<MapCamera>().SetTarget( _starSystems[0].GetGO().transform );
        }
        
    }



    void Start()
    {
		screenPanel = GameObject.Find("ObjectScreenPanel").transform;
		screenPanel.localPosition = new Vector3(5000,0,0);


        if (_endStarSystem != null)
            SetRoute(_endStarSystem);

        LoadLocalMap();

        //if (switchButtonStyle == null)
        //{
        //    switchButtonStyle = UIManager.Get.DefaultSkin.GetStyle("button");
        //}
    }

    void OnDestroy()
    {
        if (mObj != null)
            Destroy(mObj);
		
		screenPanel.localPosition = Vector3.zero;
//		SaveXml(_starSystems, pathXML);
    }

    private void LoadLocalMap()
    {
        GameObject localMapGO = GameObject.Find("LocalMap");
        if (localMapGO != null)
        {
            SolarMapObject[] solarMapObjects = FindObjectsOfType<SolarMapObject>();

            foreach (SolarMapObject solarMapObject in solarMapObjects)
            {
                _localMapItems.Add(solarMapObject.GetMapIcon(localMapGO.transform));
            }
        }

        //GameObject localMapGO = GameObject.Find("LocalMap");
        //if (localMapGO != null)
        //{
        //    MyPlayer mp = FindObjectOfType<MyPlayer>();
        //    if (mp != null)
        //    {
        //        GameObject mObj = GameObject.Instantiate(PrefabCache.Get("Prefabs/map/LocalMap/MyShip") as GameObject) as GameObject;
        //        mObj.transform.parent = localMapGO.transform;
        //        mObj.transform.position = mp.transform.position / 500;
        //    }

        //    EventSpaceObject[] eso = FindObjectsOfType<EventSpaceObject>();
        //    foreach (EventSpaceObject eObj in eso)
        //    {
        //        GameObject mObj = GameObject.Instantiate(PrefabCache.Get("Prefabs/map/LocalMap/event") as GameObject) as GameObject;
        //        mObj.transform.parent = localMapGO.transform;
        //        mObj.transform.position = eObj.transform.position / 500;
        //    }


        //    Station[] sobjs = FindObjectsOfType<Station>();
        //    foreach (Station eObj in sobjs)
        //    {
        //        GameObject mObj = GameObject.Instantiate(PrefabCache.Get("Prefabs/map/LocalMap/station") as GameObject) as GameObject;
        //        mObj.transform.parent = localMapGO.transform;
        //        mObj.transform.position = eObj.transform.position / 500;
        //    }

        //    Asteroid ast = FindObjectOfType<Asteroid>();
        //    if (ast != null)
        //    {
        //        GameObject mObj = GameObject.Instantiate(PrefabCache.Get("Prefabs/map/LocalMap/asteroid_fild") as GameObject) as GameObject;
        //        mObj.transform.parent = localMapGO.transform;
        //        mObj.transform.position = ast.transform.position / 500;
        //    }
            
        //}
    }

    void Update()
    {
        tapTime += Time.deltaTime;
        //_starSystems.Sort(delegate(StarSystem s1, StarSystem s2)
        //{
        //    return Vector3.Distance(s1.pos, _camera.transform.position).CompareTo(Vector3.Distance(s1.pos, _camera.transform.position));
        //});
        Vector3 tapPos;

#if !UNITY_EDITOR
        if(Input.touchCount != 0)
        {
			if(Input.touches[0].phase == TouchPhase.Began)
			{
	            tapPos = Input.touches[0].position;
	            RaycastHit hit;
	            Ray ray = _camera.ScreenPointToRay(tapPos);
	            if (Physics.Raycast(ray, out hit))
	            {
	                TapInStar(hit);
	            }
			}
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            tapPos = Input.mousePosition;
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(tapPos);
            if (Physics.Raycast(ray, out hit))
            {
                TapInStar(hit);
            }
        }
#endif
    }

    float tapTime =1;

private void TapInStar(RaycastHit hit)
{
    if (tapTime < 0.5f)
    {
        if (!localMap)
        {
            StarSystem ss = _starSystems.Find((s) => { return s.GetGO() == hit.transform.gameObject; });

            if (ss != null)
            {
                {
                    ConfirmationDialog.Setup("You want to jump", () =>
                    {
                        var game = MmoEngine.Get.Game;

                        string worldNameId = DataResources.Instance.ZoneForScene(ss._id).Id(); //MmoEngine.WorldForLevel(ss._id);

                        if (game.World.Name == worldNameId)
                        {
                            Debug.Log("you already in this world");
                        }
                        else
                        {
                            NRPC.ChangeWorld(worldNameId);
                        }
                        Debug.Log("warp -> " + worldNameId);

                        Hide();
                    }, null);
                }
            }

        }
        else
        {
            ConfirmationDialog.Setup("jump to", () =>
            {
                MyPlayer mp = FindObjectOfType<MyPlayer>();
                mp.transform.position = hit.transform.position * 600 + new Vector3(0, 80, 0);
                Hide();
            }, null);
        }
    }
    tapTime = 0;
}

    private MapCamera _moveCamera;

    private bool lMap = false;

    private bool localMap
    {
        get
        {
            return lMap;
        }
        set
        {
            if (lMap != value)
            {
                lMap = value;

                if (lMap)
                {
                    _camera.LayerCullingHide("map");
                    _camera.LayerCullingShow("local_map");
                    _camera.GetComponent<MapCamera>().SetTarget(this.transform.parent);
                    _camera.GetComponent<MapCamera>().distance = 8;
                    
                }
                else
                {
                    _camera.LayerCullingHide("local_map");
                    _camera.LayerCullingShow("map");

                    curentSS = _starSystems.Find((s) => { return s._id == DataResources.Instance.ZoneForId(MmoEngine.Get.Game.World.Name).Scene(); });

                    if (curentSS != null)
                    {
                        _camera.GetComponent<MapCamera>().SetTarget(curentSS.GetGO().transform);
                    }
                    else
                    {
                        _camera.GetComponent<MapCamera>().SetTarget(_starSystems[0].GetGO().transform);
                    }
                    _camera.GetComponent<MapCamera>().distance = 30;
                }
            }
        }
    }

    private StringSubCache<string> stringSubCache = new StringSubCache<string>();


    void OnGUI()
    {
        if(_moveCamera == null)
        {
            _moveCamera = _camera.GetComponent<MapCamera>();
        }
        else if(_moveCamera.distance < 80)
        {
            Utils.SaveMatrix();
            if (!localMap)
            {
                _starSystems.ForEach((s) =>
                {
                    string zoneNameStringId = DataResources.Instance.ZoneForId(s._id).DisplayName();
                    if (curentSS != null)
                    {
                        GUI.Label(Utils.WorldPos2ScreenRect(s.pos, new Vector2(150, 30), _camera), stringSubCache.String(zoneNameStringId, zoneNameStringId) + ((curentSS._id == s._id) ? "  (You are here)" : ""));
                    }
                    else
                    {
                        GUI.Label(Utils.WorldPos2ScreenRect(s.pos, new Vector2(150, 30), _camera), stringSubCache.String(zoneNameStringId, zoneNameStringId));
                    }
                });
            }
            else
            {
                _localMapItems.ForEach((lmi) =>
                {
                    GUI.Label(Utils.WorldPos2ScreenRect(lmi.transform.position, new Vector2(150, 30), _camera).addPos(0, 30), (lmi.transform.position * 600f).ToString());
                });
            }

//            if (GUI.Button(new Rect(Screen.width / Utils.GameMatrix().m00 - 100, Screen.height / Utils.GameMatrix().m00 - 30, 95, 25), ((localMap) ? "Galactic Map" : "Solar Map"), switchButtonStyle)) 
//            {
//                localMap = !localMap;
//            }
            Utils.RestoreMatrix();
        }

    }





    public static void ShowHide()
    {
        if (!MapExist())
        {
            _map = (GameObject)Instantiate(Resources.Load("Prefabs/map/Map"));
            _map.name = "Map";
            //if(MouseOrbitRotateZoom.Get)
            //{
            //    MouseOrbitRotateZoom.Get.Gray();
            //}
        }
        else
        {
            if (!MapController.Get.localMap)
            {
                MapController.Get.localMap = !MapController.Get.localMap;
            }
            else
            {
                Destroy(_map);
                //if(MouseOrbitRotateZoom.Get)
                //{
                //    MouseOrbitRotateZoom.Get.Ungray();
                //}
            }
        }
        
    }

    public static void Hide()
    {
        if (MapExist())
        {
            Destroy(_map);
        }
    }

    private static GameObject _map;
    public static bool MapExist()
    {
        if (_map != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static MapController Get
    {
        get
        {
            if (MapExist())
            {
                return _map.GetComponentInChildren<MapController>();
            }
            else
            {
                return null;
            }
        }
    }
    private static StarSystem _endStarSystem = null;

    public bool SetRoute(StarSystem endStarSystem)
    {
        _endStarSystem = _starSystems.Find((s) => { return s._id == endStarSystem._id; });
        for (int i = _starSystems.Count - 1; i >= 0; i--)
        {
            //Material mat;
            //mat = new Material(Shader.Find("Space/Module Group Fillbar"));
            //mat.SetColor("_Color", Color.yellow);
            //mat.SetFloat("_Value", 0.7f);
            _starSystems[i].GetGO().GetComponent<Renderer>().material = GetMaterialStarSystem(_starSystems[i]._id, _starSystems[i]._control);
        }
        Dictionary<StarSystem, int> _allStars = new Dictionary<StarSystem,int>();
        List<StarSystem> _totalRoute = new List<StarSystem>();
        _starSystems.ForEach((s) =>
            {
                _allStars.Add(s, -1);
            });
        
        curentSS = _starSystems.Find((s) => { return s._id == Application.loadedLevelName; });
        
        int d = 0;
        _allStars[curentSS] = d;

        bool stop = false;
        var _buffer = new List<StarSystem>(_allStars.Keys);
        while (!stop)
        {
            foreach (StarSystem starKey in _buffer)
            {
                if (_allStars[starKey] == d)
                {
                    starKey._nearbyStarSystems.ForEach((ns) =>
                    {
                        StarSystem tempSS = _starSystems.Find((s) => { return s._id == ns; });
                        if (_allStars[tempSS] == -1)
                        {
                            _allStars[tempSS] = d + 1;
                        }
                        if (tempSS == _endStarSystem)
                        {
                            stop = true;
                        }
                    });
                }
            }
            d++;
            if (d > _allStars.Count)
            {
                stop = true;
                return false;
            }
        }

        d = _allStars[_endStarSystem];
        StarSystem pathCurentS = _endStarSystem;
        _totalRoute.Add(_endStarSystem);
        while (d > 0)
        {
            bool founded = false;
            pathCurentS._nearbyStarSystems.ForEach((ns) =>
            {
                if (!founded)
                {
                    StarSystem tempSS = _starSystems.Find((s) => { return s._id == ns; });
                    if (_allStars[tempSS] == (d - 1))
                    {
                        _totalRoute.Add(tempSS);
                        founded = true;
                        pathCurentS = tempSS;
                    }
                }
            });

            d--;
        }
        if (_totalRoute.Count > 1)
        {

            for (int i = _totalRoute.Count - 1; i >= 0; i--)
            {
                //Material mat;
                //mat = new Material(Shader.Find("Space/Module Group Fillbar"));
                //mat.SetColor("_Color", Color.yellow);
                //mat.SetFloat("_Value", 0.7f);
                _totalRoute[i].GetGO().GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f, 1f, 0.3f, 0.5f));
            }
        }
        else
        {
            _endStarSystem = null;
        }
        return true;

    }

    public void AddRoute()
    {

    }

    public void UpdateRoute()
    {
        
    }

    public void LoadOfXml()
    {
        _starSystems = LoadXml<List<StarSystem>>(pathXML);

//		_starSystems = new List<StarSystem>();
//
//		float rndDist = 20;
//		for(int i =0; i < 15; i++)
//		{
//			StarSystem ss = new StarSystem();
//			ss._id = "H"+(i+1);
//			ss._control = Fraction.HUMANS;
//			ss.pos = Random.insideUnitSphere*rndDist + new Vector3(rndDist, 0, 0);
//			ss._nearbyStarSystems = new List<string>();
//
//			_starSystems.Add(ss);
//		}
//		for(int i =0; i < 15; i++)
//		{
//			StarSystem ss = new StarSystem();
//			ss._id = "E"+(i+1);
//			ss._control = Fraction.KRIPTIZIDS;
//			ss.pos = Random.insideUnitSphere*rndDist+ new Vector3(-rndDist, 0, 0);
//			ss._nearbyStarSystems = new List<string>();
//			
//			_starSystems.Add(ss);
//		}
//		for(int i =0; i < 15; i++)
//		{
//			StarSystem ss = new StarSystem();
//			ss._id = "B"+(i+1);
//			ss._control = Fraction.BORGUZANDS;
//			ss.pos = Random.insideUnitSphere*rndDist + new Vector3(0, rndDist*1.5f, 0);
//			ss._nearbyStarSystems = new List<string>();
//			
//			_starSystems.Add(ss);
//		}
//		for(int i =0; i < 3; i++)
//		{
//			StarSystem ss = new StarSystem();
//			ss._id = "N"+(i+1);
//			ss._control = Fraction.NEUTRAL;
//			ss.pos = Random.insideUnitSphere*(rndDist/2) + new Vector3(0, rndDist/2, 0);
//			ss._nearbyStarSystems = new List<string>();
//			
//			_starSystems.Add(ss);
//		}


        _starSystems.ForEach((s) => {
            GameObject star = (GameObject)Instantiate(Resources.Load("Prefabs/map/Star") as GameObject);
            star.name = s._id;
            star.transform.position = s.pos;
            star.transform.parent = transform;
            star.GetComponent<Renderer>().material = GetMaterialStarSystem(star.gameObject.name, s._control);

            s.SetGO(star);
        });


		
//		_starSystems.Sort((s1, s2)=>{
//			return (Vector3.Distance(s1.pos, Vector3.zero) > Vector3.Distance(s2.pos, Vector3.zero)) ? 1 : -1;
//		});
//
//
//        _starSystems.ForEach((s1) =>
//        {
//
//            //if (s1._nearbyStarSystems.Count < 2) // && s1._control != Fraction.NEUTRAL)
//            {
//                //StarSystem _tempStar = new StarSystem();
//				for(int i=0; i<2; i++)
//				{
//					float minDist = 25;
//					StarSystem tempS1 = null;
//					StarSystem tempS2 = null;
//
//	                _starSystems.ForEach((s2) =>
//	                {
//						if (s1._id != s2._id && !s2.NearbyStarExist(s1._id) && s2._nearbyStarSystems.Count < 3 && (s1._nearbyStarSystems.Count < 2 ||  (Vector3.Distance(s1.pos, Vector3.zero) < 10)))
//	                    {
//	                        float dist = Vector3.Distance(s1.pos, s2.pos);
//	                        //Debug.Log(dist);
//	                        if (minDist > dist)
//	                        {
//	                            minDist = dist;
//								tempS1 = s1;
//								tempS2 = s2;
//	                        }
//	                    }
//	                });
//					if(tempS1 != null)
//					{					
//						tempS1._nearbyStarSystems.Add(tempS2._id);
//						tempS2._nearbyStarSystems.Add(tempS1._id);
//					}
//				}
//            }
//        });

        _starSystems.ForEach((s) => {
            s._nearbyStarSystems.ForEach((n) => {
                StarSystem sn = _starSystems.Find((st) => { return n == st._id; });
                //if(s._control == sn._control && s._control != Fraction.NEUTRAL)
                {
                    GameObject line = new GameObject();
                    line.transform.parent = s.GetGO().transform;
                    line.name = "line";
                    line.transform.localPosition = Vector3.zero;
                    line.layer = s.GetGO().layer;
                    line.gameObject.AddComponent<LineRenderer>();
                    line.GetComponent<LineRenderer>().material = GetMaterialStarSystem(s._id, (s._control == sn._control) ? s._control : Fraction.NEUTRAL);
                    line.GetComponent<LineRenderer>().SetWidth(0.1f, 0.1f);
                    line.GetComponent<LineRenderer>().SetPosition(0, s.pos);
                    line.GetComponent<LineRenderer>().SetPosition(1, sn.pos);
                }
            });
        });

        //_starSystems.ForEach((s) =>
        //{
        //    if (s._control == Fraction.BORGUZANDS)
        //    {
        //        Debug.Log(s._id);
        //        s._nearbyStarSystems.ForEach((n) =>
        //        {
        //            _starSystems.Find((st) => { return n == st._id; })._control = s._control;
        //        });
        //    }
        //});

    }

    public void SaveToXml()
    {
        foreach(Transform star in transform)
        {
            List<SpaceZone> _sZone = new List<SpaceZone>();
            
            int index = 0;
            foreach(Transform zone in star)
            {
                _sZone.Add(new SpaceZone{ _zoneId = (star.gameObject.name+"_"+index), _zoneDesc = "habitable zone in the system " + star.gameObject.name, pos = zone.position});
                index++;
            }
            //Fraction frac = Utils.RandomEnum<Fraction>();
            Fraction frac = Fraction.NEUTRAL;
            star.GetComponent<Renderer>().material = GetMaterialStarSystem(star.gameObject.name, frac);
//            _starSystems.Add(new StarSystem { _id = star.gameObject.name, _control = frac, _spaceZone = _sZone, pos = star.position, _nearbyStarSystems = new List<string>()});
        }

        _starSystems.ForEach((s1) => {
            StarSystem _tempStar =new StarSystem();
            float minDist = 10000;
            _starSystems.ForEach((s2) =>{
                if (s1._id != s2._id && !s2.NearbyStarExist(s1._id))
                {
                    float dist = Vector3.Distance(s1.pos, s2.pos);
                    if (minDist > dist)
                    {
                        minDist = dist;
                        _tempStar = s2;
                    }
                }
            });
            //Debug.Log(s1._id +" -> " + _tempStar._id);
            s1._nearbyStarSystems.Add(_tempStar._id);
            _starSystems.Find((ts) => { return (ts._id == _tempStar._id); })._nearbyStarSystems.Add(s1._id);
        });

//        _starSystems.ForEach((s1) =>
//        {
//            //if (s1._nearbyStarSystems.Count < 2) // && s1._control != Fraction.NEUTRAL)
//            {
//                //StarSystem _tempStar = new StarSystem();
//                float minDist = 10;
//                _starSystems.ForEach((s2) =>
//                {
//                    if (s1._id != s2._id && !s2.NearbyStarExist(s1._id))
//                    {
//                        if (s1._nearbyStarSystems.Count < 3 && s2._nearbyStarSystems.Count < 4)
//                        {
//                            float dist = Vector3.Distance(s1.pos, s2.pos);
//                            //Debug.Log(dist);
//                            if (minDist > dist)
//                            {
//                                //minDist = dist;
//                                //_tempStar = s2;
//
//                                s1._nearbyStarSystems.Add(s2._id);
//                                s2._nearbyStarSystems.Add(s1._id);
//                            }
//                        }
//                    }
//                });
//                //Debug.Log(s1._id +" -> " + _tempStar._id);
//                //s1._nearbyStarSystems.Add(_tempStar._id);
//                //_starSystems.Find((ts) => { return (ts._id == _tempStar._id); })._nearbyStarSystems.Add(s1._id);
//            }
//        });

        _starSystems.ForEach((s) =>
        {
            foreach (Transform star in transform)
            {
                if (star.gameObject.name == s._id)
                {
                    s._nearbyStarSystems.ForEach((n) =>
                    {
                        GameObject line = new GameObject();
                        line.transform.parent = star.gameObject.transform;
                        line.name = "line";
                        line.transform.localPosition = Vector3.zero;
                        line.layer = star.gameObject.layer;
                        line.gameObject.AddComponent<LineRenderer>();
                        line.GetComponent<LineRenderer>().SetWidth(0.01f, 0.01f);
                        line.GetComponent<LineRenderer>().SetPosition(0, s.pos);
                        line.GetComponent<LineRenderer>().SetPosition(1, _starSystems.Find((st) => { return n == st._id; }).pos);
                    });
                }
            }
        });


        SaveXml(_starSystems, pathXML);
    }

	private Texture2D tex = null;

    private Material GetMaterialStarSystem(string id, Fraction frac)
    {
		if(tex == null)
		{
            tex = Resources.Load("Textures/StarSystem_1") as Texture2D;
		}
        Material mat;
        mat = new Material(Shader.Find("Particles/Additive"));
		mat.mainTexture = tex;
        switch(frac)
        {
            case Fraction.BORGUZANDS:
                if (id == "NGC-159")
                {
                    mat.SetColor("_TintColor", Color.red);
                    mat.SetFloat("_Value", 0.7f);
                }
                else
                {
					mat.SetColor("_TintColor", new Color(1f, 0.2f, 0.2f, 0.5f));
                }
                break;
            case Fraction.HUMANS:
                if(id == "NGC-99")
                {
					mat.SetColor("_TintColor", Color.green);
                    mat.SetFloat("_Value", 0.7f);
                }
                else
                {
					mat.SetColor("_TintColor", new Color(0.3f, 1f, 0.3f, 0.5f));
                }
                break;
            case Fraction.KRIPTIZIDS:
                if (id == "NGC-17")
                {
					mat.SetColor("_TintColor", Color.blue);
                    mat.SetFloat("_Value", 0.7f);
                }
                else
                {
					mat.SetColor("_TintColor", new Color(0.2f, 0.2f, 1f, 0.5f));
                }
                break;
            case Fraction.NEUTRAL:
				mat.SetColor("_TintColor", Color.grey);
                break;
        }
        return mat;
    }

    private string pathXML = @"Texts/Map";

    public T LoadXml<T>(string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        TextAsset ta = (TextAsset)Resources.Load(path) as TextAsset;
        Dbg.Print(path);
        TextReader reader = new StringReader(ta.text);

        return (T)serializer.Deserialize(reader);

    }

    public void SaveXml(object obj, string path)
    {
        string filepath = Directory.GetCurrentDirectory() + @"/Assets/Resources/"+path+".xml";

        var serializer = new XmlSerializer(obj.GetType());

        using (var stream = new FileStream(filepath, FileMode.Create))
        {
            serializer.Serialize(stream, obj);
        }
    }

}
