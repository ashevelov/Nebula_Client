using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UIC
{
    public class PlayerInfo : MonoBehaviour, IPlayerInfo
    {


        #region HP

        public Text maxHP;
        public Text curentHP;
        public Image progressHp;

        float _maxHP;
        float _curentHP;
        public int MaxHP
        {
            set
            {
                maxHP.text = " / " + value.ToString();
                _maxHP = value;
            }
        }

        public int CurentHP
        {
            set
            {
                curentHP.text = value.ToString();
                _curentHP = value;
                UpdateProgressHp();
            }
        }

        void UpdateProgressHp()
        {
            progressHp.fillAmount = _curentHP / _maxHP;
        }
        #endregion

        #region Energy

        public Text maxEnergy;
        public Text curentEnergy;
        public Image progressEnergy;

        float _maxEnergy;
        float _curentEnergy;
        public int MaxEnegry
        {
            set
            {
                maxEnergy.text = " / " + value.ToString();
                _maxEnergy = value;
                UpdateProgressEnergy();
            }
        }
        public int CurentEnergy
        {
            set
            {
                curentEnergy.text = value.ToString();
                _curentEnergy = value;
                UpdateProgressEnergy();
            }
        }

        void UpdateProgressEnergy()
        {
            progressEnergy.fillAmount = _curentEnergy / _maxEnergy;
        }


        #endregion

        #region EXP


        public Text expProgress;
        
        public int MaxEXP
        {
            set
            {

            }
        }

        public int CurentExp
        {
            set
            {
            }
        }


        void UpdateProgressExp()
        {
        }

        #endregion

        #region Info

        public Text info;

        string _playerName;
        int _speed;
        string _position;

        public string Position
        {
            set
            {
                _position = value;
                UpdateInfo();
            }
        }

        public string Name
        {
            set
            {
                _playerName = value;
                UpdateInfo();
            }
        }

        public int Speed
        {
            set
            {
                _speed = value;
                UpdateInfo();
            }
        }


        void UpdateInfo()
        {
            info.text = _playerName +  "  /  " + _speed +"  /  "+ _position;
        }
        #endregion
    }
}
