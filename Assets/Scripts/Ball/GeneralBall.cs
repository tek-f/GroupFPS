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

        public void ResetPosition()
        {
            transform.position = startPosition;
        }

        [ClientRpc]
        public void RpcResetPosition()
        {
            ResetPosition();
        }

        [Command]
        public void CmdResetPosition()
        {
            RpcResetPosition();
        }
        private void Start()
        {
            GameManagerGeneral.singleton.ballOriginPosition = transform.position;
            startPosition = transform.position;
        }
    }
}