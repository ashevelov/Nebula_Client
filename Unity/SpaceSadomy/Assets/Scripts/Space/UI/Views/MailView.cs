/*
using UnityEngine;
using System.Collections;
using Nebula.Client.Mail;
using Game.Network;
using System.Collections.Generic;
using Common;
using Nebula.Client;

namespace Game.Space.UI
{
    public class MailView : BaseContainerView
    {
        private UIScrollView mailListScroll;
        private UIScrollView attachmentsScroll;
        private ClientMailMessage selectedMessage;
        private UILabel detailTitleLabel;
        private UILabel detailBodyLabel;

        public MailView(NetworkGame game, UIManager manager, string rootName ) : base(game, manager, rootName) {
            this.Root.RegisterUpdate(Update);
            this.mailListScroll = this.Root.GetChild<UIScrollView>("mail_list_scroll");
            this.attachmentsScroll = this.Root.GetChild<UIScrollView>("attachments_scroll");

            this.detailTitleLabel = this.Root.GetChild<UILabel>("mail_message_title");
            this.detailBodyLabel = this.Root.GetChild<UILabel>("mail_message_body");
        }

        private void UpdateSelectedMessage() {
            if (this.selectedMessage != null) {
                this.selectedMessage = G.Game.MailBox().Message(this.selectedMessage.ID());
            }
        }

        public class MailMessageDateComparer : IComparer<ClientMailMessage> {

            public int Compare(ClientMailMessage x, ClientMailMessage y) {
                if (x == null) {
                    if (y == null) {
                        return 0;
                    } else {
                        return 1;
                    }
                } else {
                    if (y == null) {
                        return -1;
                    } else {
                        return -(x.Time().CompareTo(y.Time()));
                    }
                }
            }
        }

        public class AttachmentIdComparer : IComparer<ClientAttachment> {
            public int Compare(ClientAttachment x, ClientAttachment y) {
                return x.ID().CompareTo(y.ID());
            }
        }

        private void SortListByDateDescending(List<ClientMailMessage> inputMessages) {
            inputMessages.Sort(new MailMessageDateComparer()); 
        }

        private void SortAttachmentListById(List<ClientAttachment> inputAttachments) {
            inputAttachments.Sort(new AttachmentIdComparer());
        }

        private void Update(UIContainerEntity container) {
            if (G.Game == null) {
                return;
            }
            if (G.Game.MailBox() == null) {
                return;
            }

            List<ClientMailMessage> orderedMessagesByDate = new List<ClientMailMessage>();
            orderedMessagesByDate.AddRange(G.Game.MailBox().Messages());
            this.SortListByDateDescending(orderedMessagesByDate);
            this.UpdateSelectedMessage();

            FillMailListScroll(orderedMessagesByDate);

            if (selectedMessage != null) {
                var attachments = new List<ClientAttachment>();
                attachments.AddRange(selectedMessage.Attachments().Attachments());
                this.SortAttachmentListById(attachments);
                if (attachments.Count > 0) {
                    Debug.Log("Fill {0} attachments".f(attachments.Count));
                }
                FillAttachments(attachments);
            }
        }

        private void FillMailListScroll(List<ClientMailMessage> messages) {

            this.mailListScroll.Clear();

            foreach (ClientMailMessage message in messages) {
                var template = this.mailListScroll.CreateElement();
                
                var senderNameLabel = template.GetChild<UILabel>("sender_name_label");
                senderNameLabel.SetText(message.SenderGameRefID());

                var timeLabel = template.GetChild<UILabel>("time_label");
                timeLabel.SetText(message.Time().ToString());

                var messageTitleLabel = template.GetChild<UILabel>("message_title");
                messageTitleLabel.SetText(message.Title());

                if (!IsImSender(message)) {
                    messageTitleLabel.SetUseCustomColor(true, Color.green);
                }

                var notReadedTexture = template.GetChild<UITexture>("not_readed_texture");
                notReadedTexture.SetVisibility( !message.Readed() );

                var selectionToggle = template.GetChild<UIToggle>("message_toggle");

                if (selectedMessage != null) {
                    selectionToggle._value = (message.ID() == selectedMessage.ID());
                } else {
                    selectionToggle._value = false;
                }

                selectionToggle.SetTag(message);

                selectionToggle.RegisterHandler(evt => {
                    if ((bool)evt._parameters["new_value"]) {
                        var clickedMessage = evt._sender.tag as ClientMailMessage;
                        selectedMessage = clickedMessage;
                        if (!clickedMessage.Readed()) {
                            //request mark as readed
                        }
                        this.detailTitleLabel.SetText(clickedMessage.Title());
                        this.detailBodyLabel.SetText(clickedMessage.Body());
                    }
                });

                selectionToggle.RegisterLocalHandler(UIEventType.RightClick, "DELETE_MESSAGE", (sender) => {
                    Debug.Log("Right click on toggle handled");
                    var msg = sender.tag as ClientMailMessage;
                    if (msg != null) {
                        G.Game.RemoveMailMessage(msg.ID());
                    }
                });


                this.mailListScroll.AddChild(template);
            }
        }

        private void FillAttachments(List<ClientAttachment> attachments) {
            this.attachmentsScroll.Clear();
            attachments.ForEach(attachment => {
                var template = this.attachmentsScroll.CreateElement();

                var countLabel = template.GetChild<UILabel>("count_label");
                countLabel.SetText(AttachmentCount(attachment).ToString());

                var iconButton = template.GetChild<UIButton>("icon_button");
                GUIStyle newStyle = new GUIStyle(iconButton._style);
                iconButton._style = newStyle;
                newStyle.SetStyleTexture(AttachmentTexture(attachment));

                iconButton.SetTag(attachment);

                iconButton.RegisterHandler((evt) => {
                    Debug.Log("left click handler called");
                    if (selectedMessage == null) {
                        return;
                    }
                    if (IsImSender(selectedMessage)) {
                        return;
                    }

                    var a = evt._sender.tag as ClientAttachment;
                    //make request take attachment
                    G.Game.TakeMailAttachment(selectedMessage.ID(), a.ID());
                });



                this.attachmentsScroll.AddChild(template);
            });
        }

        private Texture2D AttachmentTexture(ClientAttachment attachment) {
            switch (attachment.Type()) {
                case AttachmentType.InventoryObject: {
                    return TextureCache.TextureForItem(((ClientInventoryObjectAttachment)attachment).AttachedObject());
                    }
                case AttachmentType.ShipModule: {
                    return TextureCache.TextureForModule(((ClientShipModuleAttachment)attachment).AttachedObject());
                    }
                default:
                    throw new NebulaException("Not exists texture for attachment type: {0}".f(attachment.Type()));
            }
        }

        private int AttachmentCount(ClientAttachment attachment) {
            switch (attachment.Type()) {
                case AttachmentType.InventoryObject: {
                    return ((ClientInventoryObjectAttachment)attachment).Count();
                    }
                case AttachmentType.ShipModule: {
                    return ((ClientShipModuleAttachment)attachment).Count();
                    }
                default: {
                    throw new NebulaException("Not supported attachment type: {0}".f(attachment.Type()));
                    }
            }
        }

        private bool IsImSender(ClientMailMessage message) {
            return (G.Game.UserInfo.GameRefId == message.SenderGameRefID());
        }


    }
}
*/