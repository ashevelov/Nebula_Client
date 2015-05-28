using UnityEngine;
using System.Collections;
using UnityEditor;

public class Commands : MonoBehaviour {

    [MenuItem("Space/Randomize Z Position")]
    static void ShowRandomZAndEulerAngles() {
        RandomiZAndEulerAngles wnd = EditorWindow.GetWindow<RandomiZAndEulerAngles>();
        wnd.Show();
    }

    [MenuItem("Space/Randomize rotation")]
    static void RandomizeRotation()
    {
        foreach (Transform transform in Selection.transforms)
        {
            float radius = transform.localPosition.x;
            float angle = Random.value * Mathf.PI * 2.0f;
            float y = transform.localPosition.y;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            transform.localPosition = new Vector3(x, y, z);
        }
    }
}

public class RandomiZAndEulerAngles : EditorWindow {
    private float zMin = -3;
    private float zMax = 3;
    private float xMinAng = -180;
    private float xMaxAng = 180;
    private float yMinAng = -180;
    private float yMaxAng = 180;
    private float zMinAng = -180;
    private float zMaxAng = 180;

    void OnGUI() {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        zMin = float.Parse( EditorGUILayout.TextField("Z Min", zMin.ToString() ) );
        zMax = float.Parse( EditorGUILayout.TextField("Z Max", zMax.ToString()));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        xMinAng = float.Parse(EditorGUILayout.TextField("Euler X Min", xMinAng.ToString()));
        xMaxAng = float.Parse(EditorGUILayout.TextField("Euler X Max", xMaxAng.ToString()));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        yMinAng = float.Parse(EditorGUILayout.TextField("Euler Y Min", yMinAng.ToString()));
        yMaxAng = float.Parse(EditorGUILayout.TextField("Euler Y Max", yMaxAng.ToString()));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        zMinAng = float.Parse(EditorGUILayout.TextField("Euler Z Min", zMinAng.ToString()));
        zMaxAng = float.Parse(EditorGUILayout.TextField("Euler Z Max", zMaxAng.ToString()));
        EditorGUILayout.EndHorizontal();
        if(GUILayout.Button("Randomize")) {
            foreach(var t in Selection.transforms){
                t.position = new Vector3(t.position.x, t.position.y, Random.Range(zMin, zMax));
                t.rotation = Quaternion.Euler(Random.Range(xMinAng, xMaxAng), Random.Range(yMinAng, yMaxAng), Random.Range(zMinAng, zMaxAng));
            }
        }
        EditorGUILayout.EndVertical();
    }
}
