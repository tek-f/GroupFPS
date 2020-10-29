using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;

namespace GunBall.Player
{
    public class Killable : MonoBehaviour
    {
        protected bool alive;
        protected int health = 25;

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
            Destroy(gameObject);
        }
    }
}