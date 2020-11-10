using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GunBall.Player;
using Mirror;

namespace GunBall.Mirror
{
    public class NetworkPlayerControllerSetup : MonoBehaviour
    {
        NetworkPlayerController player;
        PlayerInput playerInput;
        NetworkTransform networkTransform;
        Camera playerCamera;
        private void Start()
        {
            player = gameObject.GetComponent<NetworkPlayerController>();
            playerInput = gameObject.GetComponent<PlayerInput>();
            networkTransform = gameObject.GetComponent<NetworkTransform>();
            playerCamera = gameObject.GetComponentInChildren<Camera>();
            if (NetworkServer.active)
            {
                player.enabled = true;
                playerInput.enabled = true;
                networkTransform.enabled = true;
                playerCamera.enabled = true;
            }
            else
            {
                player.enabled = true;
                playerInput.enabled = true;
                playerCamera.enabled = true;
            }
        }
    }
}