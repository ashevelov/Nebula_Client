using System.Collections;
using Skills;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Common;

namespace Game.Space
{
	public class PlayerSkills : IInfo
	{
        private List<ActiveSkill> _activeSkills;
        public PlayerSkills()
        {
             _activeSkills = new List<ActiveSkill>();
            for (int i = 0; i < 6; i++)
            {
                _activeSkills.Add(new ActiveSkill());
            }
            UpdateSkills(new string[6] { "SK0105", "SK0107", "SK0115", "SK0102", "SK0104", "SK1103" });
        }

       

        public void UpdateSkills(string[] skillsId)
        {
            if (skillsId.Length == 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    _activeSkills[i].Id = skillsId[i];
                }
            }
        }
        public void UpdateSkill(int index, string id)
        {
            if (_activeSkills != null && _activeSkills.Count > index)
            {
                _activeSkills[index].Id = ( id != "" ) ? id : "Empty";
            }
        }

        public string SkillId(int index)
        {
            if (_activeSkills != null && _activeSkills.Count > index)
            {
                return _activeSkills[index].Id;
            }
            return "Empty";
        }

        public bool active(int index)
        {
            if (_activeSkills != null && _activeSkills.Count > index)
            {
                return _activeSkills[index].active;
            }
            return false;
        }

        public void UseSkill(int index, float cooldown, float active_time)
        {
            if (_activeSkills != null && _activeSkills.Count > index)
            {
                _activeSkills[index].UseSkill(cooldown, active_time);
            }
        }

        public void UseSkill(string id, float cooldown, float active_time)
        {
            if (_activeSkills != null)
            {
                _activeSkills.ForEach((s)=>{
                    if (s.Id == id)
                    {
                        s.UseSkill(cooldown, active_time);
                    }
                });
            }
        }

        public float progress(int index)
        {
            if (_activeSkills != null && _activeSkills.Count > index)
            {
                return _activeSkills[index].progress;
            }
            return 0f;
        }


        public Hashtable GetInfo()
        {
            return new Hashtable();
        }

        public void ParseInfo(Hashtable info)
        {
            foreach (DictionaryEntry entry in info)
            {
                this.UpdateSkill((int)entry.Key, (string)entry.Value);
            }
        }
    }
}
