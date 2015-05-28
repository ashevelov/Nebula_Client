using UnityEngine;
using System.Collections;
using Common;
using ExitGames.Client.Photon;
using Game.Space;
using Game.Network;
using Nebula;

namespace Nebula
{
    public class WaitingForChangeWorld : IGameLogicStrategy 
    {
        public static readonly WaitingForChangeWorld Instance = new WaitingForChangeWorld();



        public GameState State
        {
            get {
                return GameState.WaitingForChangeWorld;
            }
        }

        public void OnEventReceive(NetworkGame game, ExitGames.Client.Photon.EventData eventData)
        {
            game.OnUnexpectedEventReceive(eventData);
        }

        public void OnOperationReturn(NetworkGame game, ExitGames.Client.Photon.OperationResponse response)
        {
            if (response.ReturnCode == 0)
            {
                switch ((OperationCode)response.OperationCode)
                {
                    case OperationCode.CreateWorld:
                        {
                            Debug.Log("world created");
                            game.OperationEnterWorld();
                        }
                        return;
                    case OperationCode.EnterWorld:

                        var worldData = new WorldData();
                        string name = (string)response.Parameters[(byte)ParameterCode.WorldName];
                        Vector3 max = ((float[])response.Parameters[(byte)ParameterCode.BottomRightCorner]).toVector();
                        Vector3 min = ((float[])response.Parameters[(byte)ParameterCode.TopLeftCorner]).toVector();
                        Vector3 tileDimensions = ((float[])response.Parameters[(byte)ParameterCode.TileDimensions]).toVector();
                        LevelType levelType = (LevelType)(byte)response.Parameters[(byte)ParameterCode.LevelType];
                        Hashtable worldContent = response.Parameters[ParameterCode.WorldContent.toByte()] as Hashtable;
                        if (worldContent != null)
                        {
                            game.ClientWorld.ParseInfo(worldContent);
                            worldContent.Print(1);
                        }
                        else
                        {
                            Debug.Log("WaitingForChangeWorld: world content is null");
                        }
                        //Hashtable content = response.Parameters[(byte)ParameterCode.WorldContent] as Hashtable;
                        //Debug.Log("at world entered: " + name);

                        worldData.SetWorldParameters(name, min, max, tileDimensions, levelType);
                        //if (content != null)
                        //{
                        //    //Debug.Log("content not null" + content.Count);
                        //    worldData.SetContent(content);
                        //}
                        game.SetStateWorldEntered(worldData);

                        return;
                }
            }
            else
            {
                switch ((OperationCode)response.OperationCode)
                {
                    case OperationCode.EnterWorld:
                        {
                            Operations.CreateWorld(game, game.World.Name);
                            return;
                        }
                }
            }
        }

        public void OnPeerStatusCallback(NetworkGame game, ExitGames.Client.Photon.StatusCode returnCode)
        {
            switch (returnCode)
            {
                case ExitGames.Client.Photon.StatusCode.Disconnect:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServer:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServerLogic:
                case ExitGames.Client.Photon.StatusCode.DisconnectByServerUserLimit:
                    {
                        game.SetDisconnected(returnCode);
                        break;
                    }
                default:
                    {
                        game.DebugReturn(DebugLevel.ERROR, returnCode.ToString());
                        break;
                    }
            }
        }

        public void OnUpdate(NetworkGame game)
        {
            game.Peer.Service();
        }

        public void SendOperation(NetworkGame game, Common.OperationCode operationCode, System.Collections.Generic.Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
        {
            game.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }
    }
}
