using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GunBall.MirrorTutorial
{
    public class LobbyPlayerDisplay : MonoBehaviour
    {
        /// <summary>
        /// Tracks if the 
        /// </summary>
        public bool Filled {  get { return button.interactable; } }
        /// <summary>
        /// 
        /// </summary>
        public bool Ready { get; private set; } = false;
        /// <summary>
        /// Reference to the Text element that displays the players name.
        /// </summary>
        [SerializeField]
        Text playerName;
        /// <summary>
        /// Reference to the Button element that is the background/display for the playerName Text element.
        /// </summary>
        [SerializeField]
        Button button;
        /// <summary>
        /// Reference to the Image element that is the indicator for the players ready state.
        /// </summary>
        [SerializeField]
        Image readyIndicator;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        Color readyColor = Color.green;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        Color unreadyColor = Color.red;
        /// <summary>
        /// 
        /// </summary>
        NetworkPlayer player;
        int index;

        public void AssignPlayer(NetworkPlayer _player, int _index)
        {
            player = _player;
            index = _index;
            button.interactable = true;
        }
        public void SetReadyState(bool _isReady) => Ready = _isReady;
        public void ToggleReadyState()
        {
            SetReadyState(!Ready);
            player.ReadyPlayer(index, Ready);
        }
        private void Start()
        {
            button.interactable = false;
        }
        private void Update()
        {
            playerName.text = player != null && string.IsNullOrEmpty(player.username) ? player.username : "Player";
            readyIndicator.color = Ready ? readyColor : unreadyColor;
        }
    }
}