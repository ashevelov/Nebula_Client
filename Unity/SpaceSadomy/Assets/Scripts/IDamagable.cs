using UnityEngine;
using System.Collections;

public interface IDamagable {

    bool IsDead();
    float GetHealth();
    float GetMaxHealth();
    float GetHealth01();

    float GetOptimalDistance();


    float GetMaxHitSpeed();
    float GetSpeed();

    Vector3 GetPosition();
}