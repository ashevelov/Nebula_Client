namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class GetPlayerStoreOperation : BaseOperationHandler {

        public override void Handle(BaseGame game, OperationResponse response) {
            if(!CheckSuccess(game, response)) {
                Debug.Log("error of receiving player store");
                return;
            }

            Hashtable storeHash = response.Parameters[(byte)ParameterCode.Info] as Hashtable;
            if(storeHash == null ) {
                Debug.Log("GetPlayerStoreOperation: store hash is null");
                return;
            }
            GameData.instance.store.ParseInfo(storeHash);
        }
    }
}
