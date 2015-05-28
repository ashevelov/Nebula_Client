﻿using UnityEngine;
using System.Collections;

public interface IDamagable {

    bool IsDead();
    bool IsPowerShieldEnabled();
    float GetHealth();
    float GetMaxHealth();
    float GetHealth01();

    float GetOptimalDistance();
    float GetRange();

    float GetMaxHitSpeed();
    float GetSpeed();

    Vector3 GetPosition();
}