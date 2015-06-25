/*
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using Game.Network;

[Obsolete("Don't use that class")]
public class NetworkEventController : Game.Space.Singleton<NetworkEventController> {

	private Hashtable clientHashtable = new Hashtable();
	private Hashtable serverHashtable = new Hashtable();

    private Dictionary<string, Hashtable> _serverPropertiesGroups = new Dictionary<string, Hashtable>();
    private Dictionary<string, Hashtable> _clientPropertieGroups = new Dictionary<string, Hashtable>();

    #region Property Groups interface
    public Dictionary<string, Hashtable> GetServerPropertyGroups
    {
        get { return _serverPropertiesGroups; }
    }

    public void ClearServerPropertyGroups() {
        _serverPropertiesGroups.Clear();
    }
    public void AddServerPropertyGroup(string groupName, Hashtable settedProperties)
    {
        if (_serverPropertiesGroups.ContainsKey(groupName))
            _serverPropertiesGroups.Remove(groupName);
        _serverPropertiesGroups.Add(groupName, settedProperties);
    }

    public void AddServerPropertyGroupPack(Dictionary<string, Hashtable> settedGroups)
    {
        foreach (var de in settedGroups)
        {
            if (_serverPropertiesGroups.ContainsKey(de.Key))
                _serverPropertiesGroups.Remove(de.Key);
            _serverPropertiesGroups.Add(de.Key, de.Value);
        }
    }

    public void AddClientPropertyGroup(string groupName, Hashtable settedProperties)
    {
        if (_clientPropertieGroups.ContainsKey(groupName))
            _clientPropertieGroups.Remove(groupName);
        _clientPropertieGroups.Add(groupName, settedProperties);
    }
    public void AddClientPropertyGroupPack(Dictionary<string, Hashtable> settedGroups)
    {
        foreach (var ke in settedGroups)
        {
            if (_clientPropertieGroups.ContainsKey(ke.Key))
                _clientPropertieGroups.Remove(ke.Key);
            _clientPropertieGroups.Add(ke.Key, ke.Value);
        }
    }

    private void UpdateClientPropertyGroups()
    {
        if (_clientPropertieGroups.Count > 0)
        {
            //process here received properties groups
        }
        _clientPropertieGroups.Clear();
    }
    #endregion


    #region Server

    public Hashtable GetServerHashtable()
	{
		return serverHashtable;
	}

    public void ClearServerHashtable() {
        serverHashtable.Clear();
    }

	public bool AddServerEvent(DictionaryEntry entry)
	{
		try
		{
            if (serverHashtable.ContainsKey(entry.Key.ToString()))
                serverHashtable.Remove(entry.Key.ToString());

			serverHashtable.Add(entry.Key, entry.Value);
			return true;
		}
		catch(System.Exception e)
		{
			Debug.Log("add server event error : " + e.ToString());
			return false;
		}
	}

	public bool AddServerEventsPack(Hashtable hashtable)
	{
		try
		{
			foreach(DictionaryEntry entry in hashtable)
			{
                if (serverHashtable.ContainsKey(entry.Key.ToString()))
                    serverHashtable.Remove(entry.Key.ToString());

				serverHashtable.Add(entry.Key, entry.Value);
			}
			return true;
		}
		catch(System.Exception e)
		{
			Debug.Log("add server events pack error : " + e.ToString());
			return false;
		}
	}
#endregion

#region Client
	public bool AddClientEvent(DictionaryEntry entry)
	{
		try
		{
            if (clientHashtable.ContainsKey(entry.Key.ToString()))
                clientHashtable.Remove(entry.Key.ToString());

			clientHashtable.Add(entry.Key, entry.Value);
			return true;
		}
		catch(System.Exception e)
		{
			Debug.Log("add client event error : " + e.ToString());
			return false;
		}
	}

	public bool AddClientEventsPack(Hashtable hashtable)
	{
		try
		{
			foreach(DictionaryEntry entry in hashtable)
			{
                if (clientHashtable.ContainsKey(entry.Key.ToString()))
                    clientHashtable.Remove(entry.Key.ToString());

				clientHashtable.Add(entry.Key, entry.Value);
			}
			return true;
		}
		catch(System.Exception e)
		{
			Debug.Log("add client events pack error : " + e.ToString());
			return false;
		}
	}

	
	private ClientEvents clientEvents = new ClientEvents();

	private void RunClientEvents()
	{
		foreach(DictionaryEntry entry in clientHashtable)
		{

			clientEvents.StartEvent(entry.Key.ToString(), entry.Value);
//			try {
//				Binder defaultBinder = Type.DefaultBinder;
//				ClientEvents myClass = new ClientEvents ();
//				clientEvents.GetType ().InvokeMember (entry.Key.ToString(), BindingFlags.InvokeMethod,
//				                                      defaultBinder, clientEvents, new object[] {entry.Value});
//			} catch (System.Exception ex) {
//				Debug.Log (ex.ToString ());
//			}
		}
		clientHashtable.Clear();
	}


#endregion

	void Update()
	{
		RunClientEvents();
        UpdateClientPropertyGroups();
	}

	
	private bool create = false;
	
	void Start()
	{
		if(!create){
			DontDestroyOnLoad(gameObject);
		}else
		{
			Destroy(gameObject);
		}
	}



	#region Test

//	public bool visibleTest = false;
//
//	void OnGUI()
//	{
//		if(visibleTest)
//		{
//			if(GUI.Button(new Rect(0,40,200,30), "add update gun event"))
//			{
//				AddClientEvent(new DictionaryEntry("ship_update_gun", 14.5f));
//			}
//			if(GUI.Button(new Rect(0, 70,200,30), "add update gun ditrist event"))
//			{
//				AddClientEvent(new DictionaryEntry("ship_update_gun_ditrist", 53.0f));
//			}
//		}
//	}

	#endregion


}
*/
