/*
using UnityEngine;
using System.Collections;
using Game.Space.Resources;
using Game.Space;
using System.Collections.Generic;
using Common;

public class PlayerEvents{

    private Dictionary<string, Dictionary<string, EventInfo>> _events;

    public PlayerEvents() {
        _events = new Dictionary<string, Dictionary<string, EventInfo>>();
    }

    public void OnEventReceived(Hashtable info) {
        UpdateEvent(info);
    }

    public void OnEventStarted(Hashtable info) {
        UpdateEvent(info);
    }

    public void OnEventCompleted(Hashtable info) { 
        string eventId = (string)info[GenericEventProps.WorldEventId];
        string worldId = (string)info[GenericEventProps.WorldId];
        RemoveEvent(worldId, eventId);
        TempQuestWindow.Init(false);
    }

    public void OnEventRewardReceived(Hashtable info) {
        string eventId = (string)info[GenericEventProps.WorldEventId];
        string worldId = (string)info[GenericEventProps.WorldId];
        RemoveEvent(worldId, eventId); 
    }

    private void UpdateEvent(Hashtable info) {
        string eventId = (string)info[GenericEventProps.WorldEventId];
        string worldId = (string)info[GenericEventProps.WorldId];
        bool active = (bool)info[GenericEventProps.WorldEventActive];
        ReplaceEvent(worldId, eventId, active, info);
    }

    public bool TryGetEvent(string worldId, string eventId, out EventInfo result) {
        result = null;
        if (_events.ContainsKey(worldId)) {
            return _events[worldId].TryGetValue(eventId, out result);
        }
        return false;
    }

    public void ReplaceEvent(string worldId, string eventId, bool active, Hashtable info) {
        EventInfo evt = null;
        if (TryGetEvent(worldId, eventId, out evt))
        {
            _events[worldId].Remove(eventId);
        }


        EventInfo newInfo = new EventInfo { active = active, evt = eventId, info = info, world = worldId };
        if (_events.ContainsKey(worldId))
            _events[worldId].Add(eventId, newInfo);
        else
            _events.Add(worldId, new Dictionary<string, EventInfo> { { eventId, newInfo } });
    }

    public void RemoveEvent(string world, string evt) {
        if (_events.ContainsKey(world))
            _events[world].Remove(evt);
    }

    public Dictionary<string, Dictionary<string, EventInfo>> Events {
        get {
            return _events;
        }
    }
}


public class EventInfo {

    public string world;
    public string evt;
    public bool active;
    public Hashtable info;

    private WorldEventData _cachedData;

    public WorldEventData GetData() {
        if (_cachedData == null)
            _cachedData = XmlLoader.WorldEventCache.GetWorld(world).GetEvent(evt);
        return _cachedData;
    }

    public string StatusString {
        get {
            if (info != null) {
                int currentCount = (int)info[GenericEventProps.WorldEventCurrentCount];
                int totalCount = (int)info[GenericEventProps.WorldEventTotalCount];
                return string.Format("Killed: {0}/{1}, Status: {2}",
                    currentCount, totalCount, active ? "Active" : "Not active");
            }
            return string.Empty;
        }
    }
}
*/