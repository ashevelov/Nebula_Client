namespace Nebula
{
    using System;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using Common;
    using UnityEngine;


    [CLSCompliant(false)]
    public class Connected : IGameLogicStrategy
    {
        public static readonly IGameLogicStrategy Instance = new Connected();
        public GameState State
        {
            get
            {
                return GameState.Connected;
            }
        }


        public void OnEventReceive(NetworkGame game, EventData eventData)
        {
            game.OnUnexpectedEventReceive(eventData);
        }

        public void OnOperationReturn(NetworkGame game, OperationResponse response)
        {
            Debug.Log("ON CONNECTED OPERATIN RETURN WITH CODE: " + (OperationCode)response.OperationCode);
            if (response.ReturnCode == 0)
            {
                switch ((OperationCode)response.OperationCode)
                {

                }
            }
            else
            {

            }
            game.OnUnexpectedOperationError(response);
        }

        public void OnPeerStatusCallback(NetworkGame game, StatusCode returnCode)
        {
            switch (returnCode)
            {
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.TimeoutDisconnect:
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
            //Debug.Log("CONNECTED OnUpdate() called");
            game.Peer.Service();
        }

        public void SendOperation(NetworkGame game, OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
        {
            Debug.Log("SEND OPERATION on CONNECTED STATE");
            game.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }
    }
}