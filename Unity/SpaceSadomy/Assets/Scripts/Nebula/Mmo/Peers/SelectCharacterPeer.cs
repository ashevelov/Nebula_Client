namespace Nebula.Mmo.Peers {
    using UnityEngine;
    using System.Collections;
    using ExitGames.Client.Photon;

    public class SelectCharacterPeer : PhotonPeer {
        public SelectCharacterPeer(IPhotonPeerListener listener, ConnectionProtocol protocol)
            : base(listener, protocol) {

        }
    }
}
