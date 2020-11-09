using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GunBall.Weapons;
using GunBall.Player;
using GunBall.Ball;

namespace GunBall.Game
{
    public class GameManagerGeneral : MonoBehaviour
    {
        #region Singleton
        public static GameManagerGeneral gameManager;
        #endregion

        [Header("Game Score")]
        [SerializeField] public static int team1Score, team2Score;
        [SerializeField] GeneralBall gameBall;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] List<PlayerController> Team1List = new List<PlayerController>();
        [SerializeField] List<PlayerController> Team2List = new List<PlayerController>();
        [Header("Game Score Display")]
        [SerializeField] TMP_Text team1ScoreDisplay, team2ScoreDisplay;
        [Header("Game")]
        [SerializeField] Vector3 ballOriginPosition;
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
        public void GoalScored(int team)
        {
            if(team == 1)
            {
                team1Score++;
                team1ScoreDisplay.text = team1Score.ToString();
            }
            else
            {
                team2Score++;
                team2ScoreDisplay.text = team2Score.ToString();
            }
            Destroy(gameBall.GetComponent<Rigidbody>());
            gameBall.transform.position = ballOriginPosition;
        }
        //TEMP
        public void TestSpawn()
        {
            PlayerController newPlayer = Instantiate(playerPrefab).GetComponent<PlayerController>();
            newPlayer.PlayerSetUp(gameBall);
            AddPlayerToTeam(newPlayer);
        }
        void EndGame()
        {
            /*snap*/

        }
        private void Awake()
        {
            #region Singleton Set Up
            if (gameManager != null && gameManager != this)
            {
                Destroy(gameObject);
            }
            else
            {
                gameManager = this;
            }
            #endregion
        }
        private void Start()
        {
            GameSetUp(1, 0);
        }
    }
}