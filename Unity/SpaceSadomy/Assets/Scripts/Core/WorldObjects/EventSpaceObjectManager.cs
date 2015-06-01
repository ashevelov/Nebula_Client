// EventSpaceObjectManager.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Tuesday, December 2, 2014 5:09:50 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//

using Nebula;
using Nebula.Client;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handle creation/destruction event objects in world( each object corresponds active event in zone )
/// </summary>
public class EventSpaceObjectManager : MonoBehaviour 
{
    private Dictionary<string, EventSpaceObject> eventObjects = new Dictionary<string, EventSpaceObject>();
    private List<ClientWorldEventInfo> eventsToAdd = new List<ClientWorldEventInfo>();
    private List<EventSpaceObject> deleteObjects = new List<EventSpaceObject>();

    private float updateInterval = 1f; //update every second
    private float updateTimer;

    private GameObject cachedPrefab;

    void Start()
    {
        this.cachedPrefab = PrefabCache.Get("Prefabs/EventSpaceObject");
        this.updateTimer = this.updateInterval;
    }

    void Update()
    {
        this.updateTimer -= Time.deltaTime;

        if (this.updateTimer <= 0f)
        {
            string worldId = G.Game.Engine.GameData.World.Name;

            eventsToAdd.Clear();
            deleteObjects.Clear();

            Dictionary<string, ClientWorldEventInfo> worldEvents = G.Game.WorldEventConnection.EventsForWorld(worldId);

            //find event for which need create object
            foreach (var eventPair in worldEvents)
            {
                if (eventPair.Value.Active)
                {
                    if (!this.eventObjects.ContainsKey(eventPair.Key))
                    {
                        eventsToAdd.Add(eventPair.Value);
                    }
                }
            }

            //find objects for which don't exist event (for removing)
            foreach (var eventObjPair in this.eventObjects)
            {
                if ((!worldEvents.ContainsKey(eventObjPair.Key)) || (!worldEvents[eventObjPair.Key].Active))
                {
                    deleteObjects.Add(eventObjPair.Value);
                }
            }

            //fsrt delete not existing or not active event objects
            foreach (var dObj in deleteObjects)
            {
                this.eventObjects.Remove(dObj.EventInfo.Id);
                Destroy(dObj.gameObject);
            }

            //second add objects for new or active events
            foreach (var evt in eventsToAdd)
            {
                GameObject eObj = (GameObject)Instantiate(cachedPrefab, evt.Position.toVector(), Quaternion.identity);
                this.eventObjects.Add(evt.Id, eObj.GetComponent<EventSpaceObject>());
                eObj.GetComponent<EventSpaceObject>().SetWorldEvent(evt);
            }

            this.eventsToAdd.Clear();
            this.deleteObjects.Clear();

            this.updateTimer = this.updateInterval;
        }
    }
}
