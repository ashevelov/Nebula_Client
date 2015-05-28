using UnityEngine;
using System.Collections;
using UnityEditor;

public class RandomGameObj : EditorWindow
{

    [MenuItem("Space/RandomAsteroid")]
    static void ShowLoader()
    {
        RandomGameObj w = EditorWindow.GetWindow<RandomGameObj>();
        w.Show();
    }
    private string name = "Asteroid";
    private Object source;
    private GameObject GameObj;
    private Vector3 minPos = new Vector3(-100,-100,-100);
    private Vector3 maxPos = new Vector3(100, 100, 100);
    private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 maxScale = new Vector3(2, 2, 2);
    private Vector3 minRotation = new Vector3(0, 0, 0);
    private Vector3 maxRotation = new Vector3(360, 360, 360);
    private float count = 1;
    void OnGUI()
    {
        source = EditorGUILayout.ObjectField(source, typeof(Object), true);
        name = GUILayout.TextField(name, 25);
        count = EditorGUILayout.FloatField("count ", count);
        minPos = EditorGUILayout.Vector3Field("min pos", minPos);
        maxPos = EditorGUILayout.Vector3Field("max pos", maxPos);

        minScale = EditorGUILayout.Vector3Field("min Scale", minScale);
        maxScale = EditorGUILayout.Vector3Field("max Scale", maxScale);

        minRotation = EditorGUILayout.Vector3Field("min Rotation", minRotation);
        maxRotation = EditorGUILayout.Vector3Field("max Rotation", maxRotation);

        if (GUILayout.Button("Create new obj"))
        {
            for (int i = 0; i < count; i++ )
            {
                GameObject go = Instantiate(source) as GameObject;
                go.name = name+"_"+i.ToString();
                go.transform.position = new Vector3(Random.Range(minPos.x, maxPos.x),
                                                    Random.Range(minPos.y, maxPos.y),
                                                    Random.Range(minPos.z, maxPos.z));

                go.transform.localScale = new Vector3(Random.Range(minScale.x, maxScale.x),
                                                    Random.Range(minScale.y, maxScale.y),
                                                    Random.Range(minScale.z, maxScale.z));

                go.transform.rotation = new Quaternion(Random.Range(minRotation.x, maxRotation.x),
                                                    Random.Range(minRotation.y, maxRotation.y),
                                                    Random.Range(minRotation.z, maxRotation.z), 0);
            }
        }
    }
}
