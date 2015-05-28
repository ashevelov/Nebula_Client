using UnityEngine;
using System.Collections;

[System.Serializable]
public struct MinMax
{
    public float Min;
    public float Max;
    


    public MinMax(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }
}
