namespace Nebula.Server.Components {
    using Common;
    using System.Xml.Linq;
    using UnityEngine;
    using System;

    [AddComponentMenu("Server/Objects/Components/Follow Path Non Combat AI")]
    public class ServerFollowPathNonCombatAIComponent : ServerBaseAIComponent {

        public Transform[] path;


        public override ComponentSubType subType {
            get {
                return ComponentSubType.ai_follow_path_non_combat;
            }
        }

        public override XElement ToXElement() {
            var result = base.ToXElement();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < path.Length; i++) {
                sb.Append(FormatVector(path[i].position));
                if (i != (path.Length - 1)) {
                    sb.Append(";");
                }
            }
            result.SetAttributeValue("path", sb.ToString());
            return result;
        }

        void OnDrawGizmos() {
            if(path == null || path.Length == 0 ) { return; }
            Gizmos.color = Color.green;
            for(int i = 0; i < path.Length; i++) {
                Gizmos.DrawSphere(path[i].position, 1);
                if(i < path.Length - 1) {
                    Gizmos.DrawLine(path[i].position, path[i + 1].position);
                } else if(i == path.Length - 1) {
                    Gizmos.DrawLine(path[i].position, path[0].position);
                }
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, path[0].position);
        }


    }
}
