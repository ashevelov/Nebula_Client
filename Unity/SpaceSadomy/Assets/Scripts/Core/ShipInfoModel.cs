using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Space;
using Nebula;

public class ShipInfoModel : MonoBehaviour
{

    private float speed = 20;

    private Dictionary<ShipModelSlotType, string> slots;
    private bool started = true;

    void Start()
    {
        this.slots = GameData.instance.ship.ShipModel.SlotPrefabs();
        this.RebuildModel();
    }

    private void RebuildModel()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        var obj = ShipModel.Init(this.slots, false);
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        obj.SetLayerRecursively(LayerMask.NameToLayer("ship_image"));

        foreach (var pmt in obj.GetComponentsInChildren<PlayerModuleType>())
        {
            pmt.gameObject.AddComponent<StationShipModule>().slotType = pmt.SlotType;
        }
    }

    private Vector3 oldPos;

    void Update()
    {
        if (this.started)
        {
            if (false)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    oldPos = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 delta = Input.mousePosition - oldPos;
                    transform.eulerAngles -= new Vector3(0, delta.x * Time.deltaTime * speed, 0);
                    oldPos = Input.mousePosition;
                }
            }

            if (false == this.CompareSlots(GameData.instance.ship.ShipModel.SlotPrefabs()))
            {
                this.slots = GameData.instance.ship.ShipModel.SlotPrefabs();
                this.RebuildModel();
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
