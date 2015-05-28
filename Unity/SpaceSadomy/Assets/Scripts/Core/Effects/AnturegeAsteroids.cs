using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnturegeAsteroids : MonoBehaviour {

    public GameObject asteroid;
    private List<GameObject> _asteroids = new List<GameObject>();
    private List<GameObject> _removeAsteroids = new List<GameObject>();

    private int _maxCount = 300;
    private Vector3 _oldPos;
    private float _addPos;

    void Start()
    {
        _oldPos = transform.parent.position;
    }
    void Update()
    {
        if (_asteroids.Count < _maxCount)
        {
            Vector3 deltaPos = transform.parent.position - _oldPos;
            GameObject go = Instantiate(asteroid) as GameObject;
            go.transform.position = transform.position + (deltaPos.normalized *50000) + (50000 * Random.insideUnitSphere);
            go.transform.localEulerAngles = Random.insideUnitSphere * 360;
            go.transform.localScale = Vector3.one * Random.value * 1000;
            _asteroids.Add(go);
        }
        for (int i = 0; i < _asteroids.Count; i++ )
        {
            if (Vector3.Distance(_asteroids[i].transform.position, transform.position) > 100000)
            {
                _removeAsteroids.Add(_asteroids[i]);
            }
        }

        _removeAsteroids.ForEach((ra) => {
            if (ra != null)
            {
                _asteroids.Remove(ra);
                Destroy(ra);
            }
        });
        _removeAsteroids.Clear();

        _oldPos = transform.parent.position;
    }
}
