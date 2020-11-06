using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

namespace GunBall.Mirror
{
    public class NetworkLobbyManager : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;//-------------
        [Scene] [SerializeField] private string menuScene = string.Empty;
        [Scene] [SerializeField] public string levelScene = string.Empty;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private NetworkLobbyPlayer gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayer> roomPlayers { get; } = new List<NetworkRoomPlayer>();
        public List<NetworkLobbyPlayer> gamePlayers { get; } = new List<NetworkLobbyPlayer>();

        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("NetworkPrefabs").ToList();
        }

        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("NetworkPrefabs");
            print(spawnablePrefabs.Length);
            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientDisconnect(NetworkConnection networkConnection)
        {
            base.OnClientDisconnect(networkConnection);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection networkConnection)
        {
            if (numPlayers >= maxConnections)
            {
                networkConnection.Disconnect();
                return;
            }
            if (SceneManager.GetActiveScene().path != menuScene)
            {
                NetworkRoomPlayer roomPlayerLobby = Instantiate(roomPlayerPrefab);
            }
        }
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayer>();

                roomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }
        public override void OnServerAddPlayer(NetworkConnection networkConnection)
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                bool isLeader = roomPlayers.Count == 0;

                NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(networkConnection, roomPlayerInstance.gameObject);
            }
        }
        public override void OnStopServer()
        {
            roomPlayers.Clear();
        }
        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in roomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }
        public bool IsReadyToStart()
        {
            if (numPlayers < minPlayers)
            {
                return true;
            }
            foreach (var player in roomPlayers)
            {
                if (!player.isReady)
                {
                    return false;
                }
            }
            Debug.LogWarning("IsReadyTooStart not returned");
            return true;
        }
        public void StartGame()
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if (!IsReadyToStart())
                {
                    return;
                }
                if (levelScene != null)
                {
                    ServerChangeScene(levelScene);
                }
            }
        }
        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Assets/Scenes/GameScene_"))
            {
                for (int i = roomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = roomPlayers[i].connectionToClient;
                    var gamePlayersInstance = Instantiate(gamePlayerPrefab);
                    gamePlayersInstance.SetDisplayName(roomPlayers[i].displayName);

                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayersInstance.gameObject, true);
                }
            }
            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Assets/Scenes/GameScene_"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }
    }
}