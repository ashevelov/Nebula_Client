/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.Space
{
    using Game.Space.Resources;

    public static class XmlLoader 
    {

        private static Dictionary<ModuleGroupType, ModuleGroup> moduleGroups = null;

        private static WorldEventsCache _worldEventCache = null;
       
        public static Dictionary<ModuleGroupType, ModuleGroup> ModuleGroups
        {
            get
            {
                LoadModules();
                return moduleGroups;
            }
        }

        public static ModuleGroup GetModuleGroup(ModuleGroupType moduleGroupType)
        {
            LoadModules();
            if (moduleGroups.ContainsKey(moduleGroupType))
                return moduleGroups[moduleGroupType];
            return null;
        }

        //loading functions
        private static void LoadModules()
        {
            if (moduleGroups == null || moduleGroups.Count == 0)
            {
                sXDocument document = new sXDocument();
                moduleGroups = new Dictionary<ModuleGroupType, ModuleGroup>();
                document.Parse("Texts/modules");


                moduleGroups = document.GetElement("modules").GetElements("module_group").Select((mge) =>
                {
                    ModuleGroupType moduleGroupType = mge.GetEnum<ModuleGroupType>("type");
                    float maxEnergy = mge.GetFloat("max_energy");
                    float overheatInterval = mge.GetFloat("overheat_interval");
                    float overheatSpeed = mge.GetFloat("overheat_speed");
                    string moduleGroupAbbreviation = mge.GetString("abbr");

                    Dictionary<ModuleType, Module> modules = mge.GetElements("module").Select((me) =>
                    {
                        if (moduleGroupType == ModuleGroupType.battle)
                        {
                            ModuleType moduleType = me.GetEnum<ModuleType>("type");
                            float maxModuleEnergy = me.GetFloat("max_energy");
                            float minModuleEnergy = me.GetFloat("min_energy");
                            float minDamage = me.GetFloat("min_damage");
                            float maxDamage = me.GetFloat("max_damage");
                            float minEfficiencyPercent = me.GetFloat("min_efficiency_percent");
                            string abbreviation = me.GetString("abbr");
                            return new { KEY = moduleType, VALUE = (Module)(new BattleModule(moduleType, minModuleEnergy, maxModuleEnergy, minDamage, maxDamage, minEfficiencyPercent, abbreviation)) };
                        }
                        else
                        {
                            Debug.LogError(string.Format("Unsopperted module group type: {0}", moduleGroupType));
                            return null;
                        }
                    }).ToDictionary((kv) => kv.KEY, (kv) => kv.VALUE);
                    ModuleGroup group = new ModuleGroup(moduleGroupType, maxEnergy, overheatInterval, overheatSpeed, modules, moduleGroupAbbreviation);
                    return new { KEY = moduleGroupType, VALUE = group };
                }).ToDictionary((kv) => kv.KEY, (kv) => kv.VALUE);
            }
        }

        public static WorldEventsCache WorldEventCache {
            get {
                if (_worldEventCache == null)
                    _worldEventCache = new WorldEventsCache();
                if (_worldEventCache.Loaded == false)
                    _worldEventCache.Load();
                return _worldEventCache;
            }
        }
    }

}

*/