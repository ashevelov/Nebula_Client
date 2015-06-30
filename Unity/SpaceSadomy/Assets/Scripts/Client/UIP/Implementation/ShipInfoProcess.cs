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


            AddInfo("HP", (int)info.MaxHP);
            AddInfo("Energy", (int)info.MaxEnergy);
            AddInfo("Speed", (int)info.MaxSpeed);
            AddInfo("Damage", (int)info.damage);
            AddInfo("Resist", (int)info.DamageResist);
            AddInfo("Optimal Distance", (int)info.WeaponOptimalDistance);

            uicPanel.SetAllParams(names, values);
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


            AddInfo("damage", info.damage);
            AddInfo("level", info.critDamage);
            AddInfo("HitProb", (int)info.HitProb);
            AddInfo("Range", (int)info.Range);
            AddInfo("Level", (int)info.WeaponObject.Level);
            AddInfo("Color", (int)info.WeaponObject.Color);

            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.WeaponObject.Template);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = null;
            string skillDesc = ""; //SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
            uicPanel.SetSkill(skillIcon, skillDesc);
        }

        string names = string.Empty;
        string values = string.Empty;

        private void UpdateModuleInfo(ClientShipModule info)
        {
            names = string.Empty;
            values = string.Empty;


            AddInfo("Name", info.name);
            AddInfo("level", info.level);
            AddInfo("hp", (int)info.hp);
            AddInfo("energy", (int)info.energy);
            AddInfo("resist", (int)info.resist);
            AddInfo("speed", (int)info.speed);
            AddInfo("damageBonus", (int)info.damageBonus);
            AddInfo("critChance", (int)info.critChance);
            AddInfo("critDamage", (int)info.critDamage);
            AddInfo("hold", (int)info.hold);
            AddInfo("color", (int)info.color);

            uicPanel.SetParams(names, values);

            Sprite icon = SpriteCache.SpriteModule(info.templateId);
            uicPanel.SetIcon(icon);

            Sprite skillIcon = SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
            string skillDesc = StringCache.Get("H" + info.skill.ToString("X8")); //SpriteCache.SpriteSkill("H" + info.skill.ToString("X8"));
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
