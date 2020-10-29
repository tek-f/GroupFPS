using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;
using GunBall.Player;
using GunBall.Ball;

namespace GunBall.Game
{
    public class GameManagerGeneral : MonoBehaviour
    {
        [SerializeField] GeneralBall gameBall;
        [SerializeField] List<PlayerController> Team1List = new List<PlayerController>();
        [SerializeField] List<PlayerController> Team2List = new List<PlayerController>();

        void GameSetUp()
        {
            #region Player/Team SetUp
            //Add each player to the correct team list
            #endregion
        }

    }
}