using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GunBall.Mirror
{
    public class NetworkLobbyPlayer : NetworkBehaviour
    {
        [SyncVar] private string displayName = "Loading...";

        private NetworkLobbyManager room;
        private NetworkLobbyManager Room
        {
            get
            {
                if (room != null)
                {
                    return room;
                }
                return room = NetworkManager.singleton as NetworkLobbyManager;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.gamePlayers.Add(this);
        }
        public override void OnStopClient()
        {
            Room.gamePlayers.Remove(this);
        }
        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
    }
}