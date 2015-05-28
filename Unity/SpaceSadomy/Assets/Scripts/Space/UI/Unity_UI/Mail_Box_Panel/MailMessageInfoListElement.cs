using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Client.Mail;

public class MailMessageInfoListElement : MonoBehaviour {

    public Toggle readedToggle;
    public Text senderName;
    public Text dateName;
    public Text titleText;
    public Text isNotReadedText;
    public Color notReadedColor;
    public Color readedColor;


    private ClientMailMessage messageData;

    public ClientMailMessage MessageData() {
        return this.messageData;
    }

    public void SetMessageData(ClientMailMessage messageData) {
        this.messageData = messageData;
        this.SetReadedView(this.messageData.Readed());
        this.senderName.text = this.messageData.SenderGameRefID();
        this.dateName.text = this.messageData.Time().ToString(System.Globalization.CultureInfo.InvariantCulture);
        this.titleText.text = this.messageData.Title();
        this.readedToggle.onValueChanged.RemoveAllListeners();
        this.readedToggle.onValueChanged.AddListener((val) => {
            Debug.Log("onValueChanged occured!");
            if (val && this.MessageData() != null) {
                transform.GetComponentInParent<MailBoxPanel>().MessageDetail().SetMessage(MessageData());
            }
        });
    }

    private void SetReadedView(bool isReaded) {
        this.senderName.color = (isReaded) ? readedColor : notReadedColor;
        this.isNotReadedText.gameObject.SetActive(!isReaded);
    }

    public void SetToggleValue(bool value) {
        this.readedToggle.isOn = value;
    }
}
