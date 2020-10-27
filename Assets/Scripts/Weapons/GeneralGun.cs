using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroupFPS.Weapons
{
    public class GeneralGun : MonoBehaviour
    {
        #region Variables
        [Header("Set Up")]
        [SerializeField] Camera playerCamera;
        [Header("Gun Metrics")]
        protected float damage, range;
        [Header("Animation")]
        Animator animator;
        #endregion

        public void Shoot()
        {
            Ray shot = playerCamera.ScreenPointToRay(new Vector3(0.5f, 0.5f));
        }
        protected void Reload()
        {

        }
    }
}