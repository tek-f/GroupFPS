using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


namespace GunBall.MirrorTutorial
{
    public class NetworkPlayer : NetworkBehaviour
    {
        /// <summary>
        /// String that is the username of the player. Is a SyncVar.
        /// </summary>
        [SyncVar]
        public string username = "Player";
        /// <summary>
        /// Prefab for the lobby player game object.
        /// </summary>
        [SerializeField]
        private GameObject lobbyPlayer;
        /// <summary>
        /// Prefab for the lobby player game object.
        /// </summary>
        [SerializeField]
        private GameObject gameplayerPlayer;
        /// <summary>
        /// Tracks if the player has connected to the UI.
        /// </summary>
        private bool connectedToLobbyUI = false;
        /// <summary>
        /// Reference to a LobbyMenu script.
        /// </summary>
        LobbyMenu lobby;
        /// <summary>
        /// Runs when the local player calls Start().
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            SceneManager.LoadSceneAsync("MenuScene_LobbyMenu", LoadSceneMode.Additive);
            base.OnStartLocalPlayer();
        }
        public void ReadyPlayer(int _index, bool _isReady)
        {
            if(isLocalPlayer)
            {
                CmdReadyPlayer(_index, _isReady);
            }
        }
        public void SetName(string _name)
        {
            if (isLocalPlayer)
            {
                CmdSetPlayerName(_name);
            }
        }
        public void StartGame()
        {
            if (isLocalPlayer)
            {
                CmdStartGame();
            }
        }
        private void Start()
        {
            lobbyPlayer.SetActive(true);
            gameplayerPlayer.SetActive(false);
        }
        private void Update()
        {
            if (lobby == null && lobbyPlayer.activeSelf)
            {
                lobby = FindObjectOfType<LobbyMenu>();
            }
            if(!connectedToLobbyUI && lobby != null)
            {
                lobby.OnPlayerConnect(this);
                connectedToLobbyUI = true;

            }
        }

        [Command] public void CmdReadyPlayer(int _index, bool _isReady) => RpcReadyPlayer(_index, _isReady);
        [ClientRpc] public void RpcReadyPlayer(int _index, bool _isReady) => lobby?.SetReadyPlayer(_index, _isReady);

        [Command] public void CmdSetPlayerName(string _name) => RpcSetPlayerName(_name);
        [ClientRpc] public void RpcSetPlayerName(string _name) => username = _name;

        [Command] public void CmdStartGame() => RpcStartGame();
        [ClientRpc]
        public void RpcStartGame()
        {
            NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
            foreach (NetworkPlayer player in players)
            {
                player.lobbyPlayer.SetActive(false);
                player.gameplayerPlayer.SetActive(true);

                if(player.isLocalPlayer)
                {
                    SceneManager.UnloadSceneAsync("MenuScene_LobbyMenu");
                    SceneManager.LoadSceneAsync("GameScene_OneVsOne", LoadSceneMode.Additive);

                    //enable/add fps controller and player handler here
                }
            }
        }
    }
}