using Nebula.Client.Mail;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxAttachmentList : MonoBehaviour {

    public GameObject attachmentElementPrefab;
    public Transform attachmentListLayout;

    private ClientMailMessage message;

    public void SetAttachments(ClientMailMessage message) {
        this.message = message;
        attachmentListLayout.RemoveChildrens();
        var attachments = this.SortAttachments(message.Attachments().Attachments());

        foreach (var attachment in attachments) {
            GameObject attachmentElementInstance = Instantiate(attachmentElementPrefab) as GameObject;
            attachmentElementInstance.GetComponent<MailBoxAttachmentListElement>().SetAttachment(attachment);
            attachmentElementInstance.transform.SetParent(attachmentListLayout, false);
        }
    }
    public ClientMailMessage Message() {
        return this.message;
    }

    public class AttachmentComparer : IComparer<ClientAttachment> {
        public int Compare(ClientAttachment x, ClientAttachment y) {
            return x.ID().CompareTo(y.ID());
        }
    }

    private List<ClientAttachment> SortAttachments(List<ClientAttachment> inputs) {
        inputs.Sort(new AttachmentComparer());
        return inputs;
    }
}
