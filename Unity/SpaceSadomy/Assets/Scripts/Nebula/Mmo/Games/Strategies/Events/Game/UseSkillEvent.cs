namespace Nebula.Mmo.Games.Strategies.Events.Game {
    using Common;
    using ExitGames.Client.Photon;
    using Nebula.Mmo.Games;
    using ServerClientCommon;
    using System.Collections;
    using UnityEngine;

    public class UseSkillEvent: BaseEventHandler {

        public override void Handle(BaseGame game, EventData eventData) {
            Hashtable skillProperties = eventData.Parameters[(byte)ParameterCode.Properties] as Hashtable;
            bool success = skillProperties.GetValue<bool>((int)SPC.IsSuccess, false);

            if (!success) {
                string message = skillProperties.GetValue<string>((int)SPC.Message, string.Empty);
                Debug.LogError("Skill use fail with message: {0}".f(message));
                return;
            }

            Hashtable castInfo = skillProperties.GetValue<Hashtable>((int)SPC.Info, new Hashtable());
            string sourceItemId = skillProperties.GetValue<string>((int)SPC.Source, string.Empty);
            byte sourceItemType = skillProperties.GetValue<byte>((int)SPC.SourceType, (byte)0);
            Hashtable skillData = skillProperties.GetValue<Hashtable>((int)SPC.Data, new Hashtable());
            string skillId = skillData.GetValue<string>((int)SPC.Id, string.Empty);

            Item sourceItem;
            if (!((NetworkGame)game).TryGetItem(sourceItemType, sourceItemId, out sourceItem)) {
                Debug.LogError("Not founded source skill item {0}:{1}".f(sourceItemId, sourceItemType.toItemType()));
                return;
            }

            sourceItem.UseSkill(skillProperties);
        }
    }

}