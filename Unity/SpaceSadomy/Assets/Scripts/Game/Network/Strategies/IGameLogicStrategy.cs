// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGameLogicStrategy.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The i event dispatcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Nebula
{
    using System;
    using System.Collections.Generic;

    using ExitGames.Client.Photon;
    using Common;
    using Game.Network;

    /// <summary>
    /// The i event dispatcher.
    /// </summary>
    [CLSCompliant(false)]
    public interface IGameLogicStrategy
    {
        /// <summary>
        /// Gets State.
        /// </summary>
        GameState State { get; }

        /// <summary>
        /// The on event receive.
        /// </summary>
        /// <param name="gameLogic">
        /// The game logic.
        /// </param>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        void OnEventReceive(NetworkGame gameLogic, EventData eventData);

        /// <summary>
        /// The on operation return.
        /// </summary>
        /// <param name="gameLogic">
        /// The game logic.
        /// </param>
        /// <param name="operationResponse">
        /// The operation response.
        /// </param>
        void OnOperationReturn(NetworkGame gameLogic, OperationResponse operationResponse);

        /// <summary>
        /// The on peer status callback.
        /// </summary>
        /// <param name="gameLogic">
        /// The game logic.
        /// </param>
        /// <param name="returnCode">
        /// The return code.
        /// </param>
        void OnPeerStatusCallback(NetworkGame gameLogic, StatusCode returnCode);

        /// <summary>
        /// The on update.
        /// </summary>
        /// <param name="gameLogic">
        /// The game logic.
        /// </param>
        void OnUpdate(NetworkGame gameLogic);

        /// <summary>
        /// The send operation.
        /// </summary>
        /// <param name="game">
        /// The mmo game.
        /// </param>
        /// <param name="operationCode">
        /// The operation code.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="sendReliable">
        /// The send Reliable.
        /// </param>
        /// <param name="channelId">
        /// The channel Id.
        /// </param>
        void SendOperation(NetworkGame game, OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId);
    }
}