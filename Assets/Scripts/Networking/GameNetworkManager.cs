using Mirror;
namespace GunBall.MirrorTutorial
{
    public class GameNetworkManager : NetworkManager
    {
        /// <summary>
        /// Tracks if the local player is the host.
        /// </summary>
        public bool IsHost { get; private set; } = false;
        public override void OnStartHost() => IsHost = true;
    }
}