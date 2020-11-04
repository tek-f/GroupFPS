using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunBall.Mirror
{
    public class MultiplayerMenu : MonoBehaviour
    {
        [SerializeField] private NetworkLobbyManager networkManager;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel;
        public void HostLobby()
        {
            networkManager.StartHost();
            landingPagePanel.SetActive(false);
        }
    }
}