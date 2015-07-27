using UnityEngine;
using System.Collections;
using UnityEditor;
using Nebula.Server;
using System.Xml.Linq;
using System.Linq;
using Nebula.Server.Components;

public class ZoneXMLUtility : MonoBehaviour {

    [MenuItem("Space/Replace Zone XML")]
    static void Replace() {
        var zone = GameObject.FindObjectOfType<Zone>();
        if(!zone) {
            Debug.LogWarning("Not found zone object");
            return;
        }

        XElement zonesElement = new XElement("zones");
        XElement zoneElement = new XElement("zone");
        zonesElement.Add(zoneElement);

        zoneElement.SetAttributeValue("id", zone.zoneID);
        zoneElement.SetAttributeValue("name", zone.zoneName);
        zoneElement.SetAttributeValue("level", zone.level.ToString());
        zoneElement.SetAttributeValue("owned_race", ((int)zone.race).ToString());
        zoneElement.SetAttributeValue("world_type", zone.worldType.ToString());

        XElement planets = new XElement("planets");
        zoneElement.Add(planets);

        XElement npcGroups = new XElement("npc_groups");
        zoneElement.Add(npcGroups);

        XElement npcs = new XElement("npcs");
        zoneElement.Add(npcs);

        var npcObjects = zone.GetComponentsInChildren<NPC>();
        if(!CheckNPCDuplicates(npcObjects)) {
            return;
        }

        foreach(var npcObj in npcObjects) {
            if (npcObj.gameObject.activeSelf) {
                WriteNPC(npcObj, npcs);
            }
        }

        XElement asteroids = new XElement("asteroids");
        zoneElement.Add(asteroids);
        var asteroidObjects = zone.GetComponentsInChildren<ASTEROID>();
        if(!CheckAsteroidDuplicates(asteroidObjects)) {
            return;
        }
        foreach(var aObject in asteroidObjects) {
            if (aObject.gameObject.activeSelf) {
                WriteASTEROID(aObject, asteroids);
            }
        }

        XElement objectsElement = new XElement("nebula_objects");
        zoneElement.Add(objectsElement);
        WriteNebulaObjects(objectsElement, zone.transform);

        XElement events = new XElement("events");
        zoneElement.Add(events);
        var eventObjects = zone.GetComponentsInChildren<EVENT>();
        foreach(var evtObj in eventObjects) {
            if (evtObj.gameObject.activeSelf) {
                WriteEVENT(evtObj, events);
            }
        }

        XElement inputs = new XElement("inputs");
        zoneElement.Add(inputs);
        WriteZoneInputs(inputs);

        string fullPath = System.IO.Path.Combine(Application.dataPath, "Resources/Data/Zones/" + zone.zoneID + ".xml");
        zonesElement.Save(fullPath);
        Debug.Log(fullPath);
        AssetDatabase.Refresh(ImportAssetOptions.Default);

    }

    private static void WriteNebulaObjects(XElement objectsElement, Transform parent ) {
        foreach (Transform t in parent) {
            if (t.gameObject.activeSelf) {
                Nebula.Server.Components.ServerComponent[] serverComponents = t.GetComponents<Nebula.Server.Components.ServerComponent>();
                if (serverComponents.Length > 0) {
                    XElement objectElement = new XElement("nebula_object");
                    objectElement.SetAttributeValue("id", t.name);
                    objectElement.SetAttributeValue("position", FormatVector3(t.position));
                    objectElement.SetAttributeValue("rotation", FormatVector3(t.rotation.eulerAngles));
                    if (t.GetComponent<ServerNebulaObjectComponent>()) {
                        objectElement.SetAttributeValue("script", t.GetComponent<ServerNebulaObjectComponent>().script);
                    } else {
                        objectElement.SetAttributeValue("script", string.Empty);
                    }
                    foreach (var servComp in serverComponents) {
                        objectElement.Add(servComp.ToXElement());
                    }
                    objectsElement.Add(objectElement);
                }

                WriteNebulaObjects(objectsElement, t);
            }
        }
    }

