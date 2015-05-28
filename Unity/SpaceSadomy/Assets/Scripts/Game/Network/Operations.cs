using System;
using System.Collections;
using System.Collections.Generic;

using Common;


namespace Nebula
{
    /// <summary>
    /// The operations.
    /// </summary>
    [CLSCompliant(false)]
    public static class Operations
    {

        public static void SelectCharacter(NetworkGame game, string characterId, string login, string password)
        {
            var data = new Dictionary<byte, object>
        {
            { ParameterCode.GameRefId.toByte(), game.LoginInfo.gameRefId },
            {ParameterCode.CharacterId.toByte(), characterId },
            {ParameterCode.Login.toByte(), login },
            {ParameterCode.Password.toByte(), password }
        };

            game.SendOperation(OperationCode.SelectCharacter, data, true, Settings.ItemChannel);
        }

        public static void AddCharacter(NetworkGame game, byte race, Workshop workshop)
        {
            var data = new Dictionary<byte, object>
        {
            {ParameterCode.WorkshopId.toByte(), (byte)workshop },
            {ParameterCode.GameRefId.toByte(), game.LoginInfo.gameRefId },
            {ParameterCode.Race.toByte() , race }
        };
            game.SendOperation(OperationCode.AddCharacter, data, true, Settings.ItemChannel);
        }

        public static void Login(NetworkGame game, string login, string password, string displayName)
        {

            var data = new Dictionary<byte, object> { 
            {(byte)ParameterCode.Login, login },
            {(byte)ParameterCode.Password, password },
            {(byte)ParameterCode.Username, displayName }
        };
            game.SendOperation(OperationCode.Login, data, true, Settings.ItemChannel);
        }
        /// <summary>
        /// The add interest area.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="cameraId">
        /// The camera id.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view distance enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view distance exit.
        /// </param>
        public static void AddInterestArea(NetworkGame NetworkGame, byte cameraId, float[] position, float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.InterestAreaId, cameraId }, 
                    { (byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter }, 
                    { (byte)ParameterCode.ViewDistanceExit, viewDistanceExit }, 
                    { (byte)ParameterCode.Position, position }
                };

            NetworkGame.SendOperation(OperationCode.AddInterestArea, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The attach camera.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void AttachInterestArea(NetworkGame NetworkGame, string itemId, byte? itemType)
        {
            var data = new Dictionary<byte, object>();

            if (!string.IsNullOrEmpty(itemId))
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            NetworkGame.SendOperation(OperationCode.AttachInterestArea, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The counter subscribe.
        /// </summary>
        /// <param name="peer">
        /// The photon peer.
        /// </param>
        /// <param name="receiveInterval">
        /// The receive interval.
        /// </param>
        public static void CounterSubscribe(PhotonPeer peer, int receiveInterval)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.CounterReceiveInterval, receiveInterval } };
            peer.OpCustom((byte)OperationCode.SubscribeCounter, data, true, Settings.DiagnosticsChannel);
        }

