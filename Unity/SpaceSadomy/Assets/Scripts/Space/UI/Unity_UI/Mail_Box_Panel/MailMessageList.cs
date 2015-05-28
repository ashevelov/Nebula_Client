using Nebula.Client.Mail;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailMessageList : MonoBehaviour {

    public GameObject mailMessageInfoElementPrefab;
    public Transform listLayout;
    public Scrollbar listScrollbar;


    private void SetMailBox(List<ClientMailMessage> messages) {

        for (int i = 0; i < messages.Count; i++) {
            var message = messages[i];
            GameObject mailMessageInfoInstance = Instantiate(this.mailMessageInfoElementPrefab) as GameObject;
            mailMessageInfoInstance.GetComponent<MailMessageInfoListElement>().SetMessageData(message);
            mailMessageInfoInstance.transform.SetParent(listLayout, false);
            mailMessageInfoInstance.transform.SetSiblingIndex(0);
            mailMessageInfoInstance.GetComponent<MailMessageInfoListElement>().readedToggle.group = listLayout.GetComponent<ToggleGroup>();
        }
    }

    private List<MailMessageInfoListElement> ToList(MailMessageInfoListElement[] arr) {
        List<MailMessageInfoListElement> result = new List<MailMessageInfoListElement>();
        result.AddRange(arr);
        return result;
    }

    public void UpdateList(List<ClientMailMessage> messages) {
        List<MailMessageInfoListElement> existingElements = ToList(listLayout.GetComponentsInChildren<MailMessageInfoListElement>());
        List<MailMessageInfoListElement> elementsToRemove = new List<MailMessageInfoListElement>();
        List<ClientMailMessage> messagesToAdd = new List<ClientMailMessage>();

        foreach (var message in messages) {
            MailMessageInfoListElement updatedElement = null;
            if (ContainsMessage(existingElements, message, out updatedElement)) {
                updatedElement.SetMessageData(message);
            } else {
                messagesToAdd.Add(message);
            }
        }

        foreach (var elem in existingElements) {
            if (!ExistInMessages(messages, elem)) {
                elementsToRemove.Add(elem);
            }
        }

        SetMailBox(this.OrderMailMessages(messagesToAdd));

        foreach (var elem in elementsToRemove) {
            Destroy(elem.gameObject);
        }
    }

    public class MailMessageComparer : IComparer<ClientMailMessage> {
        public int Compare(ClientMailMessage x, ClientMailMessage y) {
            return x.Time().CompareTo(y.Time());
        }
    }

    private List<ClientMailMessage> OrderMailMessages(List<ClientMailMessage> inputs) {
        inputs.Sort(new MailMessageComparer());
        return inputs;
    }

    private bool ContainsMessage(List<MailMessageInfoListElement> elems, ClientMailMessage message, out MailMessageInfoListElement result) {
        result = null;
        foreach (var elem in elems) {
            if (elem.MessageData().ID() == message.ID()) {
                result = elem;
                return true;
            }
        }
        return false;
    }

    private bool ExistInMessages(List<ClientMailMessage> messages, MailMessageInfoListElement elem) {
        foreach (var msg in messages) {
            if (msg.ID() == elem.MessageData().ID()) {
                return true;
            }
        }
        return false;
    }


}
