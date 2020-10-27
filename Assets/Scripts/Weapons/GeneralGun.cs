﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GroupFPS.Weapons
{
    public class GeneralGun : MonoBehaviour
    {
        #region Variables
        [Header("Set Up")]
        [SerializeField] Camera playerCamera;
        [Header("Gun Metrics")]
        [SerializeField] protected float range;
        [Header("UI")]
        [SerializeField] TMP_Text clipText;
        [SerializeField] TMP_Text ammoPoolText;

        public float Range
        {
            get
            {
                return range;
            }
        }
        protected int damage, maxClip, currentClip, currentAmmoPool;
        public int Damage
        {
            get
            {
                return damage;
            }
        }
        public int MaxClip
        {
            get
            {
                return maxClip;
            }
        }
        public int CurrentClip
        {
            get
            {
                return currentClip;
            }
        }
        public int CurrentAmmoPool
        {
            get
            {
                return currentAmmoPool;
            }
        }
        [Header("Animation")]
        Animator animator;
        #endregion

        public void Shoot()
        {
            if (currentClip > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
                {
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red, range);
                    if (hit.transform.GetComponent<Killable>())
                    {
                        Debug.Log("target hit, " + damage + " damage taken");
                        hit.transform.GetComponent<Killable>().TakeDamage(damage);
                    }
                }
                currentClip--;
                clipText.text = currentClip + " / " + maxClip;
            }
        }
        public void Reload()
        {
            if(currentClip < maxClip && currentAmmoPool > 0)
            {
                if(maxClip - currentClip > currentAmmoPool)
                {
                    currentClip += currentAmmoPool;
                    currentAmmoPool = 0;
                }
                else
                {
                    currentAmmoPool -= (maxClip - currentClip);
                    currentClip = maxClip;
                }
                clipText.text = currentClip + " / " + maxClip;
                ammoPoolText.text = currentAmmoPool.ToString();
            }
        }
        private void Awake()
        {
            damage = 5;
            maxClip = 30;
            currentClip = maxClip;
            currentAmmoPool = maxClip * 3;
        }
    }
}