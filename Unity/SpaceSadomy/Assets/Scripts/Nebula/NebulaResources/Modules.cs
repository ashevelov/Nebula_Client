namespace Nebula.Resources {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public enum ModuleGroupType : byte { battle = 0 }

    public class Modules
    {
        private Dictionary<ModuleGroupType, ModuleGroup> moduleGroups;
    }

    public class ModuleGroup
    {
        private ModuleGroupType type;
        private Dictionary<ModuleType, Module> modules;
        private float maxEnergy;
        private float overheatInterval;
        private float overheatSpeed;
        private string displayAbbreviation;

        public ModuleGroup(ModuleGroupType type, float maxEnergy, float overheatInterval, float overheatSpeed, Dictionary<ModuleType, Module> modules,
            string abbreviation )
        {
            this.type = type;
            this.maxEnergy = maxEnergy;
            this.overheatInterval = overheatInterval;
            this.overheatSpeed = overheatSpeed;
            this.modules = modules;
            this.displayAbbreviation = abbreviation;
        }

        public Module GetModule(ModuleType moduleType)
        {
            if (modules.ContainsKey(moduleType))
                return modules[moduleType];
            return null;
        }

        public ModuleGroupType Type
        {
            get { return type; }
        }

        public Dictionary<ModuleType, Module> Modules
        {
            get { return modules; }
        }
        public float MaxEnergy
        {
            get { return maxEnergy; }
        }
        public float OverheatInterval
        {
            get { return overheatInterval; }
        }
        public float OverheatSpeed
        {
            get { return overheatSpeed; }
        }
        public string Abbreviation
        {
            get { return displayAbbreviation; }
        }


    }

    public enum ModuleType : byte { 
        thermal = 0, 
        electromagnetic = 1, 
        explosive = 2, 
        kinetic = 3 
    }

    public abstract class Module
    {
        private ModuleType type;
        public Module(ModuleType type)
        {
            this.type = type;
        }

        public ModuleType Type
        {
            get { return type; }
        }
    }

    public class BattleModule : Module
    {
        private float minEnergy;
        private float maxEnergy;
        private float minDamage;
        private float maxDamage;
        private float minEfficiencyPercent;
        private string displayAbbreviation;

        public BattleModule(ModuleType type, float minEnergy, float maxEnergy, float minDamage, float maxDamage, float minEfficiencyPercent, string abbreviation)
            : base(type)
        {
            this.minEnergy = minEnergy;
            this.maxEnergy = maxEnergy;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.minEfficiencyPercent = minEfficiencyPercent;
            this.displayAbbreviation = abbreviation;
        }
        public float MinEnergy
        {
            get { return minEnergy; }
        }
        public float MaxEnergy
        {
            get { return maxEnergy; }
        }

        public float MinDamage
        {
            get { return minDamage; }
        }

        public float MaxDamage
        {
            get { return maxDamage; }
        }

        public float MinEfficiencyPercent
        {
            get { return minEfficiencyPercent; }
        }
        public string Abbreviation
        {
            get { return displayAbbreviation; }
        }
    }
}