using UnityEngine;
using System.Collections;
using Common;
using System.Collections.Generic;

public class ShipModel : MonoBehaviour {

	private ArrayList m_shipModules							 = new ArrayList();
	private ArrayList m_shipModulesWithFireSlot				 = new ArrayList();

    private string newWorkshop = "DT";
    private bool hungar = false;
    private bool engineParticles = true;

    private Dictionary<ShipModelSlotType, string> _modulesID = new Dictionary<ShipModelSlotType, string> { 
    { ShipModelSlotType.CB, "Prefabs/Ships/Modules/H_DT0001_CB"},
    { ShipModelSlotType.DF, "Prefabs/Ships/Modules/H_DT0002_DF"},
    { ShipModelSlotType.DM, "Prefabs/Ships/Modules/H_DT0002_DM"},
    { ShipModelSlotType.CM, "Prefabs/Ships/Modules/H_DT0001_CM"},
    { ShipModelSlotType.ES, "Prefabs/Ships/Modules/H_DT0002_ES"} };
	

    // TEST METHOD!!! load first workshop and first ship modules group
    public static GameObject Init(bool isPlayer)
    {
        GameObject _ship = new GameObject("ship_model");
        ShipModel _shipModel = _ship.AddComponent<ShipModel>();
        //Rigidbody _rigidBody = _ship.AddComponent<Rigidbody>();
        SphereCollider _collider = _ship.AddComponent<SphereCollider>();
        //_collider.isTrigger = true;
        
        //_rigidBody.useGravity = false;
        //_rigidBody.mass = 500;
        //_rigidBody.isKinematic = true;
        _shipModel.AddModules();
        _shipModel.UpdateModulesPos();

        if(isPlayer)
            _ship.SetLayerRecursively(LayerMask.NameToLayer("Player"));

        //_shipModel.transform.localScale = Vector3.one * 8;
        GameObject temp = _ship;
		Destroy(_ship);
		temp.AddComponent<EnginParticle>();
        return temp;
    }
    //---------------------------------------------

    public static GameObject Init(Dictionary<ShipModelSlotType, string> modulesID, bool isPlayer, bool engineParticles = true)
    {
        GameObject _ship = new GameObject("ship_model");
        ShipModel _shipModel = _ship.AddComponent<ShipModel>();
        _shipModel.engineParticles = engineParticles;
        //Rigidbody _rigidBody = _ship.AddComponent<Rigidbody>();
        SphereCollider _collider = _ship.AddComponent<SphereCollider>();

        //_collider.isTrigger = true;
        //_ship.layer = LayerMask.NameToLayer("Player");
        //_rigidBody.useGravity = false;
       // _rigidBody.mass = 500;
        //_rigidBody.isKinematic = true;
        //_shipModel.transform.localScale = Vector3.one * 8;
        _shipModel.SetModulesId(modulesID);
        _shipModel.AddModules();
        _shipModel.UpdateModulesPos();

        if(isPlayer)
            _ship.SetLayerRecursively(LayerMask.NameToLayer("Player"));

		_ship.AddComponent<EnginParticle>();
                
        if (isPlayer)
        {
            _ship.AddComponent<SolarMapObject>().objectType = SolarMapObjectType.my_ship;
        }
        return _ship;
    }

    public void SetModulesId(Dictionary<ShipModelSlotType, string> modulesID)
    {
        _modulesID = modulesID;
    }

    public void AddModules()
    {
        hungar = (Application.loadedLevelName == "Angar" || Application.loadedLevelName == "DemoAngar" || Application.loadedLevelName == "select_character");
        GameObject _ship = new GameObject("ship");
        _ship.transform.parent = transform;
        CreateModule(ShipModelSlotType.CB, _ship.transform, 0);
        CreateModule(ShipModelSlotType.DF, _ship.transform, 1);
        CreateModule(ShipModelSlotType.DM, _ship.transform, 2);
        CreateModule(ShipModelSlotType.CM, _ship.transform, 3);
        CreateModule(ShipModelSlotType.ES, _ship.transform, 4);

        if (!hungar)
        {
            GameObject engineSound = Instantiate(Resources.Load("Prefabs/Ships/EngineSound") as GameObject) as GameObject;
            engineSound.transform.parent = _ship.transform;
        }

        _ship.transform.localPosition = Vector3.zero;
        _ship.transform.localRotation = Quaternion.identity;
    }

    public void SetWorkShop(string workshop)
    {
        newWorkshop = workshop;
        UpdateModel();
    }
    public void UpdateModel()
    {
        GameObject ship = transform.Find("ship").gameObject;
        if (ship != null)
        {
            Destroy(ship);
        }
        AddModules();
        UpdateModulesPos();
    }

    private void CreateModule(ShipModelSlotType type, Transform parent, int index)
    {
        Debug.Log("_modulesID[type] = " + _modulesID[type]+ "indx "+index);
        string t = _modulesID[type];
        t = t.Replace("DT", newWorkshop);
		if (!hungar)
		{
	        switch(newWorkshop)
	        { 
	            case "DT":
	                GetComponent<SphereCollider>().radius = 3;
	                break;
	            case "RE":
	                GetComponent<SphereCollider>().radius = 3;
	                break;
	            case "EQ":
	                GetComponent<SphereCollider>().radius = 3;
	                break;
	        }
		}
        //Debug.LogFormat("try instantiate: {0}", t);
        GameObject go = Instantiate(Resources.Load(t) as GameObject) as GameObject;
        go.transform.parent = parent;
        if (hungar || !engineParticles)
        {
            foreach (ParticleSystem particle in go.GetComponentsInChildren<ParticleSystem>())
            {
                Destroy(particle.gameObject);
            }
        }
        if (hungar || !engineParticles)
        {
            foreach (EngineRotate engeRot in go.GetComponentsInChildren<EngineRotate>())
            {
                Destroy(engeRot);
            }
        }
        go.GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<PlayerModuleType>().SetSlotType(type);

		ShipModule mod = new ShipModule(go);
		m_shipModules.Insert(index,mod);

		if (mod.GetFireSlots().Count != 0)
			m_shipModulesWithFireSlot.Add(mod);


    }

	public ShipModule 	GetModule(int indx)
	{
		Debug.Log("Get module "+indx);
		if (indx >= 0 && indx < m_shipModules.Count)
			return (ShipModule)m_shipModules[indx];
		else return null;
	}
	public ShipModule 	GetModuleFireSlot(int indx)
	{
		Debug.Log("Get module "+indx);
		if (indx >= 0 && indx < m_shipModulesWithFireSlot.Count)
			return (ShipModule)m_shipModulesWithFireSlot[indx];
		else return null;
	}
	public ArrayList	 GetModules(){ return m_shipModules; }
	public ArrayList	 GetModulesFireSlot(){return m_shipModulesWithFireSlot;}

    public void UpdateModulesPos()
    {
		for (int i = 1; i < m_shipModules.Count; i++)
        {
            string _soketName = "soket_" + (i).ToString() + "_" + (i + 1).ToString();
			Transform _soket1 = GetModule(i).GetModel().transform.Find(_soketName);
			Transform _soket2 = GetModule(i - 1).GetModel().transform.Find(_soketName);
            Vector3 delta = _soket2.position - _soket1.position;
			GetModule(i).GetModel().transform.localPosition = delta;
        }

		if (m_shipModules.Count>2)
		{
			Vector3 offset = GetModule(2).GetModel().transform.localPosition;

			for (int i = 0; i < m_shipModules.Count; i++)
			{
				GetModule(i).GetModel().transform.localPosition -= offset;
			}
		}

    }

}
