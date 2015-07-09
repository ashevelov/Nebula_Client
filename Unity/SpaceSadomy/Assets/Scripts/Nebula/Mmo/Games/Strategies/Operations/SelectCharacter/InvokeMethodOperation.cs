﻿namespace Nebula.Mmo.Games.Strategies.Operations.SelectCharacter {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using Common;

    public class InvokeMethodOperation : BaseOperationHandler {

        private readonly Dictionary<string, System.Action<SelectCharacterGame, OperationResponse>> mHandlers;

        public InvokeMethodOperation() {
            mHandlers = new Dictionary<string, System.Action<SelectCharacterGame, OperationResponse>>();
            mHandlers.Add("SetYesNoNotification", HandleSetYesNoNotification);
            mHandlers.Add("FindGuilds", HandleFindGuilds);
            mHandlers.Add("InviteToGroup", HandleInviteToGroup);
            mHandlers.Add("RequestToGroup", HandleRequestToGroup);
            mHandlers.Add("SetGroupOpened", HandleSetGroupOpened);
            mHandlers.Add("PutToAuction", HandlePutToAuction);
            mHandlers.Add("GetCurrentAuctionPage", HandleGetCurrentAuctionPage);
            mHandlers.Add("GetNextAuctionPage", HandleGetNextAuctionPage);
            mHandlers.Add("GetPrevAuctionPage", HandleGetPrevAuctionPage);
        }

        public override void Handle(BaseGame game, OperationResponse response) {
            string methodName = (string)response.Parameters[(byte)ParameterCode.Action];
            SelectCharacterGame scgame = game as SelectCharacterGame;
            if(mHandlers.ContainsKey(methodName)) {
                mHandlers[methodName](scgame, response);
            }
        }

        private void HandleGetCurrentAuctionPage(SelectCharacterGame game, OperationResponse response) {
            Hashtable ret = ReturnValue<Hashtable>(response);
            GameData.instance.auction.ParseInfo(ret);
        }

        private void HandleGetNextAuctionPage(SelectCharacterGame game, OperationResponse response) {
            Hashtable ret = ReturnValue<Hashtable>(response);
            GameData.instance.auction.ParseInfo(ret);
        }

        private void HandleGetPrevAuctionPage(SelectCharacterGame game, OperationResponse response) {
            Hashtable ret = ReturnValue<Hashtable>(response);
            GameData.instance.auction.ParseInfo(ret);
        }

        private void HandlePutToAuction(SelectCharacterGame game, OperationResponse response) {
            bool success = ReturnValue<bool>(response);
            Debug.LogFormat("HandlePutToAuction: return value = {0}", success);
        }

        private void HandleSetYesNoNotification(SelectCharacterGame game, OperationResponse response) {
            Debug.LogFormat("SetYesNoNotification invoke completed with reurn = {0}", ReturnValue<int>(response));
        }

        private void HandleFindGuilds(SelectCharacterGame game, OperationResponse response) {
            Hashtable guilds = ReturnValue<Hashtable>(response);
            if(guilds == null ) {
                Debug.Log("return null guilds");
                return;
            }
            Debug.LogFormat("found {0} guilds", guilds.Count);
        }

        private void HandleInviteToGroup(SelectCharacterGame game, OperationResponse response) {
            ReturnCode returnCode = (ReturnCode)ReturnValue<int>(response);
            Debug.LogFormat("Invite to group return code = {0}", returnCode);
        }

        private void HandleRequestToGroup(SelectCharacterGame game, OperationResponse response) {
            ReturnCode returnCode = (ReturnCode)ReturnValue<int>(response);
            Debug.LogFormat("Request To Group return code = {0}", returnCode);
        }

        private void HandleSetGroupOpened(SelectCharacterGame game, OperationResponse response) {
            ReturnCode returnCode = (ReturnCode)ReturnValue<int>(response);
            Debug.LogFormat("Set Group Opened return code = {0}", returnCode);
        }

        private T ReturnValue<T>(OperationResponse response) {
            return (T)response.Parameters[(byte)ParameterCode.Result];
        }
    }
}
