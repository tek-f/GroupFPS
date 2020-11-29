using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Game;

namespace GunBall.Ball
{
    public class GeneralBall : MonoBehaviour
    {
        public static GeneralBall singleton;
        private void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
                return;
            }
            else if(singleton != this)
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            GameManagerGeneral.singleton.ballOriginPosition = transform.position;
        }
    }
}