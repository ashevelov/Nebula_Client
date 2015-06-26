using UnityEngine;
using System.Collections;
using Client.UIC;
using Nebula.Client;
using Nebula.Resources;

namespace Client.UIP.Implementation
{
    public class ShipInfoProcess : MonoBehaviour
    {
        IShipModuleInfo uicPanel;

        void Start()
        {
            uicPanel = GetComponent<IShipModuleInfo>();
        }


        public void UpdateCBInfo()
        {
            ClientShipModule info = G.Game.Ship.ShipModel.cb.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateDFInfo()
        {
            ClientShipModule info = G.Game.Ship.ShipModel.df.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateDMInfo()
        {
            ClientShipModule info = G.Game.Ship.ShipModel.dm.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateCMInfo()
        {
            ClientShipModule info = G.Game.Ship.ShipModel.cm.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateESInfo()
        {
            ClientShipModule info = G.Game.Ship.ShipModel.es.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateWeaponInfo()
        {
            //ClientShipModule info = G.Game.Ship.ShipModel.cb.Module;
            //UpdateModuleInfo(info);
        }

        private void UpdateModuleInfo(ClientShipModule info)
        {
            string names = string.Empty;
            string values = string.Empty;

            //AddInfo( out names, out values, "Name", info.name);

            names += "Name\n"; values += info.name + "\n";
            names += "level\n"; values += info.level + "\n";
            names += "hp\n"; values += (int)info.hp + "\n";
            names += "energy\n"; values += (int)info.energy + "\n";
            names += "resist\n"; values += (int)info.resist + "\n";
            names += "speed\n"; values += (int)info.speed + "\n";
            names += "damageBonus\n"; values += (int)info.damageBonus + "\n";
            names += "critChance\n"; values += (int)info.critChance + "\n";
            names += "critDamage\n"; values += (int)info.critDamage + "\n";
            names += "hold\n"; values += (int)info.hold + "\n";
            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.templateId);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
            string skillDesc = StringCache.Get("H" + info.skill.ToString("X8")); //SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
            uicPanel.SetSkill(skillIcon, skillDesc);
        }

        //private void AddInfo(out string names, out string values, string name, string value)
        //{
        //    if (value != "0")
        //    {
        //        names += name + "\n";
        //        values += value + "\n";
        //    }
        //}
    }
}
