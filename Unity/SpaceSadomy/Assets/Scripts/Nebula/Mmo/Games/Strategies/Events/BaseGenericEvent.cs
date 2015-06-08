namespace Nebula.Mmo.Games.Strategies.Events {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;
    using Common;

    public class BaseGenericEvent : BaseEventHandler {
        protected Hashtable Properties(EventData eventData) {
            return (Hashtable)eventData.Parameters[ParameterCode.EventData.toByte()];
        }

        protected object Data(EventData eventData) {
            return eventData.Parameters[(byte)ParameterCode.EventData];
        }
    }
}
