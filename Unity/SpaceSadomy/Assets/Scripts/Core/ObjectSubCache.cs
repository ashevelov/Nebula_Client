using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Nebula.Client.Inventory;
using Nebula.Client.Inventory.Objects;
using Game.Space;
using Nebula.Client;
using Nebula.Client.Res;

public class ObjectSubCache<T,U> where T : Object {

    private Dictionary<U, T> cachedObjects = new Dictionary<U, T>();

    public T GetObject(U key, string globalCacheObjectPath) {
        if (this.cachedObjects.ContainsKey(key)) {
            return this.cachedObjects[key];
        } else {
            T obj = ObjectCache<T>.GetObject(globalCacheObjectPath);
            if (((object)obj) != null) {
                this.cachedObjects.Add(key, obj);
            }
            return obj;
        }
    }

    public void Preload(U key, string globalCacheObjectPath) {
        var obj = this.GetObject(key, globalCacheObjectPath);
    }
}

public class SpriteSubCache<U> : ObjectSubCache<Sprite, U> { 
    
}

public static class ItemTypeName {
    public const string ASTEROID = "asteroid";
    public const string PLANET = "planet";
    public const string STATION = "station";
    public const string PLAYER = "player";
    public const string EVENT = "event";
    public const string BOT_ENEMY = "bot_enemy";
    public const string MISC = "misc";
    public const string ACTIVATOR = "activator";
    public const string UNKNOWN = "unknown";
}

public static class SpriteCache {

    private static readonly SpriteSubCache<Race> raceSprites = new SpriteSubCache<Race>();
    private static readonly SpriteSubCache<InventoryObjectType> inventoryItemSprites = new SpriteSubCache<InventoryObjectType>();
    private static readonly SpriteSubCache<string> materialSprites = new SpriteSubCache<string>();
    private static readonly SpriteSubCache<ObjectColor> colorSprites = new SpriteSubCache<ObjectColor>();
    private static readonly SpriteSubCache<string> moduleSprites = new SpriteSubCache<string>();
    private static readonly SpriteSubCache<Workshop> workshopSprites = new SpriteSubCache<Workshop>();

    private static readonly Dictionary<Workshop, string> workshopSpritePathDict;

    private static readonly SpriteSubCache<string> oreSprites = new SpriteSubCache<string>();
    private static readonly SpriteSubCache<string> targetsSprites = new SpriteSubCache<string>();

    private static readonly SpriteSubCache<string> itemTypes = new SpriteSubCache<string>();
    private static readonly SpriteSubCache<string> skillSprites = new SpriteSubCache<string>();
    private static readonly SpriteSubCache<BonusType> buffSprites = new SpriteSubCache<BonusType>();

    private static readonly SpriteSubCache<Workshop> schemeSprites = new SpriteSubCache<Workshop>();



    private static Sprite missedSprite;
    private static Sprite transparentSprite;

    static SpriteCache() {
        workshopSpritePathDict = new Dictionary<Workshop, string> { 
            {Workshop.Arlen,        "Workshops/Arlen"},
            {Workshop.BigBang,      "Workshops/BigBang" },
            {Workshop.DarthTribe,   "Workshops/DarthTribe" },
            {Workshop.Dyneira,      "Workshops/Dyneira" },
            {Workshop.Equilibrium,  "Workshops/Equilibrium"},
            {Workshop.Evasive,      "Workshops/Evasive" },
            {Workshop.KrolRo,       "Workshops/KrolRo"},
            {Workshop.Lerjees,      "Workshops/Lerjees"},
            {Workshop.Molvice,      "Workshops/Molvice"},
            {Workshop.Phelpars,     "Workshops/Phelpars"},
            {Workshop.Rakhgals,     "Workshops/Rakhgals"},
            {Workshop.RedEye,       "Workshops/RedEye"},
            {Workshop.Serenity,     "Workshops/Serenity"},
            {Workshop.Yshuar,       "Workshops/Yshuar"},
            {Workshop.Zoards,       "Workshops/Zoards"}
        };

    }

