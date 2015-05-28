using UnityEngine;
using System.Collections;

public class REGunsY : MonoBehaviour {

    private float speed = 100;
    public LineRenderer lineRender;


    void Update()
    {
        if (lineRender == null)
        {
            lineRender = GetComponentInChildren<LineRenderer>();
        }
        else
        {
            lineRender.SetPosition(0, transform.position + transform.forward * 0.02f + transform.up * 0.04f);
            if (Input.GetKey(KeyCode.Space))
            {
                lineRender.SetPosition(1, transform.position + transform.up * 10000);
            }
            else
            {
                lineRender.SetPosition(1, transform.position + transform.forward * 0.02f + transform.up * 0.04f);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.localEulerAngles += new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localEulerAngles -= new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }


    }
}
