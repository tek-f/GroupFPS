using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using GunBall.Player;

namespace GunBall.Mirror
{
    public class NetworkSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        NetworkLobbyManager networkLobbyManager;

        private static List<Transform> team1SpawnPoints = new List<Transform>();
        private static List<Transform> team2SpawnPoints = new List<Transform>();

        private int team1NextIndex = 0, team2NextIndex = 0;
        int teamTracker = 1;

        public static void AddSpawnPoint(Transform transform)
        {
            if(transform.GetComponent<NetworkSpawnPoint>().TeamID == 1)
            {
                team1SpawnPoints.Add(transform);

                team1SpawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
            }
            else
            {
                team2SpawnPoints.Add(transform);

                team2SpawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
            }
        }

        public static void RemoveSpawnPoint(Transform transform)
        {
            if(transform.GetComponent<NetworkSpawnPoint>().TeamID == 1)
            {
                team1SpawnPoints.Remove(transform);
            }
            else
            {
                team2SpawnPoints.Remove(transform);
            }
        }

        public override void OnStartServer() => NetworkLobbyManager.OnServerReadied += SpawnPlayer;

        [ServerCallback]
        private void OnDestroy() => NetworkLobbyManager.OnServerReadied -= SpawnPlayer;
        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            if (teamTracker == 1)
            {
                Transform spawnPoint = team1SpawnPoints.ElementAtOrDefault(team1NextIndex);

                if (spawnPoint == null)
                {
                    Debug.LogError("Spawn point transform not found");
                    return;
                }

                GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                playerInstance.GetComponent<NetworkPlayerController>().TeamID = 1;
                NetworkServer.Spawn(playerInstance, conn);

                team1NextIndex++;
                if (team1NextIndex >= team1SpawnPoints.Count)
                {
                    team1NextIndex = 0;
                }
                teamTracker = 2;
            }
            else
            {
                Transform spawnPoint = team1SpawnPoints.ElementAtOrDefault(team1NextIndex);

                if (spawnPoint == null)
                {
                    Debug.LogError("Spawn point transform not found");
                    return;
                }

                GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                playerInstance.GetComponent<NetworkPlayerController>().TeamID = 2;
                NetworkServer.Spawn(playerInstance, conn);

                team2NextIndex++;
                if (team2NextIndex >= team2SpawnPoints.Count)
                {
                    team2NextIndex = 0;
                }
                teamTracker = 1;
            }
        }
    }
}