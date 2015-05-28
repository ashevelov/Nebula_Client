using UnityEngine;
using System.Collections;
using UnityEditor;
using Game.Space.Editor;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

public class EventEditor : EditorWindow {

    private static WorldEvent worldEvent;

    private Vector2 scrollPosition = Vector2.zero;
    private List<EventStage> stagesToDelete = new List<EventStage>();

    [MenuItem("Space/Show Event Editor")]
    static void ShowEventEditor()
    {
        EventEditor w = EventEditor.GetWindow<EventEditor>();
        w.Show();
    }

    private static void SetEvent(WorldEvent evt)
    {
        worldEvent = evt;
    }

    void OnGUI()
    {
        if(worldEvent != null )
        {
            EditorGUILayout.BeginVertical();
            worldEvent.Id = EditorGUILayout.TextField("Id: ", worldEvent.Id);
            worldEvent.Name = EditorGUILayout.TextField("Name: ", worldEvent.Name);
            worldEvent.Description = EditorGUILayout.TextField("Description: ", worldEvent.Description);
            worldEvent.Cooldown = EditorGUILayout.IntField("Cooldown: ", worldEvent.Cooldown);
            worldEvent.RewardExp = EditorGUILayout.IntField("Reward Exp: ", worldEvent.RewardExp);
            worldEvent.RewardCoins = EditorGUILayout.IntField("Reward Coins: ", worldEvent.RewardCoins);
            worldEvent.Position = EditorGUILayout.Vector3Field("Position", worldEvent.Position);

            if(GUILayout.Button("Add New Stage"))
            {
                EventStage stage = new EventStage();
                worldEvent.Stages.Add(stage);
                EventStageEditor.ShowEventStageEditor(stage);
            }

            if(GUILayout.Button("Draw Event"))
            {
                DrawEventEditor.ShowEventDrawer(worldEvent);
            }

            if(GUILayout.Button("Save"))
            {
                //Debug.Log(Application.dataPath);
                //Debug.Log(VarType.@int.ToString());
                XElement events = new XElement("events");
                events.Add(worldEvent.ToXElement());
                var path = EditorUtility.SaveFilePanelInProject("Save Event", "EVM.xml", "xml", "Save Event File");

                if(!string.IsNullOrEmpty(path))
                    File.WriteAllText(path, events.ToString());
            }

            if(GUILayout.Button("Remove Event"))
            {
                worldEvent = null;
                return;
            }

            stagesToDelete.Clear();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach(var stage in worldEvent.Stages)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(stage.ToString());
                if(GUILayout.Button("Edit"))
                {
                    EventStageEditor.ShowEventStageEditor(stage);
                }
                if(GUILayout.Button("Delete"))
                {
                    stagesToDelete.Add(stage);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            foreach(var s in stagesToDelete)
            {
                worldEvent.Stages.Remove(s);
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.BeginVertical();
            if(GUILayout.Button("Create New Event"))
            {
                WorldEvent newEvent = new WorldEvent();
                SetEvent(newEvent);
            }

            if (GUILayout.Button("Load"))
            {
                var path = EditorUtility.SaveFilePanelInProject("Save Event", "EVM.xml", "xml", "Save Event File");
                if (!string.IsNullOrEmpty(path))
                {
                    XDocument document = XDocument.Load(path);
                    if (document.Element("events") != null)
                    {
                        if (document.Element("events").Element("event") != null)
                        {
                            worldEvent = WorldEvent.Parse(document.Element("events").Element("event"));
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    void OnDestroy()
    {
        //worldEvent = null;
    }
}
