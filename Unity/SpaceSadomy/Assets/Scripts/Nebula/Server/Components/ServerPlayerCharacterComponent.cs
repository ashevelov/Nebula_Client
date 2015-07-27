namespace Nebula.Server.Components {
    using UnityEngine;
    using System.Collections;

    [AddComponentMenu("Server/Objects/Components/Player Character")]
    public class ServerPlayerCharacterComponent : ServerBotCharacterComponent {

        public override ComponentSubType subType {
            get {
                return ComponentSubType.character_player;
            }
        }

    }
}