        /// <summary>
        /// The create world.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="worldName">
        /// The world name.
        /// </param>
        /// <param name="topLeftCorner">
        /// The top left corner.
        /// </param>
        /// <param name="bottomRightCorner">
        /// The bottom right corner.
        /// </param>
        /// <param name="tileDimensions">
        /// The tile dimensions.
        /// </param>
        public static void CreateWorld(NetworkGame NetworkGame, string worldName)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.WorldName, worldName }, 
                };
            NetworkGame.SendOperation(OperationCode.CreateWorld, data, true, Settings.OperationChannel);
        }

        /// <summary>
        /// The destroy item.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void DestroyItem(NetworkGame NetworkGame, string itemId, byte itemType)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };
            NetworkGame.SendOperation(OperationCode.DestroyItem, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The detach camera.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        public static void DetachInterestArea(NetworkGame NetworkGame)
        {
            NetworkGame.SendOperation(OperationCode.DetachInterestArea, new Dictionary<byte, object>(), true, Settings.ItemChannel);
        }

        /// <summary>
        /// The enter world.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="worldName">
        /// The world name.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        public static void EnterWorld(
            NetworkGame NetworkGame, string worldName, Dictionary<string, Hashtable> properties, float[] position, float[] rotation, float[] viewDistanceEnter, float[] viewDistanceExit, string gameRefID)
        {
            //UnityEngine.Debug.Log("try enter to world: " + worldName);
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.WorldName, worldName }, 
                    { (byte)ParameterCode.Position, position }, 
                    { (byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter }, 
                    { (byte)ParameterCode.ViewDistanceExit, viewDistanceExit },
                    {(byte)ParameterCode.GameRefId, gameRefID }
                };
            if (properties != null)
            {
                data.Add((byte)ParameterCode.Properties, properties.toHash());
            }

            if (rotation != null)
            {
                data.Add((byte)ParameterCode.Rotation, rotation);
            }

            NetworkGame.SendOperation(OperationCode.EnterWorld, data, true, Settings.OperationChannel);
        }

        /// <summary>
        /// The exit world.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        public static void ExitWorld(NetworkGame NetworkGame)
        {
            NetworkGame.SendOperation(OperationCode.ExitWorld, new Dictionary<byte, object>(), true, Settings.OperationChannel);
        }

        public static void EnterWorkshop(NetworkGame game, string itemId, WorkshopStrategyType workshopStrategyType)
        {
            game.SendOperation(OperationCode.EnterWorkshop,
                new Dictionary<byte, object>() 
            { 
                { ParameterCode.ItemId.toByte(), itemId },
                { ParameterCode.Type.toByte(), (byte)workshopStrategyType}
            }, true,
                Settings.OperationChannel);
        }

        public static void ExitWorkshop(NetworkGame game)
        {
            game.SendOperation(OperationCode.ExitWorkshop, new Dictionary<byte, object>(), true, Settings.OperationChannel);
        }

        /// <summary>
        /// The get properties.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="knownRevision">
        /// The known revision.
        /// </param>
        public static void GetProperties(NetworkGame NetworkGame, string itemId, byte itemType, int? knownRevision, string[] groups)
        {

            var data = new Dictionary<byte, object> { 
        { (byte)ParameterCode.ItemId, itemId }, 
        { (byte)ParameterCode.ItemType, itemType } };

            if (knownRevision.HasValue)
            {
                data.Add((byte)ParameterCode.PropertiesRevision, knownRevision.Value);
            }

            if (groups != null)
            {
                data.Add((byte)ParameterCode.Groups, groups);
            }
            else
            {
                data.Add((byte)ParameterCode.Groups, new string[] { });
            }

            NetworkGame.SendOperation(OperationCode.GetProperties, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The move operation.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="sendReliable">
        /// The send Reliable.
        /// </param>
        public static void Move(NetworkGame NetworkGame, string itemId, byte? itemType, float[] position, float[] rotation, bool sendReliable)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.Position, position } };
            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            if (rotation != null)
            {
                data.Add((byte)ParameterCode.Rotation, rotation);
            }

            NetworkGame.SendOperation(OperationCode.Move, data, sendReliable, Settings.ItemChannel);
        }

        /// <summary>
        /// The move camera.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="cameraId">
        /// The camera id.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public static void MoveInterestArea(NetworkGame NetworkGame, byte cameraId, float[] position)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.InterestAreaId, cameraId }, { (byte)ParameterCode.Position, position } };

            NetworkGame.SendOperation(OperationCode.MoveInterestArea, data, true, Settings.ItemChannel);
        }

        public static void ExecAction(NetworkGame NetworkGame, string itemid, string action, object[] parameters)
        {
            var data = new Dictionary<byte, object> {
            {(byte)ParameterCode.ItemId, itemid },
            {(byte)ParameterCode.Action, action },
            {(byte)ParameterCode.Parameters, parameters }
        };
            NetworkGame.SendOperation(OperationCode.ExecAction, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The radar subscribe.
        /// </summary>
        /// <param name="peer">
        /// The photon peer.
        /// </param>
        /// <param name="worldName">
        /// The world Name.
        /// </param>
        public static void RadarSubscribe(PhotonPeer peer, string worldName)
        {
            /*
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.WorldName, worldName } };
            peer.OpCustom((byte)OperationCode.RadarSubscribe, data, true, Settings.RadarChannel);
             * */
        }

        /// <summary>
        /// The raise generic event.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="customEventCode">
        /// The custom event code.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <param name="eventReliability">
        /// The event reliability.
        /// </param>
        /// <param name="eventReceiver">
        /// The event receiver.
        /// </param>
        public static void RaiseGenericEvent(
            NetworkGame NetworkGame, string itemId, byte? itemType, byte customEventCode, object eventData, byte eventReliability, EventReceiver eventReceiver)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.CustomEventCode, customEventCode }, 
                    { (byte)ParameterCode.EventReliability, eventReliability }, 
                    { (byte)ParameterCode.EventReceiver, (byte)eventReceiver }
                };

            if (eventData != null)
            {
                data.Add((byte)ParameterCode.EventData, eventData);
            }

            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            NetworkGame.SendOperation(OperationCode.RaiseGenericEvent, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The remove interest area.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="cameraId">
        /// The camera id.
        /// </param>
        public static void RemoveInterestArea(NetworkGame NetworkGame, byte cameraId)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.InterestAreaId, cameraId } };

            NetworkGame.SendOperation(OperationCode.RemoveInterestArea, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The set properties.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="propertiesSet">
        /// The properties set.
        /// </param>
        /// <param name="propertiesUnset">
        /// The properties unset.
        /// </param>
        /// <param name="sendReliable">
        /// The send Reliable.
        /// </param>
        public static void SetProperties(NetworkGame NetworkGame, string itemId, byte? itemType, Dictionary<string, Hashtable> propertiesSet, ArrayList propertiesUnset, bool sendReliable)
        {
            var data = new Dictionary<byte, object>();
            if (propertiesSet != null)
            {
                data.Add((byte)ParameterCode.PropertiesSet, propertiesSet.toHash());
            }

            if (propertiesUnset != null)
            {
                data.Add((byte)ParameterCode.PropertiesUnset, propertiesUnset);
            }

            if (itemId != null)
            {
                data.Add((byte)ParameterCode.ItemId, itemId);
            }

            if (itemType.HasValue)
            {
                data.Add((byte)ParameterCode.ItemType, itemType.Value);
            }

            NetworkGame.SendOperation(OperationCode.SetProperties, data, sendReliable, Settings.ItemChannel);
        }

        /// <summary>
        /// The set view distance.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="viewDistanceEnter">
        /// The view Distance Enter.
        /// </param>
        /// <param name="viewDistanceExit">
        /// The view Distance Exit.
        /// </param>
        public static void SetViewDistance(NetworkGame NetworkGame, float[] viewDistanceEnter, float[] viewDistanceExit)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.ViewDistanceEnter, viewDistanceEnter }, { (byte)ParameterCode.ViewDistanceExit, viewDistanceExit } 
                };
            NetworkGame.SendOperation(OperationCode.SetViewDistance, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The spawn item.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        /// <param name="rotation">
        /// The rotation.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="subscribe">
        /// The subscribe.
        /// </param>
        public static void SpawnItem(NetworkGame NetworkGame, string itemId, byte itemType, float[] position, float[] rotation, Hashtable properties, bool subscribe)
        {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.Position, position }, 
                    { (byte)ParameterCode.ItemId, itemId }, 
                    { (byte)ParameterCode.ItemType, itemType }, 
                    { (byte)ParameterCode.Subscribe, subscribe }
                };
            if (properties != null)
            {
                data.Add((byte)ParameterCode.Properties, properties);
            }

            if (rotation != null)
            {
                data.Add((byte)ParameterCode.Rotation, rotation);
            }

            NetworkGame.SendOperation(OperationCode.SpawnItem, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The subscribe item.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        /// <param name="propertiesRevision">
        /// The properties revision.
        /// </param>
        public static void SubscribeItem(NetworkGame NetworkGame, string itemId, byte itemType, int? propertiesRevision)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };
            if (propertiesRevision.HasValue)
            {
                data.Add((byte)ParameterCode.PropertiesRevision, propertiesRevision);
            }

            NetworkGame.SendOperation(OperationCode.SubscribeItem, data, true, Settings.ItemChannel);
        }

        /// <summary>
        /// The unsubscribe item.
        /// </summary>
        /// <param name="NetworkGame">
        /// The mmo NetworkGame.
        /// </param>
        /// <param name="itemId">
        /// The item id.
        /// </param>
        /// <param name="itemType">
        /// The item type.
        /// </param>
        public static void UnsubscribeItem(NetworkGame NetworkGame, string itemId, byte itemType)
        {
            var data = new Dictionary<byte, object> { { (byte)ParameterCode.ItemId, itemId }, { (byte)ParameterCode.ItemType, itemType } };

            NetworkGame.SendOperation(OperationCode.UnsubscribeItem, data, true, Settings.ItemChannel);
        }
    }
}