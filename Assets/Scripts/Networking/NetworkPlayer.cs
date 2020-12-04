using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using GunBall.Player;

namespace GunBall.Mirror
{
    public class NetworkPlayer : NetworkBehaviour
    {
        //PlayerInput playerInput;
        //InputAction fireAction;
        public GameObject deathPanel;
        public int health = 20;
        public Vector3 respwanPosition;
        PlayerController playerControler;
        PlayerInput playerInput;
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

        public int teamID;

        public Vector3 respawnPosition;
        /// <summary>
        /// Runs when the local player calls Start().
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            SceneManager.LoadSceneAsync("MenuScene_LobbyMenu", LoadSceneMode.Additive);
        }
        public void ReadyPlayer(int _index, bool _isReady)
        {
            if (isLocalPlayer)
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
            if (!connectedToLobbyUI && lobby != null)
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

        public List<Transform> GetTeamPositions(List<Transform> allPositions, int teamID)
        {
            string teamIDString = "Team " + teamID.ToString();

            return allPositions.Where(position =>
            {
                string tag = position.tag;
                return tag.Contains(teamIDString);
            }).ToList();
        }

        public void TakeDamage(int _damage)
        {
            if (isLocalPlayer)
            { 
                health -= _damage;
                print(health);
                if (health <= 0)
                {
                    Death();
                }
            }
        }
        [ClientRpc]
        public void RpcTakeDamage(int _damage)
        {
            TakeDamage(_damage);
        }
        [Command]
        public void CmdTakeDamage(int _damage)
        {
            RpcTakeDamage(_damage);
        }
        public void PublicTakeDamage(int _damage)
        {
            if (isLocalPlayer)
            {
                CmdTakeDamage(_damage);
            }
        }
        void Death()
        {
            Debug.Log("death");
            gameplayerPlayer.transform.position = respawnPosition;
            health = 20;
            playerControler.enabled = false;
            playerInput.enabled = false;
            deathPanel.SetActive(true);
            Invoke("Respawn", 8);
        }
        [ClientRpc]
        void RpcDeath()
        {
            Debug.Log("RPC death");
            Death();
        }
        [Command]
        public void CmdDeath()
        {
            Debug.Log("CMD death");
            RpcDeath();
        }
        void Respawn()
        {
            if (isLocalPlayer)
            {
                playerControler.enabled = false;
                playerInput.enabled = false;
                deathPanel.SetActive(false);
            }
        }

        [ClientRpc]
        public void RpcStartGame()
        {
            NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
            int teamTracker = 0;
            foreach (NetworkPlayer player in players)
            {
                player.lobbyPlayer.SetActive(false);
                player.gameplayerPlayer.SetActive(true);
                PlayerController playerController = player.gameplayerPlayer.GetComponent<PlayerController>();
                playerController.TeamID = teamTracker;

                if(teamTracker == 0)
                {
                    teamTracker = 1;
                }
                else
                {
                    teamTracker = 0;
                }

                if (player.isLocalPlayer)
                {
                    SceneManager.UnloadSceneAsync("MenuScene_LobbyMenu");


                    // Get all NetworkStartPositions in the Game
                    List<Transform> startPositions = NetworkManager.startPositions;

                    //No start positions ?
                    if (startPositions.Count == 0)
                    {
                        Debug.LogWarning("No Start Positions Found");
                        return; // Exit method
                    }
                    var teamPositions = GetTeamPositions(startPositions, playerController.TeamID);

                    if (teamPositions.Count == 0)
                    {
                        Debug.LogWarning("No Team Positions Found");
                        return; // Exit method
                    }
                    var randomIndex = Random.Range(0, teamPositions.Count);
                    var randomPosition = teamPositions[randomIndex];
                    SpawnOffset spawnOffset = randomPosition.GetComponent<SpawnOffset>();
                    if (spawnOffset != null)
                    {
                        player.gameplayerPlayer.transform.position = spawnOffset.GetOffset();
                    }
                    respawnPosition = player.gameplayerPlayer.transform.position;

                    playerControler = player.gameplayerPlayer.GetComponent<PlayerController>();
                    playerInput = player.gameplayerPlayer.GetComponent<PlayerInput>();
                    //enable network player setup script
                    player.gameplayerPlayer.GetComponent<NetworkPlayerControllerSetup>().enabled = true;

                    
                }
            }
        }
        //private void OnFirePerformed(InputAction.CallbackContext _context)
        //{
        //    if (!gameplayerPlayer.GetComponent<PlayerController>().holdingBall)
        //    {
        //        gameplayerPlayer.GetComponent<PlayerController>().currentGun.Shoot();
        //    }
        //}
    }
}