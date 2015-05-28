namespace Nebula {
    using Common;
    using ExitGames.Client.Photon;
    using Game.Network;
    using Nebula.Client.Inventory;
    using Nebula.Client.Mail;
    using Nebula.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Nebula.UI;


    public class GenericEventMailBoxUpdatedStrategy : GenericEventStrategy, IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            Hashtable mailBoxInfo = Properties(eventData);
            G.Game.MailBox().Replace(new ClientMailBox(mailBoxInfo));
            Debug.Log("<color=orange>Mail message received</color>" + G.Game.MailBox().ToString());
        }
    }

    public class WorldEnteredGenericGroupStrategy : GenericEventStrategy, IServerEventStrategy {

        private readonly Dictionary<CustomEventCode, System.Action<NetworkGame, EventData>> handlers;

        public WorldEnteredGenericGroupStrategy() {
            this.handlers = new Dictionary<CustomEventCode, System.Action<NetworkGame, EventData>>();
            this.handlers.Add(CustomEventCode.CooperativeGroupRequest, HandleCooperativeGroupRequest);
            this.handlers.Add(CustomEventCode.CooperativeGroupUpdate, HandleCooperativeGroupUpdate);
        }

        public void Handle(NetworkGame game, EventData eventData) {
            CustomEventCode code = (CustomEventCode)(byte)eventData.Parameters[(byte)ParameterCode.CustomEventCode];
            if (this.handlers.ContainsKey(code)) {
                this.handlers[code](game, eventData);
            }
        }

        private void HandleCooperativeGroupRequest(NetworkGame game, EventData eventData ) {
            //here show msg box
            Hashtable requestInfo = Properties(eventData);
            Hashtable requestData = requestInfo.GetValue<Hashtable>(GenericEventProps.data, null);
            GroupActionRequestType requestType = (GroupActionRequestType)requestInfo.GetValue<byte>(GenericEventProps.request_type, (byte)GroupActionRequestType.None);

            if(requestData == null ) {
                Debug.LogError("Request Data is null");
                return;
            }



            if (requestType == GroupActionRequestType.InviteToExistingGroup || requestType == GroupActionRequestType.InviteToNewGroup) {

                string inviterName = requestData.GetValue<string>(GenericEventProps.display_name, null);

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

            } else if(requestType == GroupActionRequestType.ExcludePlayerFromGroup) {

                string fromName = requestData.GetValue<string>(GenericEventProps.from_dispay_name, string.Empty);
                string excludeName = requestData.GetValue<string>(GenericEventProps.to_exclude_display_name, string.Empty);

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
            }
            else {
                Debug.LogErrorFormat("Unhandled request type: {0}", requestType);
            }
        }

        private void HandleCooperativeGroupUpdate(NetworkGame game, EventData eventData ) {

            Hashtable groupInfo = Properties(eventData);

            if(groupInfo != null ) {
                G.Game.CooperativeGroup().ParseInfo(groupInfo);
                Events.EvtCooperativeGroupUpdated(G.Game.CooperativeGroup());
            } else {
                Debug.Log("Cooperative group info is null");
            }
        }
    }
}