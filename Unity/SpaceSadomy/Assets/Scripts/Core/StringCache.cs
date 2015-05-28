// StringCache.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, December 17, 2014 5:38:20 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;

public static class StringCache 
{
    private static Dictionary<string, string> cache;

    static StringCache()
    {
        cache = new Dictionary<string, string>();
    }

    public static string Get(string key)
    {
        if (cache.ContainsKey(key))
        {
            return cache[key];
        }
        else
        {
            cache[key] = DataResources.Instance.String(key).Trim();
            return cache[key];
        }
    }

    private static readonly StringSubCache<Race> raceStrings = new StringSubCache<Race>();
    private static readonly StringSubCache<Workshop> workshopStrings = new StringSubCache<Workshop>();
    private static readonly StringSubCache<ChatGroup> chatGroupNames = new StringSubCache<ChatGroup>();
    private static readonly StringSubCache<ShipModelSlotType> moduleTypeNames = new StringSubCache<ShipModelSlotType>();
    private static readonly StringSubCache<ModuleSetBonusType> setBonusTypes = new StringSubCache<ModuleSetBonusType>();

    public static string SetBonus(ModuleSetBonusType bonusType) {
        return setBonusTypes.String(bonusType, setBonusKeys[bonusType]);
    }

    public static string Race(Race race)
    {
        string key = string.Empty;
        switch(race)
        {
            case Common.Race.Humans:
                key = "RACE_HUMANS";
                break;
            case Common.Race.Borguzands:
                key = "RACE_BORGUZANDS";
                break;
            case Common.Race.Criptizoids:
                key = "RACE_CRIPTIZIDS";
                break;
            case Common.Race.None:
                key = "RACE_NONE";
                break;
        }
        
        return raceStrings.String(race, key).Trim();
    }

    public static string ChatGroupName(ChatGroup chatGroup) {
        return chatGroupNames.String(chatGroup, chatGroupNameKeys[chatGroup]);
    }

    public static string Workshop(Workshop workshop) {
        return workshopStrings.String(workshop, workshopNameKeys[workshop]).Trim();
    }

    public static string ModuleType(ShipModelSlotType moduleType) {
        return moduleTypeNames.String(moduleType, moduleTypeNameKeys[moduleType]);
    }

    private static readonly Dictionary<Common.Workshop, string> workshopNameKeys = new Dictionary<Common.Workshop, string> { 
        {Common.Workshop.DarthTribe, "WORKSHOP_DARTHTRIBE" },
        {Common.Workshop.RedEye, "WORKSHOP_REDEYE" },
        {Common.Workshop.Equilibrium, "WORKSHOP_EQUILIBRIUM"},
        {Common.Workshop.Evasive, "WORKSHOP_EVASIVE"},
        {Common.Workshop.BigBang, "WORKSHOP_BIGBANG"},
        {Common.Workshop.Serenity, "WORKSHOP_SERENITY" },
        {Common.Workshop.Rakhgals, "WORKSHOP_RAKHGALS"},
        {Common.Workshop.Phelpars, "WORKSHOP_PHELPARS"},
        {Common.Workshop.Zoards, "WORKSHOP_ZOARDS"},
        {Common.Workshop.Lerjees, "WORKSHOP_LERJEES"},
        {Common.Workshop.Yshuar, "WORKSHOP_YSHUAR"},
        {Common.Workshop.KrolRo, "WORKSHOP_KROLRO"},
        {Common.Workshop.Arlen, "WORKSHOP_ARLEN"},
        {Common.Workshop.Dyneira, "WORKSHOP_DYNEIRA"},
        {Common.Workshop.Molvice, "WORKSHOP_MOLVICE"}
    };

    private static readonly Dictionary<ChatGroup, string> chatGroupNameKeys = new Dictionary<ChatGroup, string> {
        { ChatGroup.all,                "CHAT_GROUP_ALL"            },
        { ChatGroup.alliance,           "CHAT_GROUP_ALLIANCE"       },
        {ChatGroup.group,               "CHAT_GROUP_GROUP"          },
        {ChatGroup.me,                  "CHAT_GROUP_ME"             },
        {ChatGroup.whisper,             "CHAT_GROUP_WHISPER"        },
        {ChatGroup.zone,                "CHAT_GROUP_ZONE"           },
        {ChatGroup.zone_and_alliance,   "CHAT_GROUP_ZONEALLIANCE"   }
    };

    private static readonly Dictionary<ShipModelSlotType, string> moduleTypeNameKeys = new Dictionary<ShipModelSlotType, string> {
        {ShipModelSlotType.CB, "CB_NAME" },
        {ShipModelSlotType.CM, "CM_NAME" },
        {ShipModelSlotType.DF, "DF_NAME" },
        {ShipModelSlotType.DM, "DM_NAME" },
        {ShipModelSlotType.ES, "ES_NAME" }
    };

    private static readonly Dictionary<ModuleSetBonusType, string> setBonusKeys = new Dictionary<ModuleSetBonusType, string> {
        { ModuleSetBonusType.crit_chance,           "BT_CC" },
        { ModuleSetBonusType.crit_damage,           "BT_CRDAM" },
        {ModuleSetBonusType.heal,                   "BT_HEAL" },
        { ModuleSetBonusType.heavy_attack_damage,   "BT_HAD"},
        { ModuleSetBonusType.hold,                  "BT_ISC"},
        {ModuleSetBonusType.hp,                     "BT_HP" },
        {ModuleSetBonusType.optimal_distance,       "BT_OD" },
        { ModuleSetBonusType.resistance,            "BT_RES" },
        {ModuleSetBonusType.speed,                  "BT_SPEED" }
    };

}
