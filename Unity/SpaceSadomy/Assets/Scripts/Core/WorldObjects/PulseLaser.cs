using UnityEngine;
using System.Collections;

public class PulseLaser : MonoBehaviour {

    public float _fireTime = 1;
    public float _speed = 10;

    public LineRenderer _lineRenderer;
    public Transform _laser_turrel;
    public Transform _target;

    private float _time = 0;
    public Vector3[] _positions = new Vector3[2] { Vector3.zero, Vector3.zero };
    public float pos;
    public float length = 0;
    public bool _powerShield = false;
    public GameObject explositionPrefab;
    public bool _hit;

    public static void Init(Transform laser_turrel, Transform target, bool shield, bool hit)
    {
        GameObject go = Resources.Load("Prefabs/Items/Weapons/Lasers/PulseLaser") as GameObject;
        if (go != null)
        {
            if (target != null && laser_turrel != null)
            {
                GameObject inst = Instantiate(go) as GameObject;
                inst.GetComponent<PulseLaser>()._laser_turrel = laser_turrel;
                inst.GetComponent<PulseLaser>()._target = target;
                inst.GetComponent<PulseLaser>()._powerShield = shield;
                inst.GetComponent<PulseLaser>()._hit = hit;
                inst.transform.parent = laser_turrel;
                inst.transform.localPosition = Vector3.zero;
                inst.GetComponent<PulseLaser>()._fireTime = Mathf.Clamp((Vector3.Distance(laser_turrel.position, target.position) / 100000), 0.1f, 2);

                inst.GetComponent<PulseLaser>()._speed = Mathf.Clamp((inst.GetComponent<PulseLaser>()._speed / inst.GetComponent<PulseLaser>()._fireTime), 0.5f, 1.3f);
            }
        }
        else
        {
            Debug.Log("game object not found");
        }
    }
    private bool shildexp = false;
    void FixedUpdate()
    {
        if (_target == null || _laser_turrel == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 delta = _target.position - _laser_turrel.position;

            float distance = Vector3.Distance(_target.position, _laser_turrel.position);
            _time += Time.deltaTime * _speed;

            if (_time > _fireTime)
            {
                pos = (_time - _fireTime) * _speed;
            }
            else
            {
                length += Time.deltaTime * _speed;
            }

            _positions[0] = (delta * pos);
            _positions[1] = (delta * length) + (delta * pos);
            if (Vector3.Distance((_laser_turrel.position + _positions[0]), _target.position) < 100*_speed)
            {
                Destroy(gameObject);
            }
            if (Vector3.Distance((_laser_turrel.position + _positions[1]), _target.position) < 100 * _speed)
            {
                length -= Time.deltaTime * _speed;
                if (_powerShield && _hit && !shildexp)
                {
                    ShieldExp.Init(_laser_turrel, _target,1);
                    shildexp = true;
                }
                if (_hit)
                {
                    if (explositionPrefab)
                    {
                        GameObject inst = Instantiate(explositionPrefab) as GameObject;
                        inst.transform.position = _target.position - delta.normalized * 10 + new Vector3(Random.Range(-5, 5),
                                                                                                          Random.Range(-5, 5),
                                                                                                          Random.Range(-5, 5));
                        inst.transform.parent = _target;
                        Destroy(inst, 2);
                    }
                }
            }


            _lineRenderer.SetPosition(0, _laser_turrel.position + (delta * pos));
            _lineRenderer.SetPosition(1, _laser_turrel.position + _positions[1]);
        }
        
    }
}
