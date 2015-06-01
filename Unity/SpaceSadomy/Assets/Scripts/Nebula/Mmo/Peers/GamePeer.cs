// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhotonPeer.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The photon peer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Nebula
{
    using System;
    using ExitGames.Client.Photon;

    public class GamePeer : ExitGames.Client.Photon.PhotonPeer
    {
        public GamePeer(IPhotonPeerListener listener, ConnectionProtocol protocol)
            : base(listener, protocol) {
            ChannelCount = Settings.ChannelCount;
        }
    }
}