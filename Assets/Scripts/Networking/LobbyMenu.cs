using UnityEngine;
using UnityEngine.UI;

namespace GunBall.MirrorTutorial
{
    public class LobbyMenu : MonoBehaviour
    {
        /// <summary>
        /// Array of LobbyPlayerDisplay scripts, which are on the players.
        /// </summary>
        [SerializeField]
        LobbyPlayerDisplay[] playerDisplays;
        /// <summary>
        /// The start game button.
        /// </summary>
        [SerializeField]
        Button startButton;
        /// <summary>
        /// Button to toggle ready state.
        /// </summary>
        [SerializeField]
        Button readyButton;
        /// <summary>
        /// Reference to Input
        /// </summary>
        [SerializeField]
        InputField playerNameInput;
        /// <summary>
        /// 
        /// </summary>
        GameNetworkManager network;
        /// <summary>
        /// 
        /// </summary>
        NetworkPlayer localPlayer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_player"></param>
        public void OnPlayerConnect(NetworkPlayer _player)
        {
            for (int i = 0; i < playerDisplays.Length; i++)
            {
                LobbyPlayerDisplay display = playerDisplays[i];

                if(!display.Filled)
                {
                    display.AssignPlayer(_player, i);
                    if(_player.isLocalPlayer)
                    {
                        localPlayer = _player;
                        readyButton.onClick.AddListener(display.ToggleReadyState);
                    }

                    break;
                }


            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnClickStart() => localPlayer.StartGame();
        public void SetReadyPlayer(int _index, bool _isReady) => playerDisplays[_index].SetReadyState(_isReady);

        void OnEndEditName(string _name)
        {
            if(localPlayer != null)
            {
                localPlayer.SetName(_name);
            }
        }

        private void Start()
        {
            network = GameNetworkManager.singleton as GameNetworkManager;
            playerNameInput.onEndEdit.AddListener(OnEndEditName);
            startButton.interactable = false;
        }

        private void Update()
        {
            if(network.IsHost)
            {
                foreach (LobbyPlayerDisplay display in playerDisplays)
                {
                    if (!display.Ready && display.Filled)
                    {
                        if (startButton.interactable)
                        {
                            startButton.interactable = false;
                        }
                        return;
                    }
                }
                if (!startButton.interactable)
                {
                    startButton.interactable = true;
                }
            }
        }

    }
}