using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Nebula.Client.Inventory;
using Nebula.Client.Inventory.Objects;
using Nebula.Client;

namespace Game.Space
{
    public static class TextureCache
    {
        private static Dictionary<string, Texture2D> cache;

        private static readonly TextureSubCache<Race> raceTextures = new TextureSubCache<Race>();
        private static readonly TextureSubCache<InventoryObjectType> inventoryItemTypeTextures = new TextureSubCache<InventoryObjectType>();
        private static readonly TextureSubCache<string> materialTextures = new TextureSubCache<string>();
        private static readonly TextureSubCache<ObjectColor> objectColorTextures = new TextureSubCache<ObjectColor>();
        private static readonly TextureSubCache<string> moduleTextures = new TextureSubCache<string>();

        static TextureCache() {
            cache = new Dictionary<string, Texture2D>();
        }

        public static Texture2D Get(string path) {
            if (cache.ContainsKey(path))
            {
                return cache[path];
            }
            else {
                Texture2D tex = UnityEngine.Resources.Load<Texture2D>(path);
                if (tex != null)
                    cache.Add(path, tex);
                return tex;
            }
        }

        public static Texture2D RaceTexture(Race race)
        {
            switch(race)
            {
                case Race.Humans:
                    return raceTextures.GetTexture(race, "Textures/Icons/Npc/humans");
                case Race.Borguzands:
                    return raceTextures.GetTexture(race, "Textures/Icons/Npc/burgi");
                case Race.Criptizoids:
                    return raceTextures.GetTexture(race, "Textures/Icons/Npc/criptizid");
                default:
                    {
                        return raceTextures.GetTexture(race, "Textures/Icons/planet");
                        //throw new InvalidRaceException(race, "Texture for race not exists");
                    }
            }
        }

        public static Texture2D TextureForItem(IInventoryObjectInfo itemObj)
        {
            switch(itemObj.Type)
            {
                case InventoryObjectType.Weapon:
                    return inventoryItemTypeTextures.GetTexture(InventoryObjectType.Weapon, "UI/Textures/weapon");
                case InventoryObjectType.Scheme:
                    return inventoryItemTypeTextures.GetTexture(InventoryObjectType.Scheme, "UI/Textures/schemes");
                case InventoryObjectType.Material:
                    return materialTextures.GetTexture(itemObj.Id, "UI/Textures/Ore/" + itemObj.Id);
                default:
                    return materialTextures.GetTexture("TEXTURE_NOT_EXIST", "UI/Textures/red");

            }
        }

        public static Texture2D ColorTexture(IInventoryObjectInfo itemObj)
        {
            switch (itemObj.Type)
            {
                case InventoryObjectType.Scheme:
                    {
                        ObjectColor color = ((SchemeInventoryObjectInfo)itemObj).Color;
                        return objectColorTextures.GetTexture(color, "UI/Textures/item_color_" + color);
                    }
                case InventoryObjectType.Weapon:
                    {
                        ObjectColor color = ((WeaponInventoryObjectInfo)itemObj).Color;
                        return objectColorTextures.GetTexture(color, "UI/Textures/item_color_" + color);
                    }
                default:
                    {
                        //Debug.Log("no color for object type: {0}".f(itemObj.Type));
                        return null;
                    }
            }
        }

        public static Texture2D TextureForModule(ClientShipModule module) {
            return moduleTextures.GetTexture(module.templateId, "UI/Textures/Modules/" + module.templateId);
        }
    }
}
