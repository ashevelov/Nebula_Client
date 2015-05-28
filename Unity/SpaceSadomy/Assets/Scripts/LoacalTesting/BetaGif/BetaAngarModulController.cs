using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

public class BetaAngarModulController : MonoBehaviour {

	public List<BetaAngarModul> modules = new List<BetaAngarModul>();

	private Dictionary<ShipModelSlotType, string> slots = new Dictionary<ShipModelSlotType, string> { 
		{ ShipModelSlotType.CB, "Prefabs/Ships/Modules/H_DT0001_CB"},
		{ ShipModelSlotType.DF, "Prefabs/Ships/Modules/H_DT0001_DF"},
		{ ShipModelSlotType.DM, "Prefabs/Ships/Modules/H_DT0001_DM"},
		{ ShipModelSlotType.CM, "Prefabs/Ships/Modules/H_DT0001_CM"},
		{ ShipModelSlotType.ES, "Prefabs/Ships/Modules/H_DT0001_ES"} };

    public void SwitchModule(ShipModelSlotType type)
    {
        modules.ForEach((m) =>
        {
            if (type == m.slotType)
            {
                m.gameObject.SetActive(!m.gameObject.activeSelf);
            }
        });
    }

	void Start()
	{
		RebuildModel(slots);
	}

	private void RebuildModel(Dictionary<ShipModelSlotType, string> rSlots)
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}
		
		var obj = ShipModel.Init(rSlots, false);
		obj.transform.parent = this.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;
		
		foreach (var pmt in obj.GetComponentsInChildren<PlayerModuleType>())
		{
			pmt.gameObject.AddComponent<StationShipModule>().slotType = pmt.SlotType;
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
//			SwitchModule(ShipModelSlotType.ES);
			slots[ShipModelSlotType.ES] = slots[ShipModelSlotType.ES].Replace( "1", "3" );
			slots[ShipModelSlotType.ES] = slots[ShipModelSlotType.ES].Replace( "2", "1" );
			slots[ShipModelSlotType.ES] = slots[ShipModelSlotType.ES].Replace( "3", "2" );
			RebuildModel(slots);
		}
		if(Input.GetKeyDown(KeyCode.W))
		{
//			SwitchModule(ShipModelSlotType.CM);
			slots[ShipModelSlotType.CM] = slots[ShipModelSlotType.CM].Replace( "1", "3" );
			slots[ShipModelSlotType.CM] = slots[ShipModelSlotType.CM].Replace( "2", "1" );
			slots[ShipModelSlotType.CM] = slots[ShipModelSlotType.CM].Replace( "3", "2" );
			RebuildModel(slots);
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
//			SwitchModule(ShipModelSlotType.DM);
			slots[ShipModelSlotType.DM] = slots[ShipModelSlotType.DM].Replace( "1", "3" );
			slots[ShipModelSlotType.DM] = slots[ShipModelSlotType.DM].Replace( "2", "1" );
			slots[ShipModelSlotType.DM] = slots[ShipModelSlotType.DM].Replace( "3", "2" );
			RebuildModel(slots);
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
//			SwitchModule(ShipModelSlotType.DF);
			slots[ShipModelSlotType.DF] = slots[ShipModelSlotType.DF].Replace( "1", "3" );
			slots[ShipModelSlotType.DF] = slots[ShipModelSlotType.DF].Replace( "2", "1" );
			slots[ShipModelSlotType.DF] = slots[ShipModelSlotType.DF].Replace( "3", "2" );
			RebuildModel(slots);
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
//			SwitchModule(ShipModelSlotType.CB);
			slots[ShipModelSlotType.CB] = slots[ShipModelSlotType.CB].Replace( "1", "3" );
			slots[ShipModelSlotType.CB] = slots[ShipModelSlotType.CB].Replace( "2", "1" );
			slots[ShipModelSlotType.CB] = slots[ShipModelSlotType.CB].Replace( "3", "2" );
			RebuildModel(slots);
		}
		if(Input.GetKeyDown(KeyCode.Y))
		{
			RebuildModel(new Dictionary<ShipModelSlotType, string> { 
				{ ShipModelSlotType.CB, "Prefabs/Ships/Modules/H_RE0001_CB"},
				{ ShipModelSlotType.DF, "Prefabs/Ships/Modules/H_RE0001_DF"},
				{ ShipModelSlotType.DM, "Prefabs/Ships/Modules/H_RE0001_DM"},
				{ ShipModelSlotType.CM, "Prefabs/Ships/Modules/H_RE0001_CM"},
				{ ShipModelSlotType.ES, "Prefabs/Ships/Modules/H_RE0001_ES"} });
			//SwitchModule(ShipModelSlotType.CB);
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			RebuildModel(new Dictionary<ShipModelSlotType, string> { 
				{ ShipModelSlotType.CB, "Prefabs/Ships/Modules/H_EQ0001_CB"},
				{ ShipModelSlotType.DF, "Prefabs/Ships/Modules/H_EQ0001_DF"},
				{ ShipModelSlotType.DM, "Prefabs/Ships/Modules/H_EQ0001_DM"},
				{ ShipModelSlotType.CM, "Prefabs/Ships/Modules/H_EQ0001_CM"},
				{ ShipModelSlotType.ES, "Prefabs/Ships/Modules/H_EQ0001_ES"} });
		}
	}
}
