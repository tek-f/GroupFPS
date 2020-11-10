using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;

namespace GunBall.Player
{
    public class Killable : MonoBehaviour
    {
        [SerializeField] protected bool alive;
        [SerializeField] protected int health = 25;

        public void TakeDamage(int damage, PlayerController attackingPlayer)
        {
            health -= damage;
            Debug.Log(damage + " damage taken, " + health + " health remaining");
            if(health <= 0)
            {
                Death();
            }
        }
        void Death()
        {
            //disable player controls

            Destroy(gameObject);
        }
        private void Start()
        {
            
        }
    }
}