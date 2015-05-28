using UnityEngine;
using System.Collections;

public class SkillEffect : MonoBehaviour 
{
    public float lifeTime = 3.0f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
