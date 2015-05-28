// PlaceObjectsInCircleSector.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, December 3, 2014 10:55:00 AM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Располагает выделенные объекты в секторальном круге с заданным раствором относительно центра с произвольным вращением
/// </summary>
public class PlaceObjectsInCircleSector : EditorWindow {

    private Vector3 centerPosition = Vector3.zero;
    private Vector3 centerRotation = Vector3.zero;
    private float sectorMinRadius = 0f;
    private float sectorMaxRadius = 0f;
    private float sectorStartAngle = 0f;
    private float sectorAngle = 0f;

    [MenuItem("Space/Place Objects In Sector")]
    static void ShowPlaceObjectWindow()
    {
        var wnd = EditorWindow.GetWindow<PlaceObjectsInCircleSector>();
        wnd.Show();
    }
    
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        centerPosition = EditorGUILayout.Vector3Field("Center Position", centerPosition);
        centerRotation = EditorGUILayout.Vector3Field("Center Rotation", centerRotation);
        sectorMinRadius = EditorGUILayout.FloatField("Min Radius", sectorMinRadius);
        sectorMaxRadius = EditorGUILayout.FloatField("Max Radius", sectorMaxRadius);
        sectorStartAngle = EditorGUILayout.FloatField("Start Angle", sectorStartAngle);
        sectorAngle = EditorGUILayout.FloatField("Sector Angle", sectorAngle);

        if(GUILayout.Button("Place"))
        {
            GameObject centerObj = new GameObject("Center");
            centerObj.transform.position = centerPosition;
            centerObj.transform.rotation = Quaternion.Euler(centerRotation);
            foreach(Transform t in Selection.transforms)
            {
                t.parent = centerObj.transform;
                t.localPosition = RandomSectorPoint(sectorStartAngle, sectorAngle, sectorMinRadius, sectorMaxRadius);
                t.localRotation = Quaternion.Euler(RandomRotation());
            }

            foreach(Transform t in Selection.transforms)
            {
                t.parent = null;
            }

            DestroyImmediate(centerObj);
        }
        EditorGUILayout.EndVertical();
    }

    private Vector3 RandomSectorPoint(float startAngle, float sectorAngle, float minRadius, float maxRadius)
    {
        float min = startAngle;
        float max = startAngle + sectorAngle;
        float ang = Random.Range(min, max);
        float rad = Random.Range(minRadius, maxRadius);
        float x = rad * Mathf.Cos(ang * Mathf.Deg2Rad);
        float y = rad * Mathf.Sin(ang * Mathf.Deg2Rad);
        return new Vector3(x, y, 0);
    }

    private Vector3 RandomRotation()
    {
        return new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }
}
