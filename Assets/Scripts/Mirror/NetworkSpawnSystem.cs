using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

namespace GunBall.Mirror
{
    public class NetworkSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;

        private static List<Transform> spawnPoints = new List<Transform>();

        private int nextIndex = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);

            spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        public override void OnStartServer() => NetworkLobbyManager.OnServerReadied += SpawnPlayer;

        [ServerCallback]
        private void OnDestroy() => NetworkLobbyManager.OnServerReadied -= SpawnPlayer;
        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

            if (spawnPoint == null)
            {
                Debug.LogError("Spawn point transform not found");
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.Spawn(playerInstance, conn);
            nextIndex++;
            if (nextIndex >= spawnPoints.Count)
            {
                nextIndex = 0;
            }
        }
    }
}