namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;
    using Nebula.Mmo.Items;
    using Nebula;
    using Nebula.Mmo.Games;
    using global::Game.Space;

    public class ItemSubscribedEvent : BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            HandleEventItemSubscribed((NetworkGame)game, eventData.Parameters);
        }

        private void HandleEventItemSubscribed(NetworkGame game, IDictionary eventData) {

            var itemType = eventData.GetValue((byte)ParameterCode.ItemType, (byte)ItemType.Avatar); //(byte)eventData[(byte)ParameterCode.ItemType];
            var itemId = (string)eventData[(byte)ParameterCode.ItemId];
            var position = (float[])eventData[(byte)ParameterCode.Position];
            var cameraId = (byte)eventData[(byte)ParameterCode.InterestAreaId];
            float[] rotation = eventData.Contains((byte)ParameterCode.Rotation) ? (float[])eventData[(byte)ParameterCode.Rotation] : null;
            byte subType = eventData.Contains((byte)ParameterCode.SubType) ? (byte)eventData[(byte)ParameterCode.SubType] : (byte)0;
            Hashtable itemProperties = eventData.Contains((byte)ParameterCode.Properties) ? (Hashtable)eventData[(byte)ParameterCode.Properties] : new Hashtable();
            object[] components = (object[])eventData[(byte)ParameterCode.Components];

            //System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //builder.Append(itemId + ": ");
            //foreach(var componentObj in components) {
            //    builder.Append(((ComponentID)(int)componentObj).ToString() + ", ");
            //}
            //Debug.Log(builder.ToString());

            string displayName = eventData.Contains((byte)ParameterCode.Username) ? (string)eventData[(byte)ParameterCode.Username] : string.Empty;


            //Debug.Log("item subscribed: {0} of type: {1}".f(itemId, itemType.toItemType()));

            if ((ItemType)itemType == ItemType.Avatar) {
                Debug.Log("avatar player subscrived: {0}".f(itemId).Bold().Color("orange"));
            }
            if((ItemType)itemType == ItemType.Asteroid) {
                Debug.LogFormat("asteroid subscribed at position {0},{1},{2}", position[0], position[1], position[2]);
            }
            if((ItemType)itemType == ItemType.Bot) {
                Debug.Log(string.Format("bot = {0} subscribed", itemId).Color("orange"));
                //Debug.LogFormat("BOT subscribed of sub type: {0}", (BotItemSubType)subType);
            }


            Item item;
            if (game.TryGetItem(itemType, itemId, out item)) {
                item.SetSubscribed(true);

                if (item.IsMine) {
                    item.AddSubscribedInterestArea(cameraId);
                    item.AddVisibleInterestArea(cameraId);
                } else {
                    var revision = (int)eventData[(byte)ParameterCode.PropertiesRevision];
                    if (revision == item.PropertyRevision) {
                        item.AddSubscribedInterestArea(cameraId);
                        item.AddVisibleInterestArea(cameraId);
                    } else {
                        item.AddSubscribedInterestArea(cameraId);
                        item.GetProperties();
                    }

                    item.SetPositions(position, position, rotation, rotation, 0);

                }
                if (!item.ExistsView) {
                    MmoEngine.Get.CreateActor(game, item);
                }
            } else {
                switch ((ItemType)itemType) {
                    case ItemType.Avatar:
                        item = new ForeignPlayerItem(itemId, itemType, game, displayName, components);
                        item.SetPositions(position, position, rotation, rotation, 0);
                        game.AddItem(item);
                        item.AddSubscribedInterestArea(cameraId);
                        item.GetProperties();
                        item.SetSubscribed(true);
                        break;
                    case ItemType.Bot:

                        switch ((BotItemSubType)subType) {

                            case BotItemSubType.StandardCombatNpc:
                                {
                                    item = new StandardNpcCombatItem(itemId, itemType, game, BotItemSubType.StandardCombatNpc, displayName, components);
                                    item.SetPositions(position, position, rotation, rotation, 0);
                                    item.SetSubscribed(true);
                                    item.SetProperties(itemProperties);
                                    game.AddItem(item);
                                    item.AddSubscribedInterestArea(cameraId);
                                    item.GetProperties();
                                }
                                break;
                            case BotItemSubType.PirateStation:
                                {
                                    //item = new PirateStationItem(itemId, itemType, game, BotItemSubType.PirateStation, displayName, components);
                                    //item.SetPositions(position, position, rotation, rotation, 0);
                                    //item.SetSubscribed(true);
                                    //item.SetProperties(itemProperties);
                                    //game.AddItem(item);
                                    //item.AddSubscribedInterestArea(cameraId);
                                    //item.GetProperties();

                                    CreateMmoItem(itemId, itemType, game, displayName, position, rotation, cameraId, itemProperties, components);
                                }
                                break;
                            case BotItemSubType.Planet:
                                {
                                    Debug.Log("<color=orange>Planet subscribed</color>");
                                    item = new PlanetItem(itemId, itemType, game, displayName, components);
                                    item.SetPositions(position, position, rotation, rotation, 0);
                                    item.SetSubscribed(true);
                                    item.SetProperties(itemProperties);
                                    game.AddItem(item);
                                    item.AddSubscribedInterestArea(cameraId);
                                    item.GetProperties();
                                }
                                break;
                            case BotItemSubType.Activator:
                                {
                                    CreateActivator(itemId, itemType, game, displayName, position, rotation, cameraId, components);
                                }
                                break;
                            default:
                                {
                                    Debug.Log("CREATE MMO ITEM");
                                    CreateMmoItem(itemId, itemType, game, displayName, position, rotation, cameraId, itemProperties, components);
                                }
                                break;

                        }
                        break;
                    case ItemType.Chest:
                        {
                            CreateChest(itemId, itemType, game, displayName, position, rotation, cameraId, components);
                        }
                        break;
                    case ItemType.Asteroid:
                        {
                            CreateAsteroid(itemId, itemType, game, displayName, position, rotation, cameraId, components);
                        }
                        break;
                    case ItemType.Event: { CreateEvent(itemId, itemType, game, displayName, position, rotation, cameraId, itemProperties, components); }
                        break;
                }


            }
        }

        private void CreateMmoItem(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId, Hashtable properties, object[] components) {
            Item item = new MmoItem(id, type, game, name, components);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            item.SetProperties(properties);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties();
        }

        private void CreateEvent(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId, Hashtable properties, object[] components) {
            Item item = new EventItem(id, type, game, name, components);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            item.SetProperties(properties);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties();
        }

        private void CreateChest(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId, object[] components) {
            Item item = new ChestItem(id, type, game, name, components);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties();
        }

        private void CreateActivator(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId, object[] components) {
            Debug.Log("Activator subscribed");
            var item = new WorldActivatorItem(id, type, game, name, components);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties();
        }

        private void CreateAsteroid(string id, byte type, NetworkGame game, string name, float[] position, float[] rotation, byte areaId, object[] components) {
            Item item = new AsteroidItem(id, type, game, name, components);
            item.SetPositions(position, position, rotation, rotation, 0);
            item.SetSubscribed(true);
            game.AddItem(item);
            item.AddSubscribedInterestArea(areaId);
            item.GetProperties();
        }
    }
}