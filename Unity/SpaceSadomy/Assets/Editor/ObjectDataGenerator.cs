// ObjectDataGenerator.cs
// Nebula
// 
// Created by Oleg Zhelestcov on Wednesday, December 3, 2014 12:43:03 PM
// Copyright (c) 2014 KomarGames. All rights reserved.
//
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.Xml.Linq;
using Common;

public class ObjectDataGenerator : EditorWindow 
{
    private int startIndex = 0;

    private float respawn = 0f;
    private string dataIdString = "CraftAsteroid0001;CraftAsteroid0002;CraftAsteroid0003;CraftAsteroid0004;CraftAsteroid0005;CraftAsteroid0006;CraftAsteroid0007;CraftAsteroid0008;CraftAsteroid0009;CraftAsteroid0010";

    private string outputText = string.Empty;
    private int forceCreate = 0;

    [MenuItem("Space/Object Data Generator")]
    static void ShowGenerator()
    {
        var wnd = GetWindow<ObjectDataGenerator>();
        wnd.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        this.startIndex = EditorGUILayout.IntField("Start Index", this.startIndex);
        this.respawn = EditorGUILayout.FloatField("Respawn", this.respawn);
        this.dataIdString = EditorGUILayout.TextField("Data Ids", this.dataIdString);
        forceCreate = EditorGUILayout.IntField("Force create", forceCreate);
        if(GUILayout.Button("Create Asteroids"))
        {
            ProcessAsteroids();
        }
        EditorGUILayout.TextArea(outputText);

        EditorGUILayout.EndVertical();
    }

    void ProcessAsteroids()
    {
        int currentIndex = startIndex;

        XElement root = new XElement("asteroids");
        foreach(Transform t in Selection.transforms)
        {
            XElement asteroid = new XElement("asteroid");
            asteroid.SetAttributeValue("index", currentIndex);
            asteroid.SetAttributeValue("data_id", GetDataId(dataIdString));
            asteroid.SetAttributeValue("respawn", respawn.ToString("F1"));
            asteroid.SetAttributeValue("position", GetVectorString(t.position));
            asteroid.SetAttributeValue("rotation", GetVectorString(t.rotation.eulerAngles));
            asteroid.SetAttributeValue("force_create", forceCreate != 0 ? true : false);
            root.Add(asteroid);
            currentIndex++;
        }

        this.outputText = root.ToString();
    }

    private string GetVectorString(Vector3 vec)
    {
        return string.Format("{0:F1},{1:F1},{2:F1}", vec.x, vec.y, vec.z);
    }

    private string GetDataId(string dataIdStr)
    {
        string[] strArr = dataIdStr.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        return strArr.GetRandomElement();
    }
}


