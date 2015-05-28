using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BetaGunsController : MonoBehaviour
{
    public float rotationSpeed = 1;

    private List<GameObject> _guns = new List<GameObject>();
    private Vector3 _gunDirection;

    private float angle = 0;
    

    void Start()
    {
        _gunDirection = transform.forward * 1000;
        foreach (GameObject gun in GameObject.FindGameObjectsWithTag("Gun"))
        {
            _guns.Add(gun);
        }
    }

    void Update()
    {
        InputController();

       _guns.ForEach((g) =>
            {
                g.transform.RotateAround(g.transform.position, g.transform.up, rotationSpeed * Time.deltaTime * angle);
            });
        angle -= angle * Time.deltaTime;
    }

    private void InputController()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            angle = -90;
            _gunDirection = new Vector3(-1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            angle = 90;
            _gunDirection = new Vector3(1, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = new GameObject();

                    go.transform.position = transform.right *10000;
                    Destroy(go, 8);

            _guns.ForEach((g) =>
            {
                if (g.transform.position.x > transform.position.x)
                {
                    GameObject missile = Instantiate(Resources.Load("Prefabs/Items/Weapons/Missiles/MissileBeta") as GameObject) as GameObject;
                    missile.transform.position = g.transform.position;
                    missile.transform.eulerAngles = -g.transform.eulerAngles;
                    missile.GetComponent<Missile>().SetTarget(go.transform, false, 0.2f, false);
                }
            });
        }
    }
}
