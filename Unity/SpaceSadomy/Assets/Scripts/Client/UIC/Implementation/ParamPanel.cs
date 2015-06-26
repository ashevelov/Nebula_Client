using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Client.UIC.Implementation
{
    public class ParamPanel : MonoBehaviour
    {

        public Text name;
        public Text value;

        public void Init(string name, string value)
        {
            this.name.text = name;
            this.value.text = value;
        }
    }
}
