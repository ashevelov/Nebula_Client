using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Game.Space;
using Nebula.Resources;

public class SelectCharacterShip : Singleton<HangarShip>
{

    private float speed = 20;

    private Dictionary<ShipModelSlotType, string> slots;
    private bool started = true;

    void Start()
    {
        if (MmoEngine.Get.SelectCharacterGame.PlayerCharacters.HasSelectedCharacter()) {
            slots = MmoEngine.Get.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Model;
            RebuildModel();
        }

    }

    private void RebuildModel()
    {

        Dictionary<ShipModelSlotType, string> s = new Dictionary<ShipModelSlotType, string>();

        if(false == MmoEngine.Get.SelectCharacterGame.PlayerCharacters.HasSelectedCharacter()) {
            return;
        }
        foreach (var m in MmoEngine.Get.SelectCharacterGame.PlayerCharacters.SelectedCharacter().Model) {
            if (string.IsNullOrEmpty(m.Value)) {
                Debug.LogError("null for {0}".f(m.Key));
                continue;
            }
            try {
                s.Add(m.Key, DataResources.Instance.ModuleData(m.Value).Model);
            } catch (System.Exception e) {
                Debug.Log(e.StackTrace);
            }
        }

        for (int i = 0; i < this.transform.childCount; i++) {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        var obj = ShipModel.Init(s, false);
        obj.transform.parent = this.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        foreach (var pmt in obj.GetComponentsInChildren<PlayerModuleType>()) {
            pmt.gameObject.AddComponent<StationShipModule>().slotType = pmt.SlotType;
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

            if(false == MmoEngine.Get.SelectCharacterGame.PlayerCharacters.HasSelectedCharacter()) {
                return;
            }

            var character = MmoEngine.Get.SelectCharacterGame.PlayerCharacters.SelectedCharacter();

            if (slots == null) {
                slots = character.Model;
                this.RebuildModel();
            } else {
                if (false == this.CompareSlots(character.Model)) {
                    slots = character.Model;
                    this.RebuildModel();
                }
            }
        }
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