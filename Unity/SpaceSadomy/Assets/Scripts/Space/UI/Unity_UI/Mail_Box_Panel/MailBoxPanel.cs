using UnityEngine;
using System.Collections;

public class MailBoxPanel : MonoBehaviour {

    public MailMessageList messageList;
    public MailBoxMessageDetailView detailView;

    private float updateTimer = 3.0f;

    public MailMessageList MessageList() {
        return this.messageList;
    }

    public MailBoxMessageDetailView MessageDetail() {
        return this.detailView;
    }

    void Start() {
    }

    void Update() {
        this.updateTimer += Time.deltaTime;
        if (updateTimer >= 3.0f) {
            UpdateMailBox();
            updateTimer = 0f;
        }
    }

    private void UpdateMailBox() {
        if (G.Game == null) {
            return;
        }
        if (G.Game.MailBox() == null) {
            return;
        }
        messageList.UpdateList(G.Game.MailBox().Messages());

    }
}
