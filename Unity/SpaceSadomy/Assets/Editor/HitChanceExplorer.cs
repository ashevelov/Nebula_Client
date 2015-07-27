using UnityEngine;
using System.Collections;
using UnityEditor;
using Common;

public class HitChanceExplorer : EditorWindow 
{
    private AnimationCurve curve = new AnimationCurve();
    private float optimalDistance;
    private float range;
    private float maxSpeed;
    private float targetSpeed;
    //private float targetDistance;

    private float sliderMinWidth = 500;
    private float step = 30;


    [MenuItem("Space/Hit Chance Explorer")]
    static void Init()
    {
        var window = GetWindow<HitChanceExplorer>();
        window.Show();
    }

    void OnGUI()
    {
        this.optimalDistance = EditorGUILayout.Slider("Optimal Dst.", this.optimalDistance, 0, 1000, GUILayout.MinWidth(this.sliderMinWidth));
        this.range = EditorGUILayout.Slider("Range", this.range, 0, 1000, GUILayout.MinWidth(this.sliderMinWidth));

        this.maxSpeed = EditorGUILayout.Slider("Max speed(max target speed with 100% of hit)", this.maxSpeed, 0, 1000, GUILayout.MinWidth(this.sliderMinWidth));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        this.targetSpeed = EditorGUILayout.Slider("Target speed", this.targetSpeed, 0, 1000, GUILayout.MinWidth(this.sliderMinWidth));
        //this.targetDistance = EditorGUILayout.Slider("Target distance", this.targetDistance, 0, 10000, GUILayout.MinWidth(this.sliderMinWidth));

        EditorGUILayout.Space();

        if (GUILayout.Button("Recompute"))
        {
            //clear curve
            //for (int i = 0; i < this.curve.length; i++)
            //{
            //    this.curve.RemoveKey(i);
            //}
            this.curve = new AnimationCurve();

            for (float t = 0; t <= 1000; t += step)
            {
                //float prob = GameBalance.ComputeHitProb(this.optimalDistance, this.range, this.farDist, this.farProb, this.nearDist, this.nearProb, this.maxSpeed, this.targetSpeed, t);

                float prob = GameBalance.ComputeHitProb(this.optimalDistance,  t);
                this.curve.AddKey(new Keyframe(t, prob));
            }
        }

        curve = EditorGUILayout.CurveField(curve, GUILayout.MinWidth(500), GUILayout.MinHeight(500));

    }
}
