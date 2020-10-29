using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunBall.Game
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] int teamID;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ball"))
            {
                GameManagerGeneral.gameManager.GoalScored(teamID);
            }
        }
    }
}