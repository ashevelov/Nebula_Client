using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour {

    public Text tempMessage;
    public List<Text> messages;

    public void AddMassage(string text)
    {
        tempMessage.text = text;
    }

	public void Send()
    {

    }
}
