namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Common;
    using ExitGames.Client.Photon;
    using ServerClientCommon;
    using Nebula.UI;
    using System;

    public class GenericMultiEvent : BaseGenericEvent {

        private readonly Dictionary<CustomEventCode, System.Action<BaseGame, EventData>> handlers;

        public GenericMultiEvent() {
            this.handlers = new Dictionary<CustomEventCode, System.Action<BaseGame, EventData>>();
            this.handlers.Add(CustomEventCode.CooperativeGroupRequest, HandleCooperativeGroupRequest);
            this.handlers.Add(CustomEventCode.CooperativeGroupUpdate, HandleCooperativeGroupUpdate);
        }

        public override void Handle(BaseGame game, EventData eventData) {
            CustomEventCode code = (CustomEventCode)(byte)eventData.Parameters[(byte)ParameterCode.CustomEventCode];
            if (this.handlers.ContainsKey(code)) {
                this.handlers[code](game, eventData);
            }
        }

        private void HandleCooperativeGroupRequest(BaseGame game, EventData eventData) {
            //here show msg box
            Hashtable requestInfo = Properties(eventData);
            Hashtable requestData = requestInfo.GetValue<Hashtable>((int)SPC.Data, null);
            GroupActionRequestType requestType = (GroupActionRequestType)requestInfo.GetValue<byte>((int)SPC.RequestType, (byte)GroupActionRequestType.None);

            if (requestData == null) {
                Debug.LogError("Request Data is null");
                return;
            }



            if (requestType == GroupActionRequestType.InviteToExistingGroup || requestType == GroupActionRequestType.InviteToNewGroup) {

                string inviterName = requestData.GetValue<string>((int)SPC.DisplayName, null);

                if (inviterName == null) {
                    Debug.LogError("Inviter name null");
                    return;
                }

                MessageBoxView.MessageBoxConfig config = new MessageBoxView.MessageBoxConfig {
                    ButtonActions = new Action[] {
                     () => {
                            NRPC.ResponseToGroupRequest(true, requestInfo);
                        },
                     () => {
                            NRPC.ResponseToGroupRequest(false, requestInfo);
                        }
                    },
                    ButtonNames = new string[] { "Join", "Decline" },
                    CountOfButtons = 2,
                    HasImage = false,
                    Text = string.Format("Игрок {0} приглашает вас присоединиться к группе. Хотите вступить в группу?", inviterName),
                    Title = string.Empty
                };
                MainCanvas.Get.Show(CanvasPanelType.MessageBox, config);

            } else if (requestType == GroupActionRequestType.ExcludePlayerFromGroup) {

                string fromName = requestData.GetValue<string>((int)SPC.FromDisplayName, string.Empty);
                string excludeName = requestData.GetValue<string>((int)SPC.ToExcludeDisplayName, string.Empty);

                MessageBoxView.MessageBoxConfig config = new MessageBoxView.MessageBoxConfig {
                    ButtonActions = new Action[] {
                     () => {
                            NRPC.ResponseToGroupRequest(true, requestInfo);
                        },
                     () => {
                            NRPC.ResponseToGroupRequest(false, requestInfo);
                        }
                    },
                    ButtonNames = new string[] { "Exclude", "No" },
                    CountOfButtons = 2,
                    HasImage = false,
                    Text = string.Format("Игрок {0} предлагает исключить из группы игрока {1}. Вы согласны?", fromName, excludeName),
                    Title = string.Empty
                };
                MainCanvas.Get.Show(CanvasPanelType.MessageBox, config);
            } else {
                Debug.LogErrorFormat("Unhandled request type: {0}", requestType);
            }
        }

        private void HandleCooperativeGroupUpdate(BaseGame game, EventData eventData) {

            Hashtable groupInfo = Properties(eventData);

            if (groupInfo != null) {
                G.Game.CooperativeGroup().ParseInfo(groupInfo);
                global::Nebula.Events.EvtCooperativeGroupUpdated(G.Game.CooperativeGroup());
            } else {
                Debug.Log("Cooperative group info is null");
            }
        }
    }
}
