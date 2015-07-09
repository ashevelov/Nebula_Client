using System;
using UnityEngine;
namespace Client.UIC
{
    interface IShipModuleInfo
    {
        void SetIcon(Sprite icon);
        void SetParams(string names, string values);
        void SetAllParams(string names, string values);
        void SetSkill(Sprite icon, string description);
        void SetNameRaceWorkshop(string text);
        void UpdateModuleIcon(string moduleType, Sprite icon);
    }
}
