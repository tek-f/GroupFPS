using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace GunBall.Game
{
    public class Goal : NetworkBehaviour
    {
        [SerializeField] int teamID;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ball"))
            {
                GameManagerGeneral.singleton.CmdGoalScored(teamID);

            }
        }
    }
}