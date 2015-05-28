using UnityEngine;
using System.Collections;
using UnityEditor;
using Game.Space.Editor;
using System.Collections.Generic;

public class EventTransitionEditor : EditorWindow {

	private static  Transition transition;


	public static void ShowTransitionWindow(Transition t)
	{
        EventTransitionEditor.SetCurrentTransition(t);
		EventTransitionEditor e = EditorWindow.GetWindow<EventTransitionEditor>();
		e.Show();
	}

	private static void SetCurrentTransition(Transition tr) {
		transition = tr;
	}

	private Vector2 scrollPosition = Vector2.zero;
    private List<Condition> conditionsToDelete = new List<Condition>();

	void OnGUI(){
		if(transition != null) 
		{
			EditorGUILayout.BeginVertical();
			transition.ToStage = EditorGUILayout.IntField("Target transition stage: ", transition.ToStage);

            if(GUILayout.Button("Create New Condition"))
            {
                Condition c = new Condition();
                transition.Conditions.Add(c);
                CreateConditionWizard.ShowCreateConditionWindow(c);
            }

            conditionsToDelete.Clear();

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			foreach(Condition c in transition.Conditions)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(c.ToString());
				if(GUILayout.Button("Edit"))
				{
					CreateConditionWizard.ShowCreateConditionWindow(c);
				}
                if(GUILayout.Button("Delete"))
                {
                    conditionsToDelete.Add(c);
                }
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();

            foreach(var c in conditionsToDelete)
            {
                transition.Conditions.Remove(c);
            }

			EditorGUILayout.EndVertical();
		}
	}
}
