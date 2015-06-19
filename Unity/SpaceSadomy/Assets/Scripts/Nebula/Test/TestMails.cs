namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Nebula.Client.Mail;
    using Common;
    using System.Collections.Generic;
    using Nebula.Client.Inventory;
    using Nebula.Mmo.Games;
    using ServerClientCommon;

    public class TestMails : MonoBehaviour {

        public GUISkin skin;

        private GUIStyle mLabelStyle;
        private string mTargetLogin = "";
        private string mTitle = "";
        private string mBody = "";
        private bool mVisible = false;

        private List<IInventoryObjectInfo> mAttachments = new List<IInventoryObjectInfo>();

        void Start() {
            mLabelStyle = skin.GetStyle("font_upper_left");
        }

        void Update() {
            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                Debug.Log("Clicked Alpha1");
                mVisible = !mVisible;
                if (mVisible) {
                    SelectCharacterGame.Instance().GetMails();
                }
            }
        }


        void OnGUI() {
            if (mVisible) {
                var mailBox = MmoEngine.Get.GameData.mailBox;
                if (mailBox == null) {
                    return;
                }
                if (mailBox.messages == null) {
                    return;
                }

                float winWidth = Screen.width - 20;
                float winHeight = Screen.height - 20;
                Rect windowRect = new Rect(10, 10, winWidth, winHeight);
                GUI.BeginGroup(windowRect);
                GUI.Box(new Rect(0, 0, winWidth, winHeight), "");
                float messageX = 5;
                float messageY = 5;
                foreach (var message in mailBox.messages) {
                    DrawMessage(new Vector2(messageX, messageY), message.Value);
                    messageY += 100;
                }

                //----------------------------------------------------------------------------------------------
                float halfWinWidth = winWidth * 0.5f;
                float lblX = halfWinWidth;
                float writeX = halfWinWidth + 50;
                float writeY = 5;

                GUI.Label(new Rect(lblX, writeY, 50, 20), "To:", mLabelStyle);
                mTargetLogin = GUI.TextField(new Rect(writeX, writeY, halfWinWidth, 20), mTargetLogin);
                writeY += 20;
                GUI.Label(new Rect(lblX, writeY, 50, 20), "Title:", mLabelStyle);
                mTitle = GUI.TextField(new Rect(writeX, writeY, halfWinWidth, 20), mTitle);
                writeY += 20;
                GUI.Label(new Rect(lblX, writeY, 50, 20), "Body:", mLabelStyle);
                mBody = GUI.TextField(new Rect(writeX, writeY, halfWinWidth, 20), mBody);
                writeY += 20;

                float itX = lblX;
                float itY = writeY;

                var inventory = NetworkGame.Instance().Inventory;
                foreach (var item in inventory.OrderedItems()) {
                    if (GUI.Button(new Rect(itX, itY, 20, 20), item.Object.Id[0] + item.Count.ToString())) {
                        AddAttachmnet(item.Object);
                    }
                    itX += 25;
                }
                writeY += 20;
                GUI.Label(new Rect(lblX, writeY, 50, 20), string.Format("Attachment count: {0}", mAttachments.Count), mLabelStyle);
                writeY += 20;
                if (GUI.Button(new Rect(lblX, writeY, 40, 20), "send")) {
                    SelectCharacterGame.Instance().WriteMailMessage(mTargetLogin, mTitle, mBody, mAttachments.ToArray());
                    mAttachments.Clear();
                }
                GUI.EndGroup();
            } else {
                GUI.Label(new Rect(0, 0, 0, 0), "Key Q for show mails testing", mLabelStyle);
            }
        }

        private void AddAttachmnet(IInventoryObjectInfo obj) {

            if (mAttachments.Find(o => o.Id == obj.Id) == null) {
                mAttachments.Add(obj);
            }
        }

        private void DrawMessage(Vector2 pos, MailMessage message) {
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), "From: " + message.senderLogin, mLabelStyle);
            pos.y += 20;
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), "Date: " + message.time.ToString(), mLabelStyle);

            if (GUI.Button(new Rect(pos.x + 200, pos.y, 100, 30), "Delete")) {
                SelectCharacterGame.Instance().DeleteMailMessage(message.id);
            }

            pos.y += 20;
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), "Title: " + message.title, mLabelStyle);
            pos.y += 20;
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), "Body: " + message.body, mLabelStyle);
            pos.y += 20;
            
            float attachmentX = pos.x;
            if (message.attachments == null) {
                return;
            }
            foreach (var attachment in message.attachments) {
                DrawAttachment(new Vector2(attachmentX, pos.y), message, attachment.Value);
                attachmentX += 25;
            }
        }

        private void DrawAttachment(Vector2 pos, MailMessage message, MailAttachment attachment) {
            try {
                //PrintAttachment(attachment);
                IPlacingType obj = attachment.ParseAttachedObject();


                if (obj.placingType == (int)PlacingType.Inventory) {
                    if (GUI.Button(new Rect(pos.x, pos.y, 20, 20), "I")) {
                        IInventoryObjectBase inventoryObject = obj as IInventoryObjectBase;
                        //take attachment
                        SelectCharacterGame.Instance().TakeAttachment(message.id, attachment.id);
                    }
                } else if (obj.placingType == (int)PlacingType.Station) {
                    if (GUI.Button(new Rect(pos.x, pos.y, 20, 20), "M")) {
                        IStationHoldableObject stationObject = obj as IStationHoldableObject;
                        SelectCharacterGame.Instance().TakeAttachment(message.id, attachment.id);
                    }
                } else {
                    Debug.LogError("unsupported object");
                }
            } catch (System.Exception exception) {
                Debug.LogError(exception.Message);
                Debug.LogError(exception.StackTrace);
            }
        }

        private void PrintAttachment(MailAttachment attachment) {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (DictionaryEntry entry in attachment.objectHash) {
                builder.AppendLine(string.Format("{0}={1}, type={2}", (SPC)(int)entry.Key, entry.Value, entry.Value.GetType().Name));
            }
            Debug.Log(builder.ToString());
        }

    }
}