    private static bool CheckNPCDuplicates(NPC[] npcArr) {
        var duplicates = npcArr.Select(n => n.npcID).GroupBy(s => s).SelectMany(grp => grp.Skip(1)).ToList();
        if(duplicates.Count > 0 ) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var s in duplicates) {
                sb.AppendLine(string.Format("Duplicated NPC ID: {0}", s));
            }
            EditorUtility.DisplayDialog("Error", sb.ToString(), "Close");
            return false;
        }
        return true;
    }

    private static bool CheckAsteroidDuplicates(ASTEROID[] astrs) {
        var duplicates = astrs.Select(a => a.asteroidID).GroupBy(s => s).SelectMany(grp => grp.Skip(1)).ToList();
        if(duplicates.Count > 0 ) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var s in duplicates) {
                sb.AppendLine(string.Format("Duplicated ASTEROID ID: {0}", s));
            }
            EditorUtility.DisplayDialog("Error", sb.ToString(), "Close");
            return false;
        }
        return true;
    }


    private static void WriteZoneInputs(XElement parent) {
        XElement zoneMin = new XElement("input");
        zoneMin.SetAttributeValue("key", "zone_min");
        zoneMin.SetAttributeValue("value", FormatVector3(new Vector3(-200, -200, -200)));
        zoneMin.SetAttributeValue("type", "vector");

        XElement zoneMax = new XElement("input");
        zoneMax.SetAttributeValue("key", "zone_max");
        zoneMax.SetAttributeValue("value", FormatVector3(new Vector3(200, 200, 200)));
        zoneMax.SetAttributeValue("type", "vector");

        XElement reachRadius = new XElement("input");
        reachRadius.SetAttributeValue("key", "reach_radius");
        reachRadius.SetAttributeValue("value", (5).ToString());
        reachRadius.SetAttributeValue("type", "float");
        parent.Add(zoneMin);
        parent.Add(zoneMax);
        parent.Add(reachRadius);
    }

    private static void WriteEVENT(EVENT evt, XElement parent) {
        XElement e = new XElement("event");
        e.SetAttributeValue("id", evt.eventID);
        e.SetAttributeValue("cooldown", ((int)evt.cooldown).ToString());
        e.SetAttributeValue("radius", ((int)evt.radius).ToString());
        e.SetAttributeValue("description", evt.description);
        e.SetAttributeValue("position", FormatVector3(evt.transform.position));

        XElement inputs = new XElement("inputs");
        e.Add(inputs);

        parent.Add(e);
    }

    private static void WriteASTEROID(ASTEROID asteroid, XElement parent) {
        XElement a = new XElement("asteroid");
        a.SetAttributeValue("index", asteroid.asteroidID.ToString());
        a.SetAttributeValue("force_create", asteroid.forceCreate.ToString());
        a.SetAttributeValue("respawn", (int)asteroid.respawnInterval);
        a.SetAttributeValue("data_id", asteroid.dataID);
        a.SetAttributeValue("model", asteroid.modelID);
        a.SetAttributeValue("position", FormatVector3(asteroid.transform.position));
        a.SetAttributeValue("rotation", FormatVector3(asteroid.transform.rotation.eulerAngles));
        parent.Add(a);
    }

    private static void WriteNPC(NPC npc, XElement parent) {
        XElement npcElement = new XElement("npc");
        npcElement.SetAttributeValue("id", npc.npcID);
        npcElement.SetAttributeValue("name", npc.npcName);
        npcElement.SetAttributeValue("level", npc.level.ToString());
        npcElement.SetAttributeValue("race", npc.race.ToString());
        npcElement.SetAttributeValue("position", FormatVector3(npc.transform.position));
        npcElement.SetAttributeValue("rotation", FormatVector3(npc.transform.rotation.eulerAngles));
        npcElement.SetAttributeValue("respawn_interval", (int)npc.respawnInterval);
        npcElement.SetAttributeValue("workshop", npc.workshop.ToString());
        npcElement.SetAttributeValue("difficulty", npc.difficulty.ToString());
        npcElement.SetAttributeValue("fraction", npc.fraction.ToString());

        XElement ai = new XElement("ai");
        ai.SetAttributeValue("name", npc.movingType.ToString());
        switch(npc.movingType) {
            case MovingType.FreeFlyAtBox:
                ai.SetAttributeValue("attack_moving_type", npc.freeFlyAtBoxAIType.battleMovingType.ToString());
                Vector3 min = new Vector3(npc.freeFlyAtBoxAIType.corners.min.X, npc.freeFlyAtBoxAIType.corners.min.Y, npc.freeFlyAtBoxAIType.corners.min.Z);
                Vector3 max = new Vector3(npc.freeFlyAtBoxAIType.corners.max.X, npc.freeFlyAtBoxAIType.corners.max.Y, npc.freeFlyAtBoxAIType.corners.max.Z);
                ai.SetAttributeValue("min", FormatVector3(min));
                ai.SetAttributeValue("max", FormatVector3(max));
                break;
            case MovingType.FreeFlyNearPoint:
                ai.SetAttributeValue("attack_moving_type", npc.freeFlyNearPointAIType.battleMovingType.ToString());
                ai.SetAttributeValue("radius", ((int)npc.freeFlyNearPointAIType.radius).ToString());
                break;
            case MovingType.OrbitAroundPoint:
                ai.SetAttributeValue("attack_moving_type", npc.orbitAroundPointAIType.battleMovingType.ToString());
                ai.SetAttributeValue("phi_speed", npc.orbitAroundPointAIType.phiSpeed.ToString());
                ai.SetAttributeValue("theta_speed", npc.orbitAroundPointAIType.thetaSpeed.ToString());
                ai.SetAttributeValue("radius", ((int)npc.orbitAroundPointAIType.radius).ToString());
                break;
            case MovingType.Patrol:
                Vector3 firstPoint = new Vector3(npc.patrolAIType.firstPoint.X, npc.patrolAIType.firstPoint.Y, npc.patrolAIType.firstPoint.Z);
                Vector3 secondPoint = new Vector3(npc.patrolAIType.secondPoint.X, npc.patrolAIType.secondPoint.Y, npc.patrolAIType.secondPoint.Z);
                ai.SetAttributeValue("attack_moving_type", npc.patrolAIType.battleMovingType.ToString());
                ai.SetAttributeValue("first", FormatVector3(firstPoint));
                ai.SetAttributeValue("second", FormatVector3(secondPoint));
                break;
            case MovingType.None:
                ai.SetAttributeValue("attack_moving_type", npc.noneAIType.battleMovingType.ToString());
                break;
            case MovingType.FollowPathCombat:
                var data = npc.GetFollowPathAIType();
                ai.SetAttributeValue("attack_moving_type", data.battleMovingType.ToString());
                ai.SetAttributeValue("path", PathString(npc.path));
                CheckPath(npc);
                break;
        }
        npcElement.Add(ai);
        parent.Add(npcElement);
    }

    private static void CheckPath(NPC npc) {
        if(npc.path == null || npc.path.Length < 2) {
            EditorUtility.DisplayDialog("ERROR", string.Format("NPC {0} not has valid path, path length must be greater or equal 2", npc.npcID), "Ok");

        }
    }

    private static string FormatVector3(Vector3 v) {
        return string.Format("{0},{1},{2}", (int)v.x, (int)v.y, (int)v.z);
    }

    private static string PathString(Transform[] path) {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < path.Length; i++) {
            if (path[i] != null) {
                sb.Append(FormatVector3(path[i].position));
                if (i != (path.Length - 1)) {
                    sb.Append(";");
                }
            }
        }
        return sb.ToString();
    }

}
