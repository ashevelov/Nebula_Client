using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula.Mmo.Games.Strategies {
    public interface IGameStrategy {
        GameState State { get; }
        void OnEventReceive(BaseGame game, EventData eventData);
        void OnOperationReturn(BaseGame game, OperationResponse operationResponse);
        void OnPeerStatusChanged(BaseGame game, StatusCode statusCode);
    }
}
