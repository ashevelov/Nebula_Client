using Common;
using ExitGames.Client.Photon;
using System.Collections;
using UnityEngine;

namespace Nebula {

    public class Login : IGameLogicStrategy {

        public static readonly IGameLogicStrategy Instance = new Login();

        public GameState State
        {
            get {
                return GameState.Login;
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
                    case OperationCode.Login:
                        {
                            bool status = response.Parameters.GetValue<bool>(ParameterCode.Status);
                            string gameRefId = response.Parameters.GetValue<string>(ParameterCode.GameRefId);
                            string displayName = response.Parameters.GetValue<string>(ParameterCode.Username);
                            string loginName = response.Parameters.GetValue<string>(ParameterCode.Login);
                            string password = response.Parameters.GetValue<string>(ParameterCode.Password);

                            LoginInfo loginInfo = new LoginInfo { displayName = displayName, gameRefId = gameRefId, status = status, loginName = loginName, password = password};

                            game.OnLoginCompleted(loginInfo);
                        }
                        return;
                    case OperationCode.CreateWorld:
                        {
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
                        }
                        else
                        {
                            Debug.Log("Login: world content is null");
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
            else {
                switch ((OperationCode)response.OperationCode)
                {
                    case OperationCode.EnterWorld:
                        {
                            Operations.CreateWorld(game, game.World.Name);
                            return;
                        }
                    case OperationCode.Login:
                        {
                            Debug.LogError("Login error: " + response.DebugMessage);
                        }
                        break;
                }
            }
        }

        public void OnPeerStatusCallback(NetworkGame game, ExitGames.Client.Photon.StatusCode returnCode)
        {
            switch (returnCode) {
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
