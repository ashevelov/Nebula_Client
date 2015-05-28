using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Client.Mail;

public class MailBoxMessageDetailView : MonoBehaviour {

    public MailBoxAttachmentList attachmentList;
    public Text titleText;
    public Text bodyText;
    public Scrollbar bodyTextScrollbar;
    public ScrollRect messageBodyScrollRect;

    private ClientMailMessage message;

    public void SetMessage(ClientMailMessage message) {
        this.message = message;
        this.attachmentList.SetAttachments(this.message);
        this.titleText.text = this.message.Title();
        this.bodyText.text = this.message.Body();
        this.bodyTextScrollbar.value = 1f;
        messageBodyScrollRect.Rebuild(CanvasUpdate.PostLayout);
    }

    public ClientMailMessage Message() {
        return this.message;
    }
}
