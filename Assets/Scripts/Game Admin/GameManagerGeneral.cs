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
        [SerializeField] GameObject playerPrefab;
        [SerializeField] List<PlayerController> Team1List = new List<PlayerController>();
        [SerializeField] List<PlayerController> Team2List = new List<PlayerController>();

        void GameSetUp(int _team1NumberofPlayers, int _team2NumberofPlayers)
        {
            #region Player/Team SetUp
            //Add each player to the correct team list
            #endregion

            //TEMP
            PlayerController newPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
            Team1List.Add(newPlayer);
            newPlayer.PlayerSetUp(gameBall);
        }
        private void Start()
        {
            GameSetUp(1, 0);
        }

    }
}