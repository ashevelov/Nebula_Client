using UnityEngine;
using System.Collections;

public class LaserGravityWave : MonoBehaviour {

    public EffectSettings effectSettings;
    public ProjectileCollisionBehaviour projectile;


    public void StartEffect(Transform target)
    {
        effectSettings.Target = target.gameObject;
        projectile.StartProjectile();
    }

    void Update()
    {
    }
}
