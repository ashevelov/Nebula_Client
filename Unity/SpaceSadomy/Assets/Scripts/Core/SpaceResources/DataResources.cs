using Common;
using Game.Space;
using Game.Space.Res;
using Nebula.Client.Res;
using Space.Game.Resources;
using System.Collections.Generic;
using Nebula;

// DataResources.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, November 5, 2014 8:06:25 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;

public class DataResources  {

    private static DataResources instance;

    public static DataResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataResources();
                instance.Load();
            }
            return instance;
        }
    }

    private bool loaded = false;
    private List<ResModuleData> modules;
    private List<ResMaterialData> ores;
    private List<ResSkillData> skills;
    private List<ResSetData> sets;
    private Dictionary<string, ResTooltipData> tooltips;
    private List<ResObjectIconData> objectIcons;
    private Dictionary<BonusType, ResBuffData> buffs;
    private Dictionary<string, string> moduleSetNames;
    private Dictionary<GameSoundsType, AudioClip> gameSounds;
    private Leveling leveling;
    private ResZones zones;
    private ResWeapons weapons;
    private ResHelp help;
    private ResRaces races;
    private ResSchemes schemes;
    private ResMiscInventoryItems miscInventoryItems;
    public ResGameEvents gameEvents { get; private set; }
    public ResPrefabsDB prefabsDb { get; private set; }


    private List<ResLoreData> lore;
    private Dictionary<string, string> strings;


    private void Load()
    {
        if (false == this.loaded)
        {
            this.modules = ResLoader.LoadModules(Resources.Load<TextAsset>("Data/Drop/modules").text);
            this.ores = ResLoader.LoadOres(Resources.Load<TextAsset>("Data/Materials/ore").text);
            this.skills = ResLoader.LoadSkills(Resources.Load<TextAsset>("Data/Skills/skills").text);
            this.sets = ResLoader.LoadSets(Resources.Load<TextAsset>("Data/Drop/module_set").text);
            this.tooltips = ResLoader.LoadTooltips(Resources.Load<TextAsset>("DataClient/tooltips").text);
            this.objectIcons = ObjectIcons.LoadObjectIcons(Resources.Load<TextAsset>("Data/object_icons").text);
            this.buffs = ResLoader.LoadBuffs(Resources.Load<TextAsset>("DataClient/buffs").text);
            this.moduleSetNames = ModulesSetNames.LoadModulesSetNames(Resources.Load<TextAsset>("Data/set_names").text);
            this.gameSounds = ResSoundsData.LoadGameSounds(Resources.Load<TextAsset>("Data/sounds").text);
            this.leveling = ResLoader.LoadLeveling(Resources.Load<TextAsset>("Data/leveling").text);
            this.lore = ResLore.LoadLore(Resources.Load<TextAsset>("Data/lore").text);
            this.strings = LoadAllStringFromFiles("DataClient/Strings");
            this.zones = ResLoader.LoadZones(Resources.Load<TextAsset>("DataClient/zones").text);
            this.weapons = ResLoader.LoadWeapons(Resources.Load<TextAsset>("Data/Drop/weapons").text);
            this.help = ResLoader.LoadHelp(Resources.Load<TextAsset>("DataClient/help").text);
            this.races = ResLoader.LoadRaces(Resources.Load<TextAsset>("DataClient/races").text);
            this.schemes = ResLoader.LoadSchemes(Resources.Load<TextAsset>("DataClient/schemes").text);
            this.miscInventoryItems = ResLoader.LoadMiscInventoryItems(Resources.Load<TextAsset>("Data/misc_inventory_items").text);
            gameEvents = ResLoader.LoadGameEvents(Resources.Load<TextAsset>("Data/zones").text);
            prefabsDb = ResLoader.LoadPrefabsDB(Resources.Load<TextAsset>("DataClient/prefabs_db").text);

            this.loaded = true;
        }
    }

    private Dictionary<string, string> LoadAllStringFromFiles(string directoryPath)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        TextAsset[] assets = Resources.LoadAll<TextAsset>("DataClient/Strings");
        try
        {
            foreach (var asset in assets)
            {
                Debug.Log("load string asset: {0}".f(asset.name).Color(Color.yellow));
                result.AddRange(ResLoader.LoadStrings(asset.text));
            }
        }
        catch(System.ArgumentException e)
        {
            Debug.LogError(e.Message);
            Debug.LogError(e.ParamName);
        }
        return result;
    }

    public ResModuleData ModuleData(string id)
    {
        if (false == this.loaded)
            return null;
        return GetModule(id);
    }

    private ResModuleData GetModule(string id) {
        foreach (var module in this.modules) {
            if (module.Id == id) {
                return module;
            }
        }
        return null;
    }

    public List<ResModuleData> Modules
    {
        get
        {
            return this.modules;
        }
    }

    public ResMaterialData OreData(string id)
    {
        if (false == this.loaded)
            return null;
        return GetOreData(id);
    }

    private ResMaterialData GetOreData(string id) {
        foreach (var ore in this.ores) {
            if (ore.Id == id) {
                return ore;
            }
        }
        return null;
    }

    public ResSkillData SkillData(string id)
    {
        if (false == this.loaded)
            return null;
        return GetSkillData(id);
    }

    private ResSkillData GetSkillData(string id) {
        foreach (var skill in this.skills) {
            if (skill.id == id) {
                return skill;
            }
        }
        return null;
    }

    public ResSkillData SkillData(System.Func<ResSkillData, bool> predicate)
    {
        foreach (var s in this.skills) {
            if (predicate(s)) {
                return s;
            }
        }
        return null;
    }

    public ResSetData SetData(string id)
    {
        if (false == this.loaded)
            return null;

        foreach (var s in this.sets) {
            if (s.Id == id) {
                return s;
            }
        }
        return null;
    }



    public ResTooltipData ToolTip(string id)
    {
        ResTooltipData data = null;
        this.tooltips.TryGetValue(id, out data);

        if (data == null)
        {
            data = new ResTooltipData { Id = id, Text = string.Empty };
        }
        return data;
    }

    public ResObjectIconData ObjectIcon(string id)
    {
        if (false == this.loaded)
            return null;

        foreach (var objIcon in this.objectIcons) {
            if (objIcon.Id == id) {
                return objIcon;
            }
        }
        return null;
    }

    public ResObjectIconData ObjectIcon(IconType type)
    {
        if (false == this.loaded)
            return null;
        foreach (var objIcon in this.objectIcons) {
            if (objIcon.Type == type) {
                return objIcon;
            }
        }
        return null;
    }

    public ResBuffData Buff(BonusType bonusType)
    {
        if (this.buffs.ContainsKey(bonusType))
        {
            return this.buffs[bonusType];
        }
        return null;
    }

    public string NameModelSet(string setId)
    {
        if (moduleSetNames.ContainsKey(setId))
        {
            return moduleSetNames[setId];
        }
        return "not";
    }
    public AudioClip GetGameSound(GameSoundsType type)
    {
        if (gameSounds.ContainsKey(type))
        {
            return gameSounds[type]; 
        }
        return null;
    }

    public ResLoreData GetRandomLoreData()
    {
        if(lore != null)
            return lore[Random.Range(0, lore.Count)];

        return null;
    }

    public Leveling Leveling
    {
        get
        {
            return this.leveling;
        }
    }

    public ResSchemes Schemes {
        get {
            return this.schemes;
        }
    }

    public ResRaces ResRaces() {
        return this.races;
    }

    public string String(string key)
    {
        if (this.strings.ContainsKey(key))
            return this.strings[key];
        //Debug.LogError("not founded string with key: " + key);
        return key;
    }

    public ResZoneInfo ZoneForId(string id)
    {
        return this.zones.Zone(id);
    }

    public ResZoneInfo ZoneForScene(string scene)
    {
        return this.zones.ZoneForScene(scene);
    }

    public ResWeaponTemplate Weapon(string id)
    {
        return this.weapons.Weapon(id);
    }

    public List<ResHelpElement> HelpElements()
    {
        return this.help.Elements();
    }

    public ResMiscInventoryItems ResMiscInventoryItems() {
        return this.miscInventoryItems;
    }
 }
