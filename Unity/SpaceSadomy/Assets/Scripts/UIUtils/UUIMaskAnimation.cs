using UnityEngine;
using System.Collections;

public class UUIMaskAnimation : MonoBehaviour {

    public Vector2 showPos;
    public Vector2 hidePos;
    public float speed = 500f;

    private RectTransform rctTransform;
	void Start () {
        rctTransform = transform as RectTransform;
	}
	void Update () {
        if (showing)
        {
            MoveAnimation(showPos);

        }
        if (hiding)
        {
            MoveAnimation(hidePos);
        }
	}

    private void MoveAnimation(Vector2 newPos)
    {
        Vector2 tempPos = rctTransform.sizeDelta;
        tempPos -= (tempPos - newPos).normalized * speed * Time.deltaTime;
        rctTransform.sizeDelta = tempPos;
        if (Vector2.Distance(rctTransform.sizeDelta, newPos) < (2 * speed * Time.deltaTime))
        {
            rctTransform.sizeDelta = newPos;
            showing = false;
            hiding = false;
        }
    }

    private bool showing = false;
    public void ShowAnimation()
    {
        showing = true;
        hiding = false;
    }
    private bool hiding = false;
    public void HideAnimation()
    {
        showing = false;
        hiding = true;
    }
}
