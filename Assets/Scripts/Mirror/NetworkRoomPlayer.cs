using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace GunBall.Mirror
{
    public class NetworkRoomPlayer : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button levelSelectButton;

        [SyncVar(hook = nameof(HandleDisplayNameChange))]
        public string displayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChange))]
        public bool isReady = false;

        private bool isLeader;
        public bool IsLeader
        {
            set
            {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
                levelSelectButton.gameObject.SetActive(value);
            }
        }
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

        public override void OnStartAuthority()
        {
            Debug.Log("is it running?");
            CMDSetDisplayName(PlayerNameInput.DisplayName);
            lobbyUI.SetActive(true);
        }

        public override void OnStartClient()
        {
            Room.roomPlayers.Add(this);
            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.roomPlayers.Remove(this);
            UpdateDisplay();
        }
        public void HandleReadyStatusChange(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChange(string oldValue, string newValue) => UpdateDisplay();

        public void SetLevelScene(int sceneID)
        {
            room.levelScene = SceneUtility.GetScenePathByBuildIndex(sceneID);
            print(room.levelScene);
        }

        private void UpdateDisplay()
        {
            if (!isLocalPlayer)
            {
                foreach (var player in Room.roomPlayers)
                {
                    if (player.isLocalPlayer)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting for player...";
                playerReadyTexts[i].text = string.Empty;
            }

            for (int i = 0; i < Room.roomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.roomPlayers[i].displayName;
                playerReadyTexts[i].text = Room.roomPlayers[i].isReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
            }
        }
        public void HandleReadyToStart(bool readyToStart)
        {
            if (!isLeader) { return; }

            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CMDSetDisplayName(string display)
        {
            this.displayName = display;
        }
        [Command]
        public void CMDReadyUp()
        {
            isReady = !isReady;

            Room.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CMDStartGame()
        {
            if (Room.roomPlayers[0].connectionToClient != connectionToClient)
            {
                return;
            }

            Room.StartGame();
        }
    }
}