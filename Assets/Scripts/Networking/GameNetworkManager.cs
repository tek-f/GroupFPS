using Mirror;
namespace GunBall.Mirror
{
    public class GameNetworkManager : NetworkManager
    {
        public static GameNetworkManager instance
        {
            get { return singleton as GameNetworkManager; }
        }
        /// <summary>
        /// Tracks if the local player is the host.
        /// </summary>
        public bool IsHost { get; private set; } = false;
        public override void OnStartHost() => IsHost = true;
    }
}