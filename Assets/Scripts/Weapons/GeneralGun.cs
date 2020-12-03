using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Player;
using TMPro;

namespace GunBall.Weapons
{
    public class GeneralGun : MonoBehaviour
    {
        #region Properties
        ///<summary>
        ///Public property for gunPlayerLocation
        /// </summary>
        public Vector3 GunPlayerLocation
        {
            get
            {
                return gunPlayerLocation;
            }
        }
        ///<summary>
        ///Public property for gun range
        /// </summary>
        public float Range
        {
            get
            {
                return range;
            }
        }
        ///<summary>
        ///The public property for the guns damage
        /// </summary>
        public int Damage
        {
            get
            {
                return damage;
            }
        }
        ///<summary>
        ///The public property for the guns maxClip
        /// </summary>
        public int MaxClip
        {
            get
            {
                return maxClip;
            }
        }
        ///<summary>
        ///The public property for the guns currentClip
        /// </summary>
        public int CurrentClip
        {
            get
            {
                return currentClip;
            }
        }
        ///<summary>
        ///The public property for the guns currentAmmoPool
        /// </summary>
        public int CurrentAmmoPool
        {
            get
            {
                return currentAmmoPool;
            }
        }
        #endregion
        #region Variables
        [Header("Set Up")]
        ///<summary>
        ///Reference to the local player camera
        /// </summary>
        [SerializeField] protected Camera playerCamera;
        ///<summary>
        ///Reference to the local player
        /// </summary>
        [SerializeField] protected PlayerController player = null;
        [Header("Gun Metrics")]
        ///<summary>
        ///The position relative to the players local space that the gun is set to when equiped
        /// </summary>
        Vector3 gunPlayerLocation = new Vector3(0.32f, -0.293f, 0.662f);
        ///<summary>
        ///The guns range
        /// </summary>
        protected float range;
        ///<summary>
        ///The guns damage
        /// </summary>
        protected int damage = 5;
        ///<summary>
        ///The amount of bullets the guns clip holds when full
        /// </summary>
        protected int maxClip;
        ///<summary>
        ///The current amount of bullets in the guns clip
        /// </summary>
        protected int currentClip;
        ///<summary>
        ///The current remaining total pool of ammo that the gun has
        /// </summary>
        protected int currentAmmoPool;
        [Header("UI")]
        ///<summary>
        ///The reference to the Text that displays the guns clip current and max, i.e. "currentClip / maxClip"
        /// </summary>
        [SerializeField] protected TMP_Text clipText;
        ///<summary>
        ///The reference to the Text that displays the guns currentAmmoPool
        /// </summary>
        [SerializeField] protected TMP_Text ammoPoolText;
        [Header("Animation")]
        ///<summary>
        ///The reference to the local players animator
        /// </summary>
        Animator animator;
        #endregion
        /// <summary>
        /// Setup function for the player.
        /// </summary>
        /// <param name="_player"> The game object of the player to be set up </param>
        public void PlayerSetUp(GameObject _player)
        {
            player = _player.GetComponent<PlayerController>();
            playerCamera = _player.GetComponentInChildren<Camera>();
            clipText = _player.GetComponent<PlayerReferences>().clipText;
            ammoPoolText = _player.GetComponent<PlayerReferences>().ammoPoolText;
        }
        /// <summary>
        /// Guns base shoot method
        /// </summary>
        public virtual void Shoot()
        {
            if (currentClip > 0)
            {
                currentClip--;
                UpdateUI();
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
                {
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red, range);
                    if (hit.transform.GetComponent<PlayerController>())
                    {
                        Debug.Log("target hit, " + damage + " damage taken");
                        hit.transform.GetComponent<Killable>().TakeDamage(damage, player);
                    }
                    if(hit.transform.GetComponent<Rigidbody>())
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce((hit.transform.position - gameObject.transform.position) * 10f, ForceMode.Impulse);
                    }
                }
            }
        }
        /// <summary>
        /// Guns general reload method
        /// </summary>
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
        /// <summary>
        /// Updates clipText and ammoPoolText according to gun ammo variables, see currentClip, maxClip and currentAmmoPool
        /// </summary>
        public virtual void UpdateUI()
        {
            clipText.text = currentClip + " / " + maxClip;
            ammoPoolText.text = currentAmmoPool.ToString();
        }
        /// <summary>
        /// General gun awake. Sets up ammo variables. Overriden in scripts that inherit from General Gun
        /// </summary>
        protected virtual void Awake()
        {
            currentClip = maxClip;
            currentAmmoPool = maxClip * 3;
        }
    }
}