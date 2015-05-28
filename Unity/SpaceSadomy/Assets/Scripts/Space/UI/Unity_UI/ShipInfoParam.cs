using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipInfoParam : MonoBehaviour {

    public Text param;
    public Text value;

    public void SetInfo(string param, string value)
    {
        this.param.text = param;
        this.value.text = value;
    }
}
