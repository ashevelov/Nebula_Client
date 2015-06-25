using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UIC
{
    public class ControlPanel : MonoBehaviour, IControlPanel
    {
        public List<Button> buttons;
        public List<Image> skillProgress;
        public List<Image> skillIcon;
        private List<System.Action<int>> actions = new List<System.Action<int>>() { null, null, null, null, null, null, null, null, null };

        private Vector3 pos;

        float[] _progress = new float[9];
        public void UpdateButton(int index, float cooldown, float progress, Sprite icon = null, System.Action<int> action = null)
        {
            _progress[index] = progress;
            skillProgress[index].fillAmount = progress;
            cooldownSpeed[index] = 1/(cooldown+0.001f);
            Debug.Log(" index = " + index + " cooldown " + progress);
            

            if (icon != null)
            {
                skillIcon[index].sprite = icon;
            }
            if (action != null)
            {
                actions[index] = action;
                skillIcon[index].gameObject.SetActive(true);
            }
            else
            {
                skillIcon[index].gameObject.SetActive(false);
            }
        }

        private float[] cooldownSpeed = new float[9];
        void Update()
        {
            for(int i=0; i<skillProgress.Count; i++)
            {
                skillProgress[i].fillAmount -= cooldownSpeed[i] *Time.deltaTime;
            }
        }

        public void ButtonClick_0() { ButtonClick(0); }
        public void ButtonClick_1() { ButtonClick(1); }
        public void ButtonClick_2() { ButtonClick(2); }
        public void ButtonClick_3() { ButtonClick(3); }
        public void ButtonClick_4() { ButtonClick(4); }
        public void ButtonClick_5() { ButtonClick(5); }
        public void ButtonClick_6() { ButtonClick(6); }
        public void ButtonClick_7() { ButtonClick(7); }
        public void ButtonClick_8() { ButtonClick(8); }

        private void ButtonClick(int index)
        {
            if (actions[index] != null)
                actions[index](index);
        }
    }
}
