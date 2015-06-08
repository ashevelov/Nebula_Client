using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIC
{
    public class TargetInfo : MonoBehaviour, ITargetInfo
    {
        public Text targeName;

        public string Name
        {
            set { targeName.text = value; }
        }

        public Image targetIcon;
        public Sprite Icon
        {
            set { targetIcon.sprite = value; }
        }

        #region HP

        public Text HP;
        public Image progressHp;

        float _maxHP;
        float _curentHP;
        public int MaxHP
        {
            set { _maxHP = value; }
        }

        public int CurentHP
        {
            set
            {
                _curentHP = value;
                UpdateProgressHp();
            }
        }

        void UpdateProgressHp()
        {
            progressHp.fillAmount = _curentHP / _maxHP;
            HP.text = _curentHP + " / " + _maxHP;
        }
        #endregion

        public Text targetDistance;
        public float Distance
        {
            set { targetDistance.text = System.Math.Round(value, 2) + "км"; }
        }
    }
}
