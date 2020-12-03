using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GunBall.Game;

namespace GunBall.Ball
{
    public class GeneralBall : NetworkBehaviour
    {
        public GeneralBall singleton;

        public Vector3 startPosition;

        void SetActive(bool _bool)
        {
            gameObject.SetActive(_bool);
        }
        [ClientRpc]
        void RpcSetActive(bool _bool)
        {
            SetActive(_bool);
        }
        [Command]
        public void CmdSetActive(bool _bool)
        {
            RpcSetActive(_bool);
        }

        public void ResetPosition()
        {
            transform.position = startPosition;
        }
        private void Awake()
        {
            GameManagerGeneral.singleton.gameBall = this;
        }
        private void Start()
        {
            GameManagerGeneral.singleton.ballOriginPosition = transform.position;
            startPosition = transform.position;
        }
    }
}