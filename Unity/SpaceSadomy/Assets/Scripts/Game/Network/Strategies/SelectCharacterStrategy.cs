
using Common;
using ExitGames.Client.Photon;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nebula
{
    public class SelectCharacterStrategy : IGameLogicStrategy
    {

        public static readonly SelectCharacterStrategy Instance = new SelectCharacterStrategy();

        public GameState State
        {
            get { return GameState.SelectCharacter; }
        }

        public void OnEventReceive(NetworkGame game, EventData eventData)
        {
            Dbg.Print("SelectCharacterStrategy event received: {0}".f(eventData.Code.toEnum<EventCode>()));
            game.OnUnexpectedEventReceive(eventData);
        }

        private readonly Dictionary<OperationCode, OperationReturnStrategy> operationStrategies;

        public SelectCharacterStrategy() {
            this.operationStrategies = new Dictionary<OperationCode, OperationReturnStrategy> { 
                {OperationCode.SelectCharacter, new SelectCharacterStrategy.SelectCharacterOperationReturnStrategy() },
                {OperationCode.AddCharacter, new SelectCharacterStrategy.AddCharacterOperationReturnStrategy() },
                {OperationCode.CreateWorld, new SelectCharacterStrategy.CreateWorldOperationReturnStrategy() },
                {OperationCode.EnterWorld, new SelectCharacterStrategy.EnterWorldOperationReturnStrategy() },
                {OperationCode.ExecAction, new SelectCharacterStrategy.RPCGroupOperationReturnStrategy() }
            };
        }

        public void OnOperationReturn(NetworkGame game, OperationResponse response)
        {
            OperationCode code = (OperationCode)response.OperationCode;
            if (this.operationStrategies.ContainsKey(code)) {
                this.operationStrategies[code].Handle(game, response);
            } else {
                Debug.Log("SelectCharacterStrategy not contains handler for {0}".f(code));
            }
            #region Old Code
            /*
            if (response.ReturnCode == 0)
            {
                switch (response.OperationCode.toEnum<OperationCode>())
                {
                    case OperationCode.SelectCharacter:
                        {
                            bool success = (bool)response.Parameters[ParameterCode.Status.toByte()];
                            Hashtable userInfo = (Hashtable)response.Parameters[ParameterCode.Info.toByte()];

                            if (userInfo != null)
                            {
                                game.UserInfo.ParseInfo(userInfo);
                            }
                            if (success)
                            {
                            }
                        }
                        break;
                    case OperationCode.AddCharacter:
                        {
                            bool success = (bool)response.Parameters[ParameterCode.Status.toByte()];
                            Hashtable userInfo = (Hashtable)response.Parameters[ParameterCode.Info.toByte()];
                            string characterId = (string)response.Parameters[ParameterCode.CharacterId.toByte()];
                            if (userInfo != null)
                            {
                                game.UserInfo.ParseInfo(userInfo);
                            }
                            if (success)
                            {

                            }
                        }
                        break;
                    case OperationCode.CreateWorld:
                        {
                            game.Avatar.EnterWorld();
                        }
                        break;
                    case OperationCode.EnterWorld:
                        {
                            var worldData = new WorldData();
                            string name = (string)response.Parameters[(byte)ParameterCode.WorldName];
                            Vector3 max = ((float[])response.Parameters[(byte)ParameterCode.BottomRightCorner]).toVector();
                            Vector3 min = ((float[])response.Parameters[(byte)ParameterCode.TopLeftCorner]).toVector();
                            Vector3 tileDimensions = ((float[])response.Parameters[(byte)ParameterCode.TileDimensions]).toVector();
                            LevelType levelType = (LevelType)(byte)response.Parameters[(byte)ParameterCode.LevelType];
                            Hashtable clientWorld = response.Parameters[ParameterCode.WorldContent.toByte()] as Hashtable;
                            if (clientWorld != null)
                            {
                                game.ClientWorld.ParseInfo(clientWorld);
                                clientWorld.Print(1);
                            }
                            else
                            {
                                Debug.Log("Client world is null");
                            }
                            worldData.SetWorldParameters(name, min, max, tileDimensions, levelType);
                            game.SetStateWorldEntered(worldData);
                        }
                        break;
                    case OperationCode.ExecAction:
                        {
                            string name = (string)response.Parameters[ParameterCode.Action.toByte()];
                            Hashtable result = (Hashtable)response.Parameters[ParameterCode.Result.toByte()];
                            switch (name)
                            {
                                case "GetUserInfo":
                                    {
                                        Dbg.Print("user info received");
                                        game.UserInfo.ParseInfo(result);
                                        foreach (var ch in game.UserInfo.Characters)
                                        {
                                            //Debug.Log("----");
                                            foreach (var m in ch.Model)
                                            {
                                                //Debug.Log("{0}:{1}".f( m.Key, m.Value ) );
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    Debug.Log("unsupported action name: {0} in SelectCharacterStategy".f(name));
                                    break;
                            }
                        }
                        break;
                }

                Dbg.Print("SelectCharacterStrategy-> operation received: {0}".f(response.OperationCode.toEnum<OperationCode>()), "PLAYER");
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
                    default:
                        {
                            //Debug.Log("{0} error: {1}".f(response.OperationCode.toEnum<OperationCode>(), response.DebugMessage));
                            return;
                        }
                }
                
            }*/
            
            #endregion
        }

        public void OnPeerStatusCallback(NetworkGame game, StatusCode returnCode)
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

        public void SendOperation(NetworkGame game, OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
        {
            game.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);
        }

        #region Operations Strategies
        public class SelectCharacterOperationReturnStrategy : OperationReturnStrategy {
            public override void Handle(NetworkGame game, OperationResponse response) {
                base.Handle(game, response);
                bool success = (bool)response.Parameters[ParameterCode.Status.toByte()];
                Hashtable userInfo = (Hashtable)response.Parameters[ParameterCode.Info.toByte()];
                if (userInfo == null) {
                    Debug.LogError("SelectCharacterOperationReturnStrategy.Handle(): User info is (null)");
                    return;
                }
                game.UserInfo.ParseInfo(userInfo);
            }
        }

        public class AddCharacterOperationReturnStrategy : OperationReturnStrategy {
            public override void Handle(NetworkGame game, OperationResponse response) {
                base.Handle(game, response);
                bool success = (bool)response.Parameters[ParameterCode.Status.toByte()];
                Hashtable userInfo = (Hashtable)response.Parameters[ParameterCode.Info.toByte()];
                string characterId = (string)response.Parameters[ParameterCode.CharacterId.toByte()];
                if (userInfo == null) {
                    Debug.LogError("AddCharacterOperationReturnStrategy.Handle(): User info is (null)");
                    return;
                }
                game.UserInfo.ParseInfo(userInfo);
            }
        }

        public class CreateWorldOperationReturnStrategy : OperationReturnStrategy {
            public override void Handle(NetworkGame game, OperationResponse response) {
                base.Handle(game, response);
                game.OperationEnterWorld();
            }
        }

        public class EnterWorldOperationReturnStrategy : OperationReturnStrategy {
            public override void Handle(NetworkGame game, OperationResponse response) {
                base.Handle(game, response);

                if (response.ReturnCode == (short)ReturnCode.Ok) {
                    var worldData = new WorldData();
                    string name = (string)response.Parameters[(byte)ParameterCode.WorldName];
                    Vector3 max = ((float[])response.Parameters[(byte)ParameterCode.BottomRightCorner]).toVector();
                    Vector3 min = ((float[])response.Parameters[(byte)ParameterCode.TopLeftCorner]).toVector();
                    Vector3 tileDimensions = ((float[])response.Parameters[(byte)ParameterCode.TileDimensions]).toVector();
                    LevelType levelType = (LevelType)(byte)response.Parameters[(byte)ParameterCode.LevelType];
                    Hashtable clientWorld = response.Parameters[ParameterCode.WorldContent.toByte()] as Hashtable;

                    if (clientWorld != null) {
                        game.ClientWorld.ParseInfo(clientWorld);
                        clientWorld.Print(1);
                    } else {
                        Debug.Log("Client world is null");
                    }
                    worldData.SetWorldParameters(name, min, max, tileDimensions, levelType);
                    game.SetStateWorldEntered(worldData);
                } else {
                    Operations.CreateWorld(game, game.World.Name);
                }
            }
        }

        public class RPCGroupOperationReturnStrategy : RPCOperationReturnStrategy {

            private readonly Dictionary<string, System.Action<NetworkGame, OperationResponse>> handlers;

            public RPCGroupOperationReturnStrategy() {
                this.handlers = new Dictionary<string, System.Action<NetworkGame, OperationResponse>>{
                    {"GetUserInfo", HandleGetUserInfo},
                    {"DeleteCharacter", HandleDeleteCharacter }
                };
            }

            public override void Handle(NetworkGame game, OperationResponse response) {
                string actionName = this.Action(response);
                if (this.handlers.ContainsKey(actionName)) {
                    this.handlers[actionName](game, response);
                }
            }

            private void HandleGetUserInfo(NetworkGame game, OperationResponse response) {
                Debug.Log("RPCGroupOperationReturnStrategy.HandleGetUserInfo(): user info received");
                game.UserInfo.ParseInfo(this.Result(response));
            }

            private void HandleDeleteCharacter(NetworkGame game, OperationResponse response) {
                Debug.Log("RPCGroupOperationReturnStrategy.HandleDeleteCharacter: delete character response");

                //if bad response print error and exit
                if (Status(response) == ACTION_RESULT.FAIL) {
                    this.PrintRPCError(response);
                    return;
                }

                var ret = this.Return(response);
                if (ret == null) {
                    Debug.LogError("SelectCharacterStrategy.HandleDeleteCharacter: Return null user info");
                    return;
                }

                var retHash = ret as Hashtable;
                if (retHash == null) {
                    Debug.LogError("SelectCharacterStrategy.HandleDeleteCharacter: Return non hash user info");
                    return;
                }

                game.UserInfo.ParseInfo(retHash);
            }
        } 
        #endregion
    }
}
