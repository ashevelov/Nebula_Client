using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

namespace Skills
{
	public class ActiveSkill
	{
		private float _cooldown;
        private float _active_time;
        private float _time;

        public string Id { get; set; }

        public bool active
        {
            get
            {
                return _time <= _active_time/_cooldown;
            }
        }

        public void UseSkill(float cooldown, float active_time)
        {
            _cooldown = cooldown;
            _active_time = active_time;
            _time = 0;
        }

        public float progress
        {
            get
            {
                _time = Mathf.Clamp((_time + (Time.deltaTime / _cooldown)), 0, 1);
                return _time;
            }
        }
	}
}
