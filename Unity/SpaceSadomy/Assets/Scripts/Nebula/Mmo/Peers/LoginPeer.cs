using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

namespace Nebula.Mmo.Peers {
    public class LoginPeer : PhotonPeer {
        public LoginPeer(IPhotonPeerListener listener, ConnectionProtocol protocol)
            : base(listener, protocol) {
        }
    }
}
