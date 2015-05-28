using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Space;

public class HotKeyController : MonoBehaviour {

	public List<HotKey> hotKeys = new List<HotKey>();

	public void Load()
	{
		TestLoad();
		LoadHotKey();
	}

	public void UpdateSkill()
	{
        /*
		NetworkGame.Get.GamePlayer.Ship.Skills.useSkills.ForEach((s)=>{
			s.time -= Time.deltaTime;
			s.cooldown -= Time.deltaTime;
			if( s.time <=0 )
			{
				s.active = false;
				s.time = 0;
			}
			if(s.cooldown <=0 )
			{
				s.cooldown = 0;
			}
		});*/

	}


	private void LoadHotKey()
	{
        /*
		hotKeys[0].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(0).Use(); };
		hotKeys[1].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(1).Use(); };
		hotKeys[2].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(2).Use(); };
		hotKeys[3].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(3).Use(); };
		hotKeys[4].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(4).Use(); };
		hotKeys[5].action = ()=>{ NetworkGame.Get.GamePlayer.Ship.Skills.GetSkill(5).Use(); };*/

		//NetworkGame.Get.GamePlayer.Ship.Skills.ReplaceSkill(0, new Skills.WeaponDamage());

	}

	private void TestLoad()
	{
	
		hotKeys.Add(new HotKey{iconID = "attack", action = null,  key = "1"});
		hotKeys.Add(new HotKey{iconID = "move", action = null,  key = "2"});
		hotKeys.Add(new HotKey{iconID = "jamp", action = null,  key = "3"});
		hotKeys.Add(new HotKey{iconID = "stop", action = null,  key = "4"});
		hotKeys.Add(new HotKey{iconID = "stop", action = null,  key = "5"});
		hotKeys.Add(new HotKey{iconID = "stop", action = null,  key = "6"});

	}
	public class HotKey
	{
		public string iconID;
		public System.Action action;
		public string key;
	}
}
