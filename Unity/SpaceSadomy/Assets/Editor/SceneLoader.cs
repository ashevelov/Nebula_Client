using UnityEngine;
using System.Collections;
using UnityEditor;

public class SceneLoader : EditorWindow{

    [MenuItem("Space/Scene Loader")]
    static void ShowLoader() {
        SceneLoader w = EditorWindow.GetWindow<SceneLoader>();
        w.Show();
    }

    void OnGUI() {
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("MAP")) {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Misc/MAP.unity");
        }
        if (GUILayout.Button("SELECT CHARACTER"))
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Misc/select_character.unity");
        }
        if (GUILayout.Button("Humans Start")) {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Humans/H1.unity");
        }
        if (GUILayout.Button("Borguzands Start")) {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Borguzands/B1.unity");
        }
        if(GUILayout.Button("Criptizids Start"))
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Criptizids/E1.unity");
        }
        if (GUILayout.Button("Neutral Start"))
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.OpenScene("Assets/_Scenes/Neutral/N1.unity");
        }
        EditorGUILayout.EndVertical();
    }
}
