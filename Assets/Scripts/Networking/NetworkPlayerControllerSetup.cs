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
        PlayerController player;
        PlayerInput playerInput;
        NetworkTransform networkTransform;
        Camera playerCamera;
        Canvas playerCanvas;
        private void Start()
        {
            player = gameObject.GetComponent<PlayerController>();
            playerInput = gameObject.GetComponent<PlayerInput>();
            playerCamera = gameObject.GetComponentInChildren<Camera>();
            playerCanvas = gameObject.GetComponentInChildren<Canvas>();

            if(GetComponentInParent<NetworkPlayer>().isLocalPlayer)
            {
                player.enabled = true;
                playerInput.enabled = true;
                playerCamera.enabled = true;
                playerCanvas.enabled = true;
            }
        }
    }
}