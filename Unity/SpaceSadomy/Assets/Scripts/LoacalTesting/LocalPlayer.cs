using UnityEngine;
using System.Collections;

namespace LocalTest
{
    [System.Serializable]
    public class LocalPlayer
    {
        public string name = "Player";
        public float hp = 200;
        [HideInInspector]
        public float curent_hp = 200;
        public float speed = 150;
       // [HideInInspector]
        public float curent_speed = 150;
        public float maneuverability = 1;
        public float damage = 15;
        public float attackSpeed = 7;
        public float energy = 100;
        [HideInInspector]
        public float curent_energy = 100;
        public float resist = 0.3f;
        public float optimalRange = 1000;
        public float extraRange = 4000;
        public float precision = 100;
        public float tracking = 2;
        [HideInInspector]
        public bool fastTravel;
    }
}
