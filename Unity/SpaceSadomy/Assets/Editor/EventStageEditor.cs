using UnityEngine;
using System.Collections;
using UnityEditor;
using Game.Space.Editor;
using System.Collections.Generic;

public class EventStageEditor : EditorWindow {

    private static EventStage eventStage;

    private static void SetCurrentStage(EventStage stage)
    {
        eventStage = stage;
    }

    public static void ShowEventStageEditor(EventStage stage)
    {
        EventStageEditor.SetCurrentStage(stage);
        EventStageEditor w = EditorWindow.GetWindow<EventStageEditor>();
        w.Show();
    }

    private List<Transition> transitionsForDelete = new List<Transition>();
    private Vector2 scrollPosition = Vector2.zero;

    void OnGUI()
    {
        if (eventStage != null)
        {
            EditorGUILayout.BeginVertical();
            eventStage.Id = EditorGUILayout.IntField("Stage id: ", eventStage.Id);
            eventStage.StartText = EditorGUILayout.TextField("Start text id: ", eventStage.StartText);
            eventStage.TaskText = EditorGUILayout.TextField("Task text id: ", eventStage.TaskText);
            eventStage.IsFinal = EditorGUILayout.Toggle("Is Final Stage: ", eventStage.IsFinal);
            eventStage.IsSuccess = EditorGUILayout.Toggle("Is Success Stage: ", eventStage.IsSuccess);
            eventStage.Timeout = (EventStageTimeout)EditorGUILayout.EnumPopup("Timeouted: ", eventStage.Timeout);

            if(GUILayout.Button("Add New Transition"))
            {
                Transition transition = new Transition();
                eventStage.Transitions.Add(transition);
                EventTransitionEditor.ShowTransitionWindow(transition);
            }

            this.transitionsForDelete.Clear();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach(var transition in eventStage.Transitions)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(transition.ToString());
                if(GUILayout.Button("Edit"))
                {
                    EventTransitionEditor.ShowTransitionWindow(transition);
                }
                if(GUILayout.Button("Delete"))
                {
                    this.transitionsForDelete.Add(transition);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            if(this.transitionsForDelete.Count > 0 )
            {
                foreach(var transition in this.transitionsForDelete)
                {
                    eventStage.Transitions.Remove(transition);
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}