    public static Sprite ItemType(string name) {
        switch(name) {
            case ItemTypeName.ASTEROID:
                return itemTypes.GetObject(ItemTypeName.ASTEROID, "ItemTypes/asteroid");
            case ItemTypeName.PLANET:
                return itemTypes.GetObject(ItemTypeName.PLANET, "ItemTypes/planet");
            case ItemTypeName.STATION:
                return itemTypes.GetObject(ItemTypeName.STATION, "ItemTypes/station");
            case ItemTypeName.PLAYER:
                return itemTypes.GetObject(ItemTypeName.PLAYER, "ItemTypes/player");
            case ItemTypeName.EVENT:
                return itemTypes.GetObject(ItemTypeName.EVENT, "ItemTypes/event");
            case ItemTypeName.BOT_ENEMY:
                return itemTypes.GetObject(ItemTypeName.BOT_ENEMY, "ItemTypes/bot_enemy");
            case ItemTypeName.MISC:
                return itemTypes.GetObject(ItemTypeName.MISC, "ItemTypes/misc");
            case ItemTypeName.ACTIVATOR:
                return itemTypes.GetObject(ItemTypeName.ACTIVATOR, "ItemTypes/activator");
            default:
                return itemTypes.GetObject(ItemTypeName.UNKNOWN, "ItemTypes/unknown");
        }
    }

    public static Sprite GetObject(string path) {
        return ObjectCache<Sprite>.GetObject(path);
    }

    public static Sprite RaceSprite(Race race) {
        switch (race) {
            case Race.Humans: {
                return raceSprites.GetObject(race, "Races/Humans");
                }
            case Race.Borguzands: {
                return raceSprites.GetObject(race, "Races/Borguzands");
                }
            case Race.Criptizoids: {
                return raceSprites.GetObject(race, "Races/Criptizids");
                }
            default: {
                return raceSprites.GetObject(race, "Missed");
                }
        }
    }

    public static Sprite WorkshopSprite(Workshop workshop) {
        return workshopSprites.GetObject(workshop, workshopSpritePathDict[workshop]);
    }

    public static Sprite SpriteForItem(IInventoryObjectInfo itemObj) {
        switch (itemObj.Type) {
            case InventoryObjectType.Weapon: {
                return inventoryItemSprites.GetObject(itemObj.Type, "UI/Textures/weapon_Sprite");
                }
            case InventoryObjectType.Scheme: {
                    ResSchemeData schemeData = null;
                    if (DataResources.Instance.Schemes.TryGetScheme(((SchemeInventoryObjectInfo)itemObj).Workshop, out schemeData)){
                        return SchemeSprite(schemeData);
                    }
                    return MissedSprite();
                }
            case InventoryObjectType.Material: {
                return materialSprites.GetObject(itemObj.Id, "Ore/" + itemObj.Id);
                }
            case InventoryObjectType.DrillScheme: {
                return inventoryItemSprites.GetObject(itemObj.Type, "UI/Textures/schemes_Sprite");
                }
            default: {
                return materialSprites.GetObject("SPRITE_NOT_EXIST", "UI/Textures/schemes_Sprite");
                }
        }
    }

    public static Sprite SpriteColor(IInventoryObjectInfo itemObj) {
        switch (itemObj.Type) {
            case InventoryObjectType.Scheme: {
                ObjectColor color = ((SchemeInventoryObjectInfo)itemObj).Color;
                return colorSprites.GetObject(color, "Colors/" + color);
                }
            case InventoryObjectType.Weapon: {
                ObjectColor color = ((WeaponInventoryObjectInfo)itemObj).Color;
                return colorSprites.GetObject(color, "Colors/" + color);
                }
            default: {
                return null;
                }
        }
    }

    public static Sprite SpriteColor(ClientShipModule module) {
        return colorSprites.GetObject(module.color, "UI/Textures/item_color_" + module.color + "_Sprite");
    }

    public static Sprite SpriteModule(string templateID) {
        return moduleSprites.GetObject(templateID, "UI/Textures/Modules_Sprites/" + templateID);
    }

    public static Sprite OreSprite(string oreId) {
        return oreSprites.GetObject(oreId, "Ore/" + oreId);
    }

    public static Sprite TargetSprite(string targetName) {
        return targetsSprites.GetObject(targetName, "Targets/" + targetName);
    }

    public static Sprite SpriteSkill(string skillId) {
        return skillSprites.GetObject(skillId, "Skills/" + skillId);
    }

    public static Sprite SpriteBuff(BonusType bonusType, string path) {
        return buffSprites.GetObject(bonusType, path);
    }
    //Return default red sprite for unknown or missed icons
    public static Sprite MissedSprite() {
        if(missedSprite == null ) {
            missedSprite = Resources.Load<Sprite>("Missed");
        }
        return missedSprite;
    }

    public static Sprite TransparentSprite() {
        if(transparentSprite == null ) {
            transparentSprite = Resources.Load<Sprite>("transparent");
        }
        return transparentSprite;
    }

    public static Sprite SchemeSprite(ResSchemeData scheme) {
        return schemeSprites.GetObject(scheme.Workshop, scheme.Icon);
    }
}
