using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using Common;
using Nebula.Client.Servers;
using ServerClientCommon;

namespace Nebula.Mmo.Games.Strategies.Operations.Master {
    public class GetServerListOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(response.ReturnCode != (short)ReturnCode.Ok) {
                Debug.LogErrorFormat("error, return code = {0}, debug message = {1}", (ReturnCode)response.ReturnCode, response.DebugMessage);
                return;
            }

            Hashtable serverListHash = response.Parameters[(byte)ParameterCode.ServerList] as Hashtable;

            if(serverListHash == null ) {
                Debug.LogError("server list null");
            }

            ClientServerCollection serverCollection = new ClientServerCollection();
            
            foreach(DictionaryEntry serverHash in serverListHash) {
                Hashtable srv = serverHash.Value as Hashtable;
                if(srv == null) {
                    continue;
                }
                ServerInfo serverInfo = new ServerInfo();
                serverInfo.ParseInfo(srv);
                serverCollection.Add(serverInfo);
            }

            game.Engine.OnServersReceived(serverCollection);
        }
    }
}
