using UnityEditor;
using UnityEngine;

public class CreateCirclePathAtSelection : EditorWindow {

    [MenuItem("Space/Create Circle Path At Selection")]
    static void Init() {
        CreateCirclePathAtSelection window = (CreateCirclePathAtSelection)EditorWindow.GetWindow<CreateCirclePathAtSelection>();
        window.Show();
    }

    private float mRadius;
    private int mCount;

    void OnGUI() {
        mRadius = EditorGUILayout.FloatField("Radius:", mRadius);
        mCount = EditorGUILayout.IntField("Path length: ", mCount);

        if(GUILayout.Button("Create path")) {
            if(Selection.activeGameObject) {
                CreatePath(Selection.activeGameObject, mRadius, mCount);
            }
        }
    }

    private void CreatePath(GameObject center, float radius, int count) {
        float dAngle = Mathf.PI * 2 / count;
        float curAngle = 0;

        for(int i = 0; i < count; i++) {
            GameObject pt = new GameObject("Path Point " + i);
            pt.transform.SetParent(center.transform);
            pt.transform.localPosition = new Vector3(radius * Mathf.Cos(curAngle), 0, radius * Mathf.Sin(curAngle));
            curAngle += dAngle;
        }
    }
}
