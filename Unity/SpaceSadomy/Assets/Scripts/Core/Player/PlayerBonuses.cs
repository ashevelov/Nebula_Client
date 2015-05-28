using System.Collections;
using System;
using System.Collections.Generic;
using Common;

namespace Nebula {

	public interface IBonuses
	{
		Bonus GetBonus(BonusType type);
	}
	
	public interface IBonus
	{
		void ReplaceBuff(string id, Buff buff);
		void RemoveBuff(string id);
		void RemoveInvalidBuffs();
		float getValue{ get; }
	}
	
	public delegate bool CheckBuff();
	public interface IBuff
	{
		bool Check();
		float power{ get; }
	}
	

	
	public class PlayerBonuses : IBonuses
	{
		private Dictionary<BonusType, Bonus> bonuses;
		
		public PlayerBonuses()
		{
			bonuses = new Dictionary<BonusType, Bonus>();

            var BonusTypeList = CastArrayToBonusTypeList(Enum.GetValues(typeof(BonusType)));
			
			BonusTypeList.ForEach((bt)=>{
				bonuses.Add(bt, new Bonus());
			});
		}

        private List<BonusType> CastArrayToBonusTypeList(Array arr) {
            List<BonusType> result = new List<BonusType>();
            foreach (var obj in arr) {
                result.Add((BonusType)obj);
            }
            return result;
        }

		
		public Bonus GetBonus(BonusType type)
		{
			if(bonuses.ContainsKey(type))
			{
				return bonuses[type];
			}
			return null;
		}
	}
	
	public class Bonus : IBonus
	{
		private Dictionary<string, Buff> _buffs;
		private List<string> _removeBuffs;
		
		public Bonus()
		{
			_buffs = new Dictionary<string, Buff>();
			_removeBuffs = new List<string>();
		}

		public void ReplaceBuff(string id, Buff buff)
		{
			if(_buffs.ContainsKey(id))
			{
				_buffs[id] = buff;
			}
			else
			{
				_buffs.Add(id, buff);
			}
		}
		
		public void RemoveBuff(string id)
		{
			_buffs.Remove(id);
		}
		
		public void RemoveInvalidBuffs()
		{
			_removeBuffs.ForEach((b)=>{
				if(_buffs.ContainsKey(b))
				{
					_buffs.Remove(b);
				}
			});
			_removeBuffs.Clear();
		}
		
		public float getValue
		{
			get
			{
				float value = 0;
				
				foreach(var b in _buffs){
					if(b.Value.Check())
					{
						value += b.Value.power;
					}
					else
					{
						_removeBuffs.Add(b.Key);
					}
				}
				RemoveInvalidBuffs();
				
				return value;
			}
		}
		
	}
	
	public class Buff : IBuff
	{
		
		private CheckBuff _checkBuff;
		private float _power;

		public Buff(float value)
		{
			_power = value;
			_checkBuff = ()=> {return true;};
		}

		public Buff(float value , CheckBuff check)
		{
			_power = value;
			_checkBuff = check;
		}
		
		public bool Check()
		{
			if(_checkBuff != null && _checkBuff())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public float power
		{
			get
			{
				return _power;
			}
		}
	}
	
	/*
	public enum BonusType
	{
		fieldPoints,
		fieldResist,
		armorPoints,
		armorResist,
		weaponDamage,
		weaponSpeed,
		shipSpeed
	}*/
}
