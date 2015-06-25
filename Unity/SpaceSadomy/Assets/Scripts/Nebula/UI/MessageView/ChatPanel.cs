using Common;
using Nebula.Mmo.Games;
using Nebula.Mmo.Items.Components;
using Nebula.Resources;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UButton = UnityEngine.UI.Button;
using UToggle = UnityEngine.UI.Toggle;

namespace Nebula.UI {
    public class ChatPanel : BaseView {

        public Text TitleText;
        public UButton CloseButton;
        public ScrollRect Scroll;
        public UnityEngine.UI.InputField Input;
        public UButton SendButton;
        public UButton ChatChannelButton;
        public UToggle HideShowGraphicToggle;

        public GameObject TextPrefab;
        public GameObject Content;
        public SelectChatGroupView SelectChatGroupView;
        
        
        private bool startCompleted;

        void Start() {
            //populate list at start
            foreach(var message in G.Game.Engine.GameData.Chat.messages ) {
                this.CreateTextForMessage(message);
            }
            this.startCompleted = true;
        }

        void OnEnable() {
            //subscribe for new chat message
            Events.ChatMessageReceived += Events_NewChatMessageAdded;
        }

        void OnDisable() {
            //unsubscribe on disabled
            Events.ChatMessageReceived -= Events_NewChatMessageAdded;
        }

        private void CreateTextForMessage(ChatMessage message) {
            //save old scroll position
            float scrollPosBeforeAddingMessage = this.Scroll.verticalNormalizedPosition;
            //create text from prefab
            var obj = Instantiate(this.TextPrefab) as GameObject;
            //set text to message string
            obj.GetComponent<Text>().text = message.DecoratedMessage;
            obj.transform.SetParent(this.Content.transform, false);

            //if we were before message at end of scroll, set new scroll position to end of scroll
            if (Mathf.Approximately(scrollPosBeforeAddingMessage, 0f) || scrollPosBeforeAddingMessage < 0f) {
                this.Scroll.verticalNormalizedPosition = 0f;
            }
        }

        void Events_NewChatMessageAdded(ChatMessage message) {
            //don't add before start populate message
            if (!this.startCompleted) {
                return;
            }
            //if all ok add new message to list
            this.CreateTextForMessage(message);
        }

        public void OnSendButtonClick() {
            string messageText = this.Input.text;
            if (string.IsNullOrEmpty(messageText)) {
                return;
            }

            if(ComputeCommand(messageText)) {
                return;
            }

            if (SelectChatGroupView.SelectedChatGroup() != ChatGroup.whisper) {
                SelectCharacterGame.Instance().SendChatMessage(this.SelectChatGroupView.SelectedChatGroup(), messageText, new List<ChatLinkedObject>(), "", "");
            } else {
                if(G.PlayerItem.Target.Item == null ) {
                    Debug.Log("not set target");
                    return;
                }
                var targetCharacter = G.PlayerItem.Target.Item.GetMmoComponent(ComponentID.Character) as MmoCharacterComponent;

                if(targetCharacter == null ) {
                    Debug.Log("invalid character");
                    return;
                }
                if(string.IsNullOrEmpty(targetCharacter.login) || string.IsNullOrEmpty(targetCharacter.characterID)) {
                    Debug.Log("invalid character ID or login");
                    return;
                }
                SelectCharacterGame.Instance().SendChatMessage(ChatGroup.whisper, messageText, new List<ChatLinkedObject>(), targetCharacter.login, targetCharacter.characterID);
            }
        }

        private bool ComputeCommand(string command) {
            switch (command.Trim().ToLower()) {
                case "$addore": {
                        NRPC.CmdAddOres();
                        G.Game.Engine.GameData.Chat.PastLocalMessage("Command: #add_ore");
                        return true;
                    }
                case "$tgm": {
                        Operations.ExecAction(G.Game, G.Game.AvatarId, "TGM", new object[] { });
                        //G.Game.Chat.PastLocalMessage("Command: #tgm");
                        return true;
                    }
                case "$mtt":{
                        if(G.Game.Avatar == null || (!G.Game.Avatar.Target.HasTarget) || (G.Game.Avatar.Target.Item == null) ||
                            (G.Game.Avatar.Target.Item.View == null )) {
                            return true;
                        }
                        Vector3 position = G.Game.Avatar.Target.Item.View.transform.position;
                        G.Game.Avatar.View.transform.position = position;
                        G.Game.Engine.GameData.Chat.PastLocalMessage("Command: #mtt");
                        return true;
                    }
                case "$testbuffs":
                    {
                        NRPC.TestBuffs();
                        return true;
                    }
                case "$addscheme": {
                        NRPC.EA_AddScheme();
                        return true;
                    }
                case "$sselector":
                    {
                        MainCanvas.Get.Show(CanvasPanelType.SchemeSelectorView, CommonUtils.RandomSlotType());
                        return true;
                    }
                case "$craft":
                    {
                        MainCanvas.Get.Show(CanvasPanelType.SchemeCraftView);
                        return true;
                    }
                case "$addslots":
                    {
                        NRPC.AddInventorySlots();
                        return true;
                    }
                case "$station":
                    {
                        G.Game.EnterWorkshop(WorkshopStrategyType.Angar);
                        return true;
                    }
                case "$menu":
                    {
                        NRPC.ExitToSelectCharacterMenu();
                        return true;
                    }
                case "$buff":
                    {
                        NetworkGame.SetRandomBuff();
                        return true;
                    }
                case "notify": { SelectCharacterGame.Instance().InvokeMethod("SetYesNoNotification", null);
                        return true;
                    }
                default:
                    return false;
            }
        }

        public void OnHideShowGraphicToggleValueChanged() {
            //show hide chat graphoc, remain visible only text
            if (this.HideShowGraphicToggle.isOn) {
                this.ToggleChatGraphic(false);
            } else {
                this.ToggleChatGraphic(true);
            }
        }

        public void OnChatChannelButtonClicked() {
            if (this.SelectChatGroupView.Visible()) {
                this.SelectChatGroupView.Hide();
            } else {
                this.SelectChatGroupView.Show(()=> {
                    Debug.LogFormat("selected chat group = {0}", SelectChatGroupView.SelectedChatGroup());
                    if(StringCache.ChatGroupName(this.SelectChatGroupView.SelectedChatGroup()) == null ) {
                        Debug.Log("Not founded chat group name string");
                    }
                    if(ChatChannelButton == null ) {
                        Debug.Log("Chat Channel Button null");
                    }
                    if(ChatChannelButton.GetComponentInChildren<Text>() == null ) {
                        Debug.Log("text component is null");
                    }
                    this.ChatChannelButton.GetComponentInChildren<Text>().text = StringCache.ChatGroupName(this.SelectChatGroupView.SelectedChatGroup()); //update text for selected chat channel
                });
            }
        }

        public void OnCloseButtonClicked() {
            MainCanvas.Get.Destroy(CanvasPanelType.ChatView);
        }

        //Show or hide graphic of chat, not affect chat text
        public void ToggleChatGraphic(bool setVisible) {
            if (!setVisible) {
                this.GetComponent<Image>().enabled = false;
                this.TitleText.gameObject.SetActive(false);
                this.CloseButton.gameObject.SetActive(false);
                this.Input.gameObject.SetActive(false);
                this.ChatChannelButton.gameObject.SetActive(false);
                this.SendButton.gameObject.SetActive(false);
            } else {
                this.GetComponent<Image>().enabled = true;
                this.TitleText.gameObject.SetActive(true);
                this.CloseButton.gameObject.SetActive(true);
                this.Input.gameObject.SetActive(true);
                this.ChatChannelButton.gameObject.SetActive(true);
                this.SendButton.gameObject.SetActive(true);
            }
        }

        

    }
}
