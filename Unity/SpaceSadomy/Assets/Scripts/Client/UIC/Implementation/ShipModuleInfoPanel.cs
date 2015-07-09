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


    public Text allModulesNames;
    public Text allModulesValues;

    public void SetAllParams(string names, string values)
    {
        this.allModulesNames.text = names;
        this.allModulesValues.text = values;
    }

    public Text nameRaceWorkshop;
    public void SetNameRaceWorkshop(string text)
    {
        nameRaceWorkshop.text = text;
    }

    public Image CB;
    public Image DF;
    public Image DM;
    public Image CM;
    public Image ES;

    public void UpdateModuleIcon(string moduleType, Sprite icon) 
    {
        Debug.Log("type " + moduleType +" icon " + icon);
        if (icon != null)
        {
            switch (moduleType)
            {
                case "CB":
                    CB.sprite = icon;
                    return;
                case "DF":
                    DF.sprite = icon;
                    return;
                case "DM":
                    DM.sprite = icon;
                    return;
                case "CM":
                    CM.sprite = icon;
                    return;
                case "ES":
                    ES.sprite = icon;
                    return;
            }
        }
    }

}
