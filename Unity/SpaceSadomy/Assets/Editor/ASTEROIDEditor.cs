using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;
using System.Linq;

[CustomEditor(typeof(Nebula.Server.ASTEROID))]
public class ASTEROIDEditor : Editor {

    private int dataIndex = 0;
    private static string[] asteroidDataIDS;

    void OnEnable() {
        Debug.Log("ASTEROID EDITOR OnEnable()");

    }

    private void LoadAsteroidDataIDS() {
        if(asteroidDataIDS == null ) {
            XDocument document = XDocument.Parse(Resources.Load<TextAsset>("Data/asteroids").text);
            asteroidDataIDS = document.Element("asteroids").Elements("asteroid").Select(e => {
                return e.Attribute("id").Value;
            }).ToArray();
        }
    }


    public override void OnInspectorGUI() {

        Nebula.Server.ASTEROID asteroid = (Nebula.Server.ASTEROID)target;
        asteroid.asteroidID = EditorGUILayout.IntField("ID:", asteroid.asteroidID);
        asteroid.forceCreate = EditorGUILayout.Toggle("Force Create:", asteroid.forceCreate);

        LoadAsteroidDataIDS();

        if(!string.IsNullOrEmpty(asteroid.dataID)) {
            dataIndex = GetDataIndex(asteroid.dataID);
        }

        dataIndex = EditorGUILayout.Popup("Data ID:", dataIndex, asteroidDataIDS);
        asteroid.dataID = asteroidDataIDS[dataIndex];
        asteroid.respawnInterval = EditorGUILayout.FloatField("Respawn Interval:", asteroid.respawnInterval);
        asteroid.modelID = EditorGUILayout.TextField("Model ID:", asteroid.modelID);
        if(GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }

    private int GetDataIndex(string dataID) {
        for(int i = 0; i < asteroidDataIDS.Length; i++) {
            if(asteroidDataIDS[i] == dataID) {
                return i;
            }
        }
        return 0;
    }
}
