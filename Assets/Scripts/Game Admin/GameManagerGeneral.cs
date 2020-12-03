using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GunBall.Weapons;
using GunBall.Player;
using GunBall.Ball;
using GunBall.Mirror;
using Mirror;

namespace GunBall.Game
{
    public class GameManagerGeneral : NetworkBehaviour
    {
        #region Singleton
        public static GameManagerGeneral singleton;
        #endregion

        [Header("Game Score")]
        [SerializeField] int scoreLimit;
        [SerializeField] public int team1Score, team2Score;
        public GeneralBall gameBall;
        [SerializeField] GameObject playerPrefab, ballPrefab;
        [SerializeField] List<PlayerController> playerList = new List<PlayerController>();
        [Header("Game Score Display")]
        [SerializeField] TMP_Text team1ScoreDisplay, team2ScoreDisplay;
        [Header("Game")]
        public Vector3 ballOriginPosition;
        void GameSetUp()
        {
            #region Player/Team SetUp
            //Add each player to the correct team list
            #endregion

            //TEMP
            //PlayerController newPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
            //newPlayer.PlayerSetUp(gameBall);
            //AddPlayerToTeam(newPlayer);

            Debug.Log(GameNetworkManager.instance.IsHost);

            if (GameNetworkManager.instance.IsHost)
            {
                Debug.Log("spawn game ball");
                ClientScene.RegisterPrefab(ballPrefab);
                GameObject ball = Instantiate(ballPrefab);
                NetworkServer.Spawn(ball);
            }
        }
        public void AddPlayerToList(PlayerController _player)
        {
            playerList.Add(_player);
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
                //team1ScoreDisplay.text = team1Score.ToString();
            }
            else
            {
                team2Score++;
                //team2ScoreDisplay.text = team2Score.ToString();
            }
            //Destroy(gameBall.GetComponent<Rigidbody>());
            //gameBall.CmdResetPosition();
        }
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

            //GameSetUp();
        }
    }
}