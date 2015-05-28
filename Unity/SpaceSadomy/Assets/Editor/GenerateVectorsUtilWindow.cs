using UnityEngine;
using System.Collections;
using UnityEditor;

public class GenerateVectorsUtilWindow : EditorWindow {

    private string countStr = string.Empty;
    private string xMinStr = string.Empty;
    private string xMaxStr = string.Empty;
    private string yMinStr = string.Empty;
    private string yMaxStr = string.Empty;
    private string zMinStr = string.Empty;
    private string zMaxStr = string.Empty;
    private string outStr = string.Empty;

    private int iCount;
    private float fXMin, fXMax, fYMin, fYMax, fZMin, fZMax;


    private string centerXStr = string.Empty;
    private string centerYStr = string.Empty;
    private string centerZStr = string.Empty;
    private string radiusStr = string.Empty;

    private float centerX, centerY, centerZ, radius;

    public enum WindowMode { GenerateInBox, GenerateInSphere }

    private WindowMode windowMode = WindowMode.GenerateInBox;

    [MenuItem("Space/Generate Vectors(Box)")]
    static void ShowEditor()
    {
        GenerateVectorsUtilWindow editor = EditorWindow.GetWindow<GenerateVectorsUtilWindow>();
        editor.SetWindowMode(GenerateVectorsUtilWindow.WindowMode.GenerateInBox);
        editor.Show();
    }

    [MenuItem("Space/Generate Vectors(Sphere)")]
    static void ShowEditorSphere()
    {
        GenerateVectorsUtilWindow editor = EditorWindow.GetWindow<GenerateVectorsUtilWindow>();
        editor.SetWindowMode(GenerateVectorsUtilWindow.WindowMode.GenerateInSphere);
        editor.Show();
    }

    public void SetWindowMode(GenerateVectorsUtilWindow.WindowMode windowMode)
    {
        this.windowMode = windowMode;
    }


    void OnGUI()
    {
        switch (this.windowMode)
        {
            case WindowMode.GenerateInBox:
                EditorGUILayout.BeginVertical();
                countStr = EditorGUILayout.TextField("Count:", countStr);
                EditorGUILayout.BeginHorizontal();
                xMinStr = EditorGUILayout.TextField("X Min:", xMinStr);
                yMinStr = EditorGUILayout.TextField("Y Min:", yMinStr);
                zMinStr = EditorGUILayout.TextField("Z Min:", zMinStr);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                xMaxStr = EditorGUILayout.TextField("X Max:", xMaxStr);
                yMaxStr = EditorGUILayout.TextField("Y Max:", yMaxStr);
                zMaxStr = EditorGUILayout.TextField("Z Max:", zMaxStr);
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Generate"))
                {
                    if (false == this.Convert())
                    {
                        this.outStr = "error of input data";
                    }
                    else
                    {
                        System.Text.StringBuilder sbOut = new System.Text.StringBuilder();
                        for (int i = 0; i < iCount; i++)
                        {
                            float x = Random.Range(fXMin, fXMax);
                            float y = Random.Range(fYMin, fYMax);
                            float z = Random.Range(fZMin, fZMax);
                            sbOut.AppendLine(string.Format("{0:F2},{1:F2},{2:F2}", x, y, z));
                        }
                        this.outStr = sbOut.ToString();
                    }
                }

                EditorGUILayout.TextArea(outStr);
                EditorGUILayout.EndVertical();
                break;
            case WindowMode.GenerateInSphere:
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                countStr = EditorGUILayout.TextField("Count: ", countStr);
                centerXStr = EditorGUILayout.TextField("Center X: ", centerXStr);
                centerYStr = EditorGUILayout.TextField("Center Y: ", centerYStr);
                centerZStr = EditorGUILayout.TextField("Center Z: ", centerZStr);
                EditorGUILayout.EndHorizontal();
                radiusStr = EditorGUILayout.TextField("Radius: ", radiusStr);

                if (GUILayout.Button("Generate"))
                {
                    if (false == this.ConvertCenterRadius())
                    {
                        this.outStr = "error of input data";
                    }
                    else
                    {
                        System.Text.StringBuilder sbOut = new System.Text.StringBuilder();
                        for (int i = 0; i < iCount; i++)
                        {
                            Vector3 result = new Vector3(centerX, centerY, centerZ) + Random.insideUnitSphere * radius;
                            sbOut.AppendLine(string.Format("{0:F1},{1:F1},{2:F1}", result.x, result.y, result.z));
                        }
                        this.outStr = sbOut.ToString();
                    }
                }
                EditorGUILayout.TextArea(outStr);
                EditorGUILayout.EndVertical();
                break;
        }
    }

    private bool Convert()
    {
        try
        {
            bool result =  int.TryParse(countStr, out iCount);
            result = result && float.TryParse(xMinStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fXMin);
            result = result && float.TryParse(yMinStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fYMin);
            result = result && float.TryParse(zMinStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fZMin);
            result = result && float.TryParse(xMaxStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fXMax);
            result = result && float.TryParse(yMaxStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fYMax);
            result = result && float.TryParse(zMaxStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out fZMax);
            return result;
        }
        catch (System.Exception ex)
        {
            Debug.Log("convert exception");
            Debug.Log(ex.Message);
            Debug.Log(ex.StackTrace);
            return false;
        }
    }

    private bool ConvertCenterRadius()
    {
        try
        {
            bool result = float.TryParse(centerXStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out centerX);
            result = result && float.TryParse(centerYStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out centerY);
            result = result && float.TryParse(centerZStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out centerZ);
            result = result && float.TryParse(radiusStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out radius);
            result = result && int.TryParse(countStr, out iCount);
            return result;
        }
        catch (System.Exception e)
        {
            Debug.Log("convert exception");
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
            return false;
        }
    }
}
