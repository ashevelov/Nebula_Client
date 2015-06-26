using UnityEngine;
using System.Collections;
using UnityEditor;
using Nebula.Server;
using Common;

[CustomEditor(typeof(Nebula.Server.NPC))]
public class NPCEditor : Editor {

    MovingType movingType;

    public override void OnInspectorGUI() {
        Nebula.Server.NPC npc = (Nebula.Server.NPC)target;

        movingType = npc.movingType;
        npc.npcID = EditorGUILayout.TextField("NPC ID:", npc.npcID);
        npc.npcName = EditorGUILayout.TextField("Name: ", npc.npcName);
        npc.race = (Race)EditorGUILayout.EnumPopup("NPC race:", npc.race);
        npc.fraction = (FractionType)EditorGUILayout.EnumPopup("NPC fraction:", npc.fraction);
        npc.respawnInterval = EditorGUILayout.FloatField("Respawn:", npc.respawnInterval);
        npc.workshop = (Workshop)EditorGUILayout.EnumPopup("NPC Workshop:", npc.workshop);
        npc.difficulty = (Difficulty)EditorGUILayout.EnumPopup("NPC Difficulty:", npc.difficulty);
        npc.levelFromZone = EditorGUILayout.Toggle("Level from Zone?", npc.levelFromZone);
        if(npc.levelFromZone) {
            var zone = npc.gameObject.GetComponentInParent<Zone>();
            if(zone != null ) {
                npc.level = zone.level;
            }
            EditorGUILayout.LabelField(string.Format("Level: {0}", npc.level));
        } else {
            npc.level = EditorGUILayout.IntField("Level: ", npc.level);
        }
        movingType = npc.movingType;
        npc.movingType = (MovingType)EditorGUILayout.EnumPopup("AI Type:", npc.movingType);
        if(npc.movingType != movingType) {
            Debug.Log("moving type changed");
        }
        EditorGUILayout.Separator();
        switch (npc.movingType) {
            case MovingType.FreeFlyAtBox:
                EditorGUILayout.BeginFadeGroup(1);
                npc.freeFlyAtBoxAIType.purchaseEnemy = EditorGUILayout.Toggle("Chase Enemy?", npc.freeFlyAtBoxAIType.purchaseEnemy);
                Vector3 cornerMin = new Vector3(npc.freeFlyAtBoxAIType.corners.min.X, npc.freeFlyAtBoxAIType.corners.min.Y, npc.freeFlyAtBoxAIType.corners.min.Z);
                Vector3 cornerMax = new Vector3(npc.freeFlyAtBoxAIType.corners.max.X, npc.freeFlyAtBoxAIType.corners.max.Y, npc.freeFlyAtBoxAIType.corners.max.Z);
                cornerMin = EditorGUILayout.Vector3Field("Corner Min", cornerMin);
                cornerMax = EditorGUILayout.Vector3Field("Corner Max", cornerMax);
                npc.freeFlyAtBoxAIType.corners.min.mx = cornerMin.x; npc.freeFlyAtBoxAIType.corners.min.my = cornerMin.y; npc.freeFlyAtBoxAIType.corners.min.mz = cornerMin.z;
                npc.freeFlyAtBoxAIType.corners.max.mx = cornerMax.x; npc.freeFlyAtBoxAIType.corners.max.my = cornerMax.y; npc.freeFlyAtBoxAIType.corners.max.mz = cornerMax.z;

                EditorGUILayout.EndFadeGroup();
                break;
            case MovingType.FreeFlyNearPoint:
                EditorGUILayout.BeginFadeGroup(1);
                npc.freeFlyNearPointAIType.purchaseEnemy = EditorGUILayout.Toggle("Chase Enemy?", npc.freeFlyNearPointAIType.purchaseEnemy);
                npc.freeFlyNearPointAIType.radius = EditorGUILayout.FloatField("Radius: ", npc.freeFlyNearPointAIType.radius);
                EditorGUILayout.EndFadeGroup();
                break;
            case MovingType.OrbitAroundPoint:
                EditorGUILayout.BeginFadeGroup(1);
                npc.orbitAroundPointAIType.purchaseEnemy = EditorGUILayout.Toggle("Chase Enemy?", npc.orbitAroundPointAIType.purchaseEnemy);
                npc.orbitAroundPointAIType.phiSpeed = EditorGUILayout.FloatField("PHI speed: ", npc.orbitAroundPointAIType.phiSpeed);
                npc.orbitAroundPointAIType.thetaSpeed = EditorGUILayout.FloatField("THETA speed: ", npc.orbitAroundPointAIType.thetaSpeed);
                npc.orbitAroundPointAIType.radius = EditorGUILayout.FloatField("Radius: ", npc.orbitAroundPointAIType.radius);
                EditorGUILayout.EndFadeGroup();
                break;
            case MovingType.Patrol:
                EditorGUILayout.BeginFadeGroup(1);
                npc.patrolAIType.purchaseEnemy = EditorGUILayout.Toggle("Chase Enemy?", npc.patrolAIType.purchaseEnemy);
                Vector3 firstPoint = new Vector3(npc.patrolAIType.firstPoint.X, npc.patrolAIType.firstPoint.Y, npc.patrolAIType.firstPoint.Z);
                Vector3 secondPoint = new Vector3(npc.patrolAIType.secondPoint.X, npc.patrolAIType.secondPoint.Y, npc.patrolAIType.secondPoint.Z);
                firstPoint = EditorGUILayout.Vector3Field("First Point:", firstPoint);
                secondPoint = EditorGUILayout.Vector3Field("Second Point:", secondPoint);
                npc.patrolAIType.firstPoint.mx = firstPoint.x; npc.patrolAIType.firstPoint.my = firstPoint.y; npc.patrolAIType.firstPoint.mz = firstPoint.z;
                npc.patrolAIType.secondPoint.mx = secondPoint.x; npc.patrolAIType.secondPoint.my = secondPoint.y; npc.patrolAIType.secondPoint.mz = secondPoint.z;
                EditorGUILayout.EndFadeGroup();

                break;
            case MovingType.None:
                EditorGUILayout.BeginFadeGroup(1);
                npc.noneAIType.purchaseEnemy = EditorGUILayout.Toggle("Purchase Enemy?", npc.noneAIType.purchaseEnemy);
                EditorGUILayout.EndFadeGroup();
                break;

        }

        if(GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
