using ExitGames.Client.Photon;

namespace Nebula.Mmo.Peers {
    public class MasterPeer : PhotonPeer {
        public MasterPeer(IPhotonPeerListener listener, ConnectionProtocol protocol)
            : base(listener, protocol) {
        }
    }
}
