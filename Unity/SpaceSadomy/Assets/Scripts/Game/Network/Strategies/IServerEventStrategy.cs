namespace Nebula {
    using ExitGames.Client.Photon;

    public interface IServerEventStrategy {
        void Handle(NetworkGame game, EventData eventData);
    }
}