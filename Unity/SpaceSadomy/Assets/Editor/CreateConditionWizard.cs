using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Game.Space.Editor;

public class CreateConditionWizard : EditorWindow {

    
    public static void ShowCreateConditionWindow(Condition c)
    {
        SetCurrentCondition(c);
		var w = EditorWindow.GetWindow<CreateConditionWizard>();
		w.Show();
    }

	private static Condition condition;

	private static void SetCurrentCondition(Condition c) {
		condition = c;
	}


    void OnGUI()
    {
		if( condition != null ) {
        	EditorGUILayout.BeginVertical();
        	condition.Type = (ConditionType)EditorGUILayout.EnumPopup("Condition type: ", condition.Type);
        	condition.VarName = EditorGUILayout.TextField("Condition variable name: ", condition.VarName);
        	condition.VarType = (VarType)EditorGUILayout.EnumPopup("Condition variable type: ", condition.VarType);
        	condition.Value = EditorGUILayout.TextField("Condition variable value: ", condition.Value);
        	EditorGUILayout.EndVertical();
		}
    }

}
