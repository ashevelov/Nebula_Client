namespace Nebula.Server {
    using UnityEngine;
    using System.Collections;
    using Common;
    using System.Collections.Generic;
    using System.Linq;

    [AddComponentMenu("Server/Objects/NPC")]
    public class NPC : MonoBehaviour {
        public string npcID;
        public string npcName;
        public Race race;
        public FractionType fraction;
        public float respawnInterval;
        public Workshop workshop;
        public int level;
        public bool levelFromZone;
        public Difficulty difficulty;
        public MovingType movingType;
        public FreeFlyAtBoxAIType freeFlyAtBoxAIType;
        public FreeFlyNearPointAIType freeFlyNearPointAIType;
        public OrbitAroundPointAIType orbitAroundPointAIType;
        public PatrolAIType patrolAIType;
        public NoneAIType noneAIType;


        [SerializeField]
        public AttackMovingType pathAttackType;
        [SerializeField]
        public Transform[] path;

        public FollowPathAIType GetFollowPathAIType() {
            List<Vector3> lstPath = new List<Vector3>();
            if(path != null ) {
                foreach(var t in path) {
                    lstPath.Add(t.position);
                }
            }
            return new FollowPathAIType { battleMovingType = pathAttackType, path = lstPath.Select(p => new GameMath.Vector3(p.x, p.y, p.z)).ToArray() };
        }

        void OnDrawGizmos() {
            switch(movingType) {
                case MovingType.FreeFlyAtBox:
                    DrawFreeFlyAtBoxGizmos();
                    break;
                case MovingType.FreeFlyNearPoint:
                    DrawFreeFlyNearPointGizmos();
                    break;
                case MovingType.OrbitAroundPoint:
                    DrawOrbitAroundPointGizmos();
                    break;
                case MovingType.Patrol:
                    DrawPatrolGizmos();
                    break;
                case MovingType.None:
                    DrawNoneGizmos();
                    break;
                case MovingType.FollowPathCombat:
                    DrawFollowPathCombatAIGizmos();
                    break;
            }
        }


        void DrawFollowPathCombatAIGizmos() {
            if (path == null || path.Length == 0) { return; }
            Gizmos.color = Color.green;
            for (int i = 0; i < path.Length; i++) {
                Gizmos.DrawSphere(path[i].position, 1);
                if (i < path.Length - 1) {
                    Gizmos.DrawLine(path[i].position, path[i + 1].position);
                } else if (i == path.Length - 1) {
                    Gizmos.DrawLine(path[i].position, path[0].position);
                }
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, path[0].position);
        }

        void DrawFreeFlyAtBoxGizmos() {
            Gizmos.color = Color.green;
            Vector3 min = new Vector3(freeFlyAtBoxAIType.corners.min.X, freeFlyAtBoxAIType.corners.min.Y, freeFlyAtBoxAIType.corners.min.Z);
            Vector3 max = new Vector3(freeFlyAtBoxAIType.corners.max.X, freeFlyAtBoxAIType.corners.max.Y, freeFlyAtBoxAIType.corners.max.Z);
            Vector3 center = (min + max) * .5f;
            Vector3 size = new Vector3(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y), Mathf.Abs(max.z - min.z));
            Gizmos.DrawWireCube(center, size);
            Gizmos.color = Color.white;

            Gizmos.DrawLine(transform.position, center);
        }

        void DrawFreeFlyNearPointGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, freeFlyNearPointAIType.radius);
        }

        void DrawOrbitAroundPointGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, orbitAroundPointAIType.radius);
        }

        void DrawPatrolGizmos() {
            Gizmos.color = Color.green;
            Vector3 firstPoint = new Vector3(patrolAIType.firstPoint.X, patrolAIType.firstPoint.Y, patrolAIType.firstPoint.Z);
            Vector3 secondPoint = new Vector3(patrolAIType.secondPoint.X, patrolAIType.secondPoint.Y, patrolAIType.secondPoint.Z);

            float cubeSize = 3;

            Gizmos.DrawWireCube(firstPoint, Vector3.one * cubeSize);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(secondPoint, Vector3.one * cubeSize);
            Gizmos.color = Color.red;

            Gizmos.DrawLine(firstPoint, secondPoint);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, firstPoint);
            Gizmos.DrawLine(transform.position, secondPoint);
        }

        void DrawNoneGizmos() {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 1);
        }
    }

}
