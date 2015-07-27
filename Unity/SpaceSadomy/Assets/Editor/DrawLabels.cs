using UnityEngine;
using System.Collections;
using UnityEditor;
using Nebula.Test.Gizmo;
using Nebula;

public class DrawLabels : MonoBehaviour {

    private static GUIStyle mLabelStyle;

    [DrawGizmo(GizmoType.InSelectionHierarchy| GizmoType.NotInSelectionHierarchy)]
    static void DrawWorldPointGizmosText(Transform t, GizmoType gizmoType) {
        if(t.GetComponent<WorldPointGizmo>()) {
            if(mLabelStyle == null ) {
                mLabelStyle = Utils.LoadDebugLabelStyle();
            }
            Handles.Label(t.position + Vector3.up, new GUIContent(t.GetComponent<WorldPointGizmo>().text.Color("orange")), mLabelStyle);
        }
    }
}
