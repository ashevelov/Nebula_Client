using UnityEngine;
using System.Collections;
using Client.UIC;
using Nebula.Client;
using Nebula.Resources;
using Nebula;

namespace Client.UIP.Implementation
{
    public class ShipInfoProcess : MonoBehaviour
    {
        IShipModuleInfo uicPanel;

        void Start()
        {
            uicPanel = GetComponent<IShipModuleInfo>();
        }

        public void UpdateAllModulesInfo()
        {
            ClientShipCombatStats info = GameData.instance.stats; //G.Game.CombatStats;
            names = string.Empty;
            values = string.Empty;
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryHP"), (int)info.MaxHP);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryenergy"), (int)info.MaxEnergy);
            AddInfo(Nebula.Resources.StringCache.Get("inventoryspeed"), (int)info.MaxSpeed);
            AddInfo(Nebula.Resources.StringCache.Get("inventorydamage"), (int)info.damage);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryresistance"), (int)info.DamageResist);
            AddInfo(Nebula.Resources.StringCache.Get("inventoryoptimal"), (int)info.WeaponOptimalDistance);
            if (uicPanel == null)
            {
                Debug.Log("uicPanel == null");
                uicPanel = GetComponent<IShipModuleInfo>();
            }
            uicPanel.SetAllParams(names, values);

            string nameRaceWorkshop = GameData.instance.playerInfo.Name;
            nameRaceWorkshop += "  /  " + Nebula.Resources.StringCache.Get("RACE_"+GameData.instance.playerInfo.Race.ToString().ToUpper());
            nameRaceWorkshop += "  /  " + Nebula.Resources.StringCache.Get("WORKSHOP_" + GameData.instance.playerInfo.Workshop.ToString().Replace(" ", "").ToUpper());
            uicPanel.SetNameRaceWorkshop(nameRaceWorkshop);

            uicPanel.UpdateModuleIcon("CB", SpriteCache.SpriteModule("ShipInfoModules/" + GameData.instance.ship.ShipModel.cb.Module.templateId));
            uicPanel.UpdateModuleIcon("DF", SpriteCache.SpriteModule("ShipInfoModules/" + GameData.instance.ship.ShipModel.df.Module.templateId));
            uicPanel.UpdateModuleIcon("DM", SpriteCache.SpriteModule("ShipInfoModules/" + GameData.instance.ship.ShipModel.dm.Module.templateId));
            uicPanel.UpdateModuleIcon("CM", SpriteCache.SpriteModule("ShipInfoModules/" + GameData.instance.ship.ShipModel.cm.Module.templateId));
            uicPanel.UpdateModuleIcon("ES", SpriteCache.SpriteModule("ShipInfoModules/" + GameData.instance.ship.ShipModel.es.Module.templateId));
        }

        public void UpdateCBInfo()
        {
            ClientShipModule info = GameData.instance.ship.ShipModel.cb.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateDFInfo()
        {
            ClientShipModule info = GameData.instance.ship.ShipModel.df.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateDMInfo()
        {
            ClientShipModule info = GameData.instance.ship.ShipModel.dm.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateCMInfo()
        {
            ClientShipModule info = GameData.instance.ship.ShipModel.cm.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateESInfo()
        {
            ClientShipModule info = GameData.instance.ship.ShipModel.es.Module;
            UpdateModuleInfo(info);
        }
        public void UpdateWeaponInfo()
        {
            ClientPlayerShipWeapon info = GameData.instance.ship.Weapon;

            names = string.Empty;
            values = string.Empty;


            AddInfo(Nebula.Resources.StringCache.Get("inventorydamage"), info.damage);
            AddInfo(Nebula.Resources.StringCache.Get("inventorycritdamage"), info.critDamage);
            AddInfo(Nebula.Resources.StringCache.Get("inventoryoptimal"), (int)info.Range);
            AddInfo(Nebula.Resources.StringCache.Get("inventory_level"), (int)info.WeaponObject.Level);
            //AddInfo(Nebula.Resources.StringCache.Get("COLOR"), (int)info.WeaponObject.Color);

            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.WeaponObject.Template);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = null;
            string skillDesc = Nebula.Resources.StringCache.Get("inventory_noskill");
            uicPanel.SetSkill(skillIcon, skillDesc);
        }

        string names = string.Empty;
        string values = string.Empty;

        private void UpdateModuleInfo(ClientShipModule info)
        {
            names = string.Empty;
            values = string.Empty;


            AddInfo(Nebula.Resources.StringCache.Get("inventory_name"), Nebula.Resources.StringCache.Get(info.type+"_NAME"));
            AddInfo(Nebula.Resources.StringCache.Get("inventory_level"), info.level);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryHP"), (int)info.hp);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryenergybonus"), (int)info.energy);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryresistance"), (int)info.resist);
            AddInfo(Nebula.Resources.StringCache.Get("inventoryspeedbonus"), (int)info.speed);
            AddInfo(Nebula.Resources.StringCache.Get("inventorydamagebonus"), (int)info.damageBonus);
            AddInfo(Nebula.Resources.StringCache.Get("inventorycritchance"), (int)info.critChance);
            AddInfo(Nebula.Resources.StringCache.Get("inventorycritdamage"), (int)info.critDamage);
            AddInfo(Nebula.Resources.StringCache.Get("hum_inventoryhold"), (int)info.hold);
            //AddInfo(Nebula.Resources.StringCache.Get("COLOR"), (int)info.color);

            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.templateId);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = null;
            string skillDesc = "No Skill";
            if (info.HasSkill)
            {
                skillIcon = SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
                skillDesc = StringCache.Get("H" + info.skill.ToString("X8")); //SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
            }
            uicPanel.SetSkill(skillIcon, skillDesc);
        }

        private void AddInfo(string name, object value)
        {
            string val = value.ToString();
            if (val != "0" && val != "")
            {
                names += name + "\n";
                values += val + "\n";
            }
        }
    }
}
