using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroupFPS.Weapon
{
    public class GeneralGun : MonoBehaviour
    {
        #region Variables
        [Header("Gun Metrics")]
        protected float damage, range;
        [Header("Animation")]
        Animator animator;
        #endregion

        protected void Shoot()
        {
            //sends ray cast, distance of range
        }
        protected void Reload()
        {

        }
    }
}