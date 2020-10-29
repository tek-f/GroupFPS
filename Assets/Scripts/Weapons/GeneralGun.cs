using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Player;
using TMPro;

namespace GunBall.Weapons
{
    public class GeneralGun : MonoBehaviour
    {
        #region Variables
        [Header("Set Up")]
        [SerializeField] Camera playerCamera;//reference var for the players camera
        [SerializeField] PlayerController player = null;
        [Header("Gun Metrics")]
        Vector3 gunPlayerLocation = new Vector3(0.32f, -0.293f, 0.662f);//the location the gun is relative to the players camera when the gun is equiped by tbe player, used when the gun is picked up by the player
        public Vector3 GunPlayerLocation
        {
            get
            {
                return gunPlayerLocation;
            }
        }
        protected float range;//the guns range
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
        [Header("UI")]
        [SerializeField] protected TMP_Text clipText;
        [SerializeField] protected TMP_Text ammoPoolText;
        [Header("Animation")]
        Animator animator;
        #endregion

        public void PlayerSetUp(GameObject _player)
        {
            player = _player.GetComponent<PlayerController>();
            playerCamera = _player.GetComponentInChildren<Camera>();
            clipText = _player.GetComponent<PlayerReferences>().clipText;
            ammoPoolText = _player.GetComponent<PlayerReferences>().ammoPoolText;
        }

        public virtual void Shoot()
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
                        hit.transform.GetComponent<Killable>().TakeDamage(damage, player);
                    }
                    if(hit.transform.GetComponent<Rigidbody>())
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce((hit.transform.position - gameObject.transform.position) * 10f, ForceMode.Impulse);
                    }
                }
                currentClip--;
                UpdateUI();
            }
        }
        public virtual void Reload()
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
                UpdateUI();
            }
        }
        public virtual void UpdateUI()
        {
            clipText.text = currentClip + " / " + maxClip;
            ammoPoolText.text = currentAmmoPool.ToString();
        }
        protected virtual void Awake()
        {
            currentClip = maxClip;
            currentAmmoPool = maxClip * 3;
        }
    }
}