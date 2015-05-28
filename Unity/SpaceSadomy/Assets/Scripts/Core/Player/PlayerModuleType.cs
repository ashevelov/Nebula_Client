using UnityEngine;
using System.Collections;
using Common;

public class PlayerModuleType : MonoBehaviour {

    private ShipModelSlotType slotType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public ShipModelSlotType SlotType
    {
        get
        {
            return this.slotType;
        }
    }

    public void SetSlotType(ShipModelSlotType sType)
    {
        this.slotType = sType;
    }
}
