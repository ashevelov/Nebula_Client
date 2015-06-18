namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Nebula.Client.Notifications;
    using ServerClientCommon;

    public class TestNotifications : MonoBehaviour {

        public GUISkin skin;

        private GUIStyle mLabelStyle;
        private bool mVisible = false;


        void Start() {
            mLabelStyle = skin.GetStyle("font_upper_left");
        }

        void Update() {
            if(Input.GetKeyUp(KeyCode.Alpha2)) {
                mVisible = !mVisible;
                if(mVisible) {
                    MmoEngine.Get.SelectCharacterGame.GetNotifications();
                }
            }
        }

        void OnGUI () {

            if (mVisible) {
                if(MmoEngine.Get.GameData.notifications.notifications == null ) { return; }

                Rect windowRect = new Rect(0, 0, Screen.width, Screen.height);
                GUI.BeginGroup(windowRect);
                GUI.Box(windowRect, "");
                float x = 5;
                float y = 5;
                foreach (var notification in MmoEngine.Get.GameData.notifications.notifications) {
                    DrawNotification(new Vector2(x, y), notification.Value);
                    y += 70;
                }
                GUI.EndGroup();
            }
        }

        private void DrawNotification(Vector2 pos, Notification notification) {
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), notification.text, mLabelStyle);
            pos.y += 20;
            GUI.Label(new Rect(pos.x, pos.y, 0, 0), string.Format("handled: {0}", notification.handled), mLabelStyle);
            pos.y += 20;
            NotficationRespondAction respondAction = (NotficationRespondAction)notification.respondAction;
            if (GUI.Button(new Rect(pos.x, pos.y, 50, 20), "NO")) {
                //handle NO
                MmoEngine.Get.SelectCharacterGame.HandleNotification(notification, false);
            }
            if (respondAction == NotficationRespondAction.YesDelete && (false == notification.handled)) {
                if (GUI.Button(new Rect(pos.x + 60, pos.y, 50, 20), "YES")) {
                    //handle yes
                    MmoEngine.Get.SelectCharacterGame.HandleNotification(notification, true);
                }
            }

            pos.y += 20;

        }
    }
}
