using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

namespace GunBall.Mirror
{
    public class JoinLobbyMenu : NetworkBehaviour
    {
        [SerializeField] private NetworkLobbyManager networkManager;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private Button joinButton;

        private void OnEnable()
        {
            NetworkLobbyManager.OnClientConnected += HandleClientConnected;
            NetworkLobbyManager.OnClientDisconnected += HandleClientDisconnected;
        }
        private void OnDisable()
        {
            NetworkLobbyManager.OnClientConnected += HandleClientConnected;
            NetworkLobbyManager.OnClientDisconnected += HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            networkManager.networkAddress = ipAddressInputField.text;
            networkManager.StartClient();

            joinButton.interactable = false;
        }
        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            landingPagePanel.SetActive(false);
            gameObject.SetActive(false);
        }
        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}