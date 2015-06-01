using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Game.Space;
using UButton = UnityEngine.UI.Button;
using UToggle = UnityEngine.UI.Toggle;
using Game.Network;
using Common;
using Nebula.Mmo.Games;

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
            foreach(var message in G.Game.Engine.GameData.Chat.Messages() ) {
                this.CreateTextForMessage(message);
            }
            this.startCompleted = true;
        }

        void OnEnable() {
            //subscribe for new chat message
            Events.NewChatMessageAdded += Events_NewChatMessageAdded;
        }

        void OnDisable() {
            //unsubscribe on disabled
            Events.NewChatMessageAdded -= Events_NewChatMessageAdded;
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

            if (messageText.StartsWith("$")) {
                this.ComputeCommand(messageText);
                return;
            }
            //simple case
            NRPC.SendChatMessage(this.SelectChatGroupView.SelectedChatGroup(), messageText, string.Empty);
        }

        private void ComputeCommand(string command) {
            switch (command.Trim().ToLower()) {
                case "$addore": {
                        NRPC.CmdAddOres();
                        G.Game.Engine.GameData.Chat.PastLocalMessage("Command: #add_ore");
                        break;
                    }
                case "$tgm": {
                        Operations.ExecAction(G.Game, G.Game.AvatarId, "ToggleGodMode", new object[] { });
                        //G.Game.Chat.PastLocalMessage("Command: #tgm");
                        break;
                    }
                case "$mtt":{
                        if(G.Game.Avatar == null || (!G.Game.Avatar.Target.HasTarget) || (G.Game.Avatar.Target.Item == null) ||
                            (G.Game.Avatar.Target.Item.View == null )) {
                            return;
                        }
                        Vector3 position = G.Game.Avatar.Target.Item.View.transform.position;
                        G.Game.Avatar.View.transform.position = position;
                        G.Game.Engine.GameData.Chat.PastLocalMessage("Command: #mtt");
                        break;
                    }
                case "$testbuffs":
                    {
                        NRPC.TestBuffs();
                        break;
                    }
                case "$addscheme": {
                        NRPC.EA_AddScheme();
                        break;
                    }
                case "$sselector":
                    {
                        MainCanvas.Get.Show(CanvasPanelType.SchemeSelectorView, CommonUtils.RandomSlotType());
                        break;
                    }
                case "$craft":
                    {
                        MainCanvas.Get.Show(CanvasPanelType.SchemeCraftView);
                        break;
                    }
                case "$addslots":
                    {
                        NRPC.AddInventorySlots();
                        break;
                    }
                case "$station":
                    {
                        G.Game.EnterWorkshop(WorkshopStrategyType.Angar);
                        break;
                    }
                case "$menu":
                    {
                        NRPC.ExitToSelectCharacterMenu();
                        break;
                    }
                case "$buff":
                    {
                        NetworkGame.SetRandomBuff();
                        break;
                    }
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
