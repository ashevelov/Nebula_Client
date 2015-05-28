namespace Nebula {
    using Common;
    using ExitGames.Client.Photon;
    using System.Collections;
    using UnityEngine;

    public class UseSkillEventStrategy : IServerEventStrategy {

        public void Handle(NetworkGame game, EventData eventData) {
            Hashtable skillProperties = eventData.Parameters[(byte)ParameterCode.Properties] as Hashtable;
            bool success = skillProperties.GetValue<bool>(GenericEventProps.success, false);

            if (!success) {
                string message = skillProperties.GetValue<string>(GenericEventProps.message, string.Empty);
                Debug.LogError("Skill use fail with message: {0}".f(message));
                return;
            }

            Hashtable castInfo = skillProperties.GetValue<Hashtable>(GenericEventProps.cast_info, new Hashtable());
            string sourceItemId = skillProperties.GetValue<string>(GenericEventProps.source, string.Empty);
            byte sourceItemType = skillProperties.GetValue<byte>(GenericEventProps.source_type, (byte)0);
            Hashtable skillData = skillProperties.GetValue<Hashtable>(GenericEventProps.data, new Hashtable());
            string skillId = skillData.GetValue<string>(GenericEventProps.id, string.Empty);

            Item sourceItem;
            if (!game.TryGetItem(sourceItemType, sourceItemId, out sourceItem)) {
                Debug.LogError("Not founded source skill item {0}:{1}".f(sourceItemId, sourceItemType.toItemType()));
                return;
            }

            sourceItem.UseSkill(skillProperties);
        }
    }

}