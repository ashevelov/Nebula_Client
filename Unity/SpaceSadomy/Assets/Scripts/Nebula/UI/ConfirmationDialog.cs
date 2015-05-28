using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour {

    public Text text;
    System.Action pAction;
    System.Action nAction;

    public static void Setup(string text, System.Action pAction = null, System.Action nAction = null)
    {
        GameObject go = Resources.Load("Prefabs/UI/Confirmation_Dialog") as GameObject;
        ConfirmationDialog cd = (Instantiate(go) as GameObject).GetComponent<ConfirmationDialog>();

        cd.Init(text, pAction, nAction);
        cd.transform.SetParent(FindObjectOfType<Canvas>().transform);
        cd.transform.localScale = Vector2.one;
        cd.transform.localPosition = Vector2.zero;
    }

    public void Init(string text, System.Action pAction = null, System.Action nAction = null)
    {
        this.text.text = text; // •_•
        this.pAction = pAction;
        this.nAction = nAction;
    }

    public void OKButton()
    {
        if (pAction != null)
        {
            pAction();
        }
        Close();
    }

    public void CloseButton()
    {
        if (nAction != null)
        {
            nAction();
        }
        Close();
    }

    void Close()
    {
        Destroy(gameObject);
    }
}
