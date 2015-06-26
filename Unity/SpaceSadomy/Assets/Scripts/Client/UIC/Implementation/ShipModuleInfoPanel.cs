using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Client.UIC;

public class ShipModuleInfoPanel : MonoBehaviour, IShipModuleInfo {

    public Text names;
    public Text values;

    public void SetParams(string names, string values)
    {
        this.names.text = names;
        this.values.text = values;
    }


    public Image skillIcon;
    public Text skillDesc;

    public void SetSkill(Sprite icon, string description)
    {
        if (icon != null)
        {
            skillIcon.gameObject.SetActive(true);
            skillIcon.sprite = icon;
        }
        else
        {
            skillIcon.gameObject.SetActive(false);
        }
        skillDesc.text = description;
        
    }

    public Image icon;

    public void SetIcon(Sprite icon)
    {
        if (icon != null)
        {
            this.icon.gameObject.SetActive(true);
            this.icon.sprite = icon;
        }
        else
        {
            this.icon.gameObject.SetActive(false);
        }
    }
}
