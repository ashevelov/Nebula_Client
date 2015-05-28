using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Space;

public class SelectCharacterShip : Singleton<HangarShip>
{

    private float speed = 20;

    private Dictionary<ShipModelSlotType, string> slots;
    private bool started = true;

    void Start()
    {
        if (G.Game.UserInfo.HasSelectedCharacter())
        {
            slots = G.Game.UserInfo.GetSelectedCharacter().Model;
            this.RebuildModel();
        }
    }

    private void RebuildModel()
    {

        Dictionary<ShipModelSlotType, string> s = new Dictionary<ShipModelSlotType, string>();

        if (G.Game.UserInfo.GetSelectedCharacter() != null)
        {
            foreach (var m in G.Game.UserInfo.GetSelectedCharacter().Model)
            {
                if (string.IsNullOrEmpty(m.Value))
                {
                    Debug.LogError("null for {0}".f(m.Key));
                    continue;
                }
                try
                {
                    s.Add(m.Key, DataResources.Instance.ModuleData(m.Value).Model);
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            }

            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }

            var obj = ShipModel.Init(s, false);
            obj.transform.parent = this.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            foreach (var pmt in obj.GetComponentsInChildren<PlayerModuleType>())
            {
                pmt.gameObject.AddComponent<StationShipModule>().slotType = pmt.SlotType;
            }
        }
    }


    private Vector3 oldPos;

    void Update()
    {
        if (this.started)
        {
            if (Input.GetMouseButtonDown(0)) {
                oldPos = Input.mousePosition;
            } else if (Input.GetMouseButton(0)) {
                Vector3 delta = Input.mousePosition - oldPos;
                transform.eulerAngles -= new Vector3(0, delta.x * Time.deltaTime * speed, 0);
                oldPos = Input.mousePosition;
            }

            //if (G.Game.UserInfo.HasSelectedCharacter())
            //{
            //    foreach (var m in G.Game.UserInfo.GetSelectedCharacter().Model)
            //    {

            //        //Dbg.Print("{0}:{1}".f(m.Key, m.Value), "USER");
            //    }
            //}

            if (G.Game.UserInfo.HasSelectedCharacter())
            {
                if (slots == null)
                {
                    slots = G.Game.UserInfo.GetSelectedCharacter().Model;
                    this.RebuildModel();
                }
                else
                {
                    if (false == this.CompareSlots(G.Game.UserInfo.GetSelectedCharacter().Model))
                    {
                        slots = G.Game.UserInfo.GetSelectedCharacter().Model;
                        this.RebuildModel();
                    }
                }
            }
        }
        //G.Game.Ship.ShipModel.
    }

    bool CompareSlots(Dictionary<ShipModelSlotType, string> newSlots)
    {
        if (this.slots.Count != 5)
            return false;
        if (newSlots.Count != 5)
            return false;
        foreach (var p in newSlots)
        {
            if (this.slots.ContainsKey(p.Key) == false)
                return false;
            if (this.slots[p.Key] != p.Value)
                return false;
        }
        return true;
    }

    public void Stop()
    {
        this.started = false;
    }
}