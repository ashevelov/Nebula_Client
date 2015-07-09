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
            AddInfo(Nebula.Resources.StringCache.Get("HP"), (int)info.MaxHP);
            AddInfo(Nebula.Resources.StringCache.Get("ENERGY"), (int)info.MaxEnergy);
            AddInfo(Nebula.Resources.StringCache.Get("SPEED"), (int)info.MaxSpeed);
            AddInfo(Nebula.Resources.StringCache.Get("DAMAGE"), (int)info.damage);
            AddInfo(Nebula.Resources.StringCache.Get("RESIST"), (int)info.DamageResist);
            AddInfo(Nebula.Resources.StringCache.Get("OPTIMAL"), (int)info.WeaponOptimalDistance);
            if (uicPanel == null)
            {
                Debug.Log("uicPanel == null");
                uicPanel = GetComponent<IShipModuleInfo>();
            }
            uicPanel.SetAllParams(names, values);

            string nameRaceWorkshop = GameData.instance.playerInfo.Name;
            nameRaceWorkshop += "  /  " + GameData.instance.playerInfo.Race;
            nameRaceWorkshop += "  /  " + GameData.instance.playerInfo.Workshop;
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


            AddInfo(Nebula.Resources.StringCache.Get("HP"), info.damage);
            AddInfo(Nebula.Resources.StringCache.Get("CRIT"), info.critDamage);
            AddInfo(Nebula.Resources.StringCache.Get("HIT"), (int)info.HitProb);
            AddInfo(Nebula.Resources.StringCache.Get("RANGE"), (int)info.Range);
            AddInfo(Nebula.Resources.StringCache.Get("LEVEL"), (int)info.WeaponObject.Level);
            AddInfo(Nebula.Resources.StringCache.Get("COLOR"), (int)info.WeaponObject.Color);

            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.WeaponObject.Template);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = null;
            string skillDesc = "No Skill";
            uicPanel.SetSkill(skillIcon, skillDesc);
        }

        string names = string.Empty;
        string values = string.Empty;

        private void UpdateModuleInfo(ClientShipModule info)
        {
            names = string.Empty;
            values = string.Empty;


            AddInfo(Nebula.Resources.StringCache.Get("NAME"), info.name);
            AddInfo(Nebula.Resources.StringCache.Get("LEVEL"), info.level);
            AddInfo(Nebula.Resources.StringCache.Get("HP"), (int)info.hp);
            AddInfo(Nebula.Resources.StringCache.Get("ENERGY"), (int)info.energy);
            AddInfo(Nebula.Resources.StringCache.Get("RESIST"), (int)info.resist);
            AddInfo(Nebula.Resources.StringCache.Get("SPEED"), (int)info.speed);
            AddInfo(Nebula.Resources.StringCache.Get("DAMAGE"), (int)info.damageBonus);
            AddInfo(Nebula.Resources.StringCache.Get("CRIT_CHANCE"), (int)info.critChance);
            AddInfo(Nebula.Resources.StringCache.Get("CRIT_DAMAGE"), (int)info.critDamage);
            AddInfo(Nebula.Resources.StringCache.Get("HOLD"), (int)info.hold);
            AddInfo(Nebula.Resources.StringCache.Get("COLOR"), (int)info.color);

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
