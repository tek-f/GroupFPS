using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GunBall.Weapons;
using GunBall.Player;
using GunBall.Ball;
using Mirror;

namespace GunBall.Game
{
    public class GameManagerGeneral : NetworkBehaviour
    {
        #region Singleton
        public static GameManagerGeneral singleton;
        private void Awake()
        {
            #region Singleton Set Up
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }
            #endregion
        }
        #endregion

        [Header("Game Score")]
        [SerializeField] int scoreLimit;
        [SerializeField] public int team1Score, team2Score;
        public GeneralBall gameBall;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] List<PlayerController> Team1List = new List<PlayerController>();
        [SerializeField] List<PlayerController> Team2List = new List<PlayerController>();
        [Header("Game Score Display")]
        [SerializeField] TMP_Text team1ScoreDisplay, team2ScoreDisplay;
        [Header("Game")]
        public Vector3 ballOriginPosition;
        void GameSetUp(int _team1NumberofPlayers, int _team2NumberofPlayers)
        {
            #region Player/Team SetUp
            //Add each player to the correct team list
            #endregion

            //TEMP
            //PlayerController newPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
            //newPlayer.PlayerSetUp(gameBall);
            //AddPlayerToTeam(newPlayer);
        }
        public void AddPlayerToTeam(PlayerController player)
        {
            if(Team1List.Count > Team2List.Count)
            {
                Team2List.Add(player);
                player.TeamID = 2;
            }
            else
            {
                Team1List.Add(player);
                player.TeamID = 1;
            }
        }
        [Command]
        public void CmdGoalScored(int _teamID)
        {
            RpcGoalScored(_teamID);
        }
        [ClientRpc]
        public void RpcGoalScored(int _teamID)
        {
            GoalScored(_teamID);
        }
        public void GoalScored(int team)
        {
            if(team == 1)
            {
                team1Score++;
            }
            else
            {
                team2Score++;
            }
            Destroy(gameBall.GetComponent<Rigidbody>());
            gameBall.CmdResetPosition();
        }
        private void Start()
        {
            //GameSetUp(1, 0);
        }
    }
}