using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Game.Space;
using Game.Network;
using Nebula.Game.Network.Items;
using Nebula;

public class ItemSubscribedEventStrategy : IServerEventStrategy 
{
    public void Handle(NetworkGame game, EventData eventData)
    {
        HandleEventItemSubscribed(game, eventData.Parameters);
    }

    private void HandleEventItemSubscribed(NetworkGame game, IDictionary eventData)
    {

        var itemType = eventData.GetValue((byte)ParameterCode.ItemType, (byte)ItemType.Avatar); //(byte)eventData[(byte)ParameterCode.ItemType];
        var itemId = (string)eventData[(byte)ParameterCode.ItemId];
        var position = (float[])eventData[(byte)ParameterCode.Position];
        var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];
        float[] rotation = eventData.Contains((byte)ParameterCode.Rotation) ? (float[])eventData[(byte)ParameterCode.Rotation] : null;
        byte subType = eventData.Contains((byte)ParameterCode.SubType) ? (byte)eventData[(byte)ParameterCode.SubType] : (byte)0;
        Hashtable itemProperties = eventData.Contains((byte)ParameterCode.Properties) ? (Hashtable)eventData[(byte)ParameterCode.Properties] : new Hashtable();

        string displayName = eventData.Contains((byte)ParameterCode.Username) ? (string)eventData[(byte)ParameterCode.Username] : string.Empty;


        //Debug.Log("item subscribed: {0} of type: {1}".f(itemId, itemType.toItemType()));

        if ((ItemType)itemType == ItemType.Avatar)
        {
            Debug.Log("avatar player subscrived: {0}".f(itemId).Bold().Color("orange"));
        }

        Item item;
        if (game.TryGetItem(itemType, itemId, out item))
        {
            item.SetSubscribed(true);

            if (item.IsMine)
            {
                item.AddSubscribedInterestArea(cameraId);
                item.AddVisibleInterestArea(cameraId);
            }
            else
            {
                var revision = (int)eventData[(byte)ParameterCode.PropertiesRevision];
                if (revision == item.PropertyRevision)
                {
                    item.AddSubscribedInterestArea(cameraId);
                    item.AddVisibleInterestArea(cameraId);
                }
                else
                {
                    item.AddSubscribedInterestArea(cameraId);
                    item.GetProperties(null);
                }

                item.SetPositions(position, position, rotation, rotation, 0);

            }
            if (!item.ExistsView)
            {
                MmoEngine.Get.CreateActor(game, item);
            }
        }
        else
        {
            switch ((ItemType)itemType)
            {
                case ItemType.Avatar:
                    item = new ForeignPlayerItem(itemId, itemType, game, displayName);
                    item.SetPositions(position, position, rotation, rotation, 0);
                    game.AddItem(item);
                    item.AddSubscribedInterestArea(cameraId);
                    item.GetProperties(null);
                    item.SetSubscribed(true);
                    break;
                case ItemType.Bot:

                    switch ((BotItemSubType)subType)
                    {
                        case BotItemSubType.StandardCombatNpc:
                            {
                                item = new StandardNpcCombatItem(itemId, itemType, game, BotItemSubType.StandardCombatNpc, displayName);
                                item.SetPositions(position, position, rotation, rotation, 0);
                                item.SetSubscribed(true);
                                foreach (DictionaryEntry entry in itemProperties)
                                {
                                    string group = entry.Key.ToString();
                                    Hashtable groupProperties = entry.Value as Hashtable;
                                    if (groupProperties != null)
                                    {
                                        item.SetProperties(group, groupProperties);
                                    }
                                }
                                game.AddItem(item);
                                item.AddSubscribedInterestArea(cameraId);
                                item.GetProperties(null);
                            }
                            break;
                        case BotItemSubType.PirateStation:
                            {
                                item = new PirateStationItem(itemId, itemType, game, BotItemSubType.PirateStation, displayName);
                                item.SetPositions(position, position, rotation, rotation, 0);
                                item.SetSubscribed(true);
                                foreach (DictionaryEntry entry in itemProperties)
                                {
                                    item.SetProperties(entry.Key.ToString(), entry.Value as Hashtable);
                                }
                                game.AddItem(item);
                                item.AddSubscribedInterestArea(cameraId);
                                item.GetProperties(null);
                            }
                            break;
                        case BotItemSubType.Planet:
                            {
                                Debug.Log("<color=orange>Planet subscribed</color>");
                                item = new PlanetItem(itemId, itemType, game, displayName);
                                item.SetPositions(position, position, rotation, rotation, 0);
                                item.SetSubscribed(true);
                                foreach (DictionaryEntry entry in itemProperties)
                                {
                                    item.SetProperties(entry.Key.ToString(), entry.Value as Hashtable);
                                }
                                game.AddItem(item);
                                item.AddSubscribedInterestArea(cameraId);
                                item.GetProperties(null);
                            }
                            break;
                        case BotItemSubType.Activator:
                            {
                                CreateActivator(itemId, itemType, game, displayName, position, rotation, cameraId);
                            }
                            break;
                    }
                    break;
                case ItemType.Chest:
                    {
                        CreateChest(itemId, itemType, game, displayName, position, rotation, cameraId);
                    }
                    break;
                case ItemType.Asteroid:
                    {
                        CreateAsteroid(itemId, itemType, game, displayName, position, rotation, cameraId);
                    }
                    break;

            }


        }
    }

    private void CreateChest(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId)
    {
        Item item = new ChestItem(id, type, game, name);
        item.SetPositions(position, position, rotation, rotation, 0);
        item.SetSubscribed(true);
        game.AddItem(item);
        item.AddSubscribedInterestArea(areaId);
        item.GetProperties(null);
    }

    private void CreateActivator(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId)
    {
        Debug.Log("Activator subscribed");
        var item = new WorldActivatorItem(id, type, game, name);
        item.SetPositions(position, position, rotation, rotation, 0);
        item.SetSubscribed(true);
        game.AddItem(item);
        item.AddSubscribedInterestArea(areaId);
        item.GetProperties(null);
    }

    private  void CreateAsteroid(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId)
    {
        Item item = new AsteroidItem(id, type, game, name);
        item.SetPositions(position, position, rotation, rotation, 0);
        item.SetSubscribed(true);
        game.AddItem(item);
        item.AddSubscribedInterestArea(areaId);
        item.GetProperties(new string[] { GroupProps.ASTEROID });
    }
}
