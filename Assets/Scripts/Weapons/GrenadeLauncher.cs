using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;

namespace GunBall.Weapons
{
    public class GrenadeLauncher : GeneralGun
    {
        /// <summary>
        /// Prefab for the grenade object
        /// </summary>
        [SerializeField] GameObject grenadePrefab;
        /// <summary>
        /// The time delay between shots
        /// </summary>
        [SerializeField] float shotDelay;
        /// <summary>
        /// Time stamp of when the previous shot happened. Used to check that the shot delay has passed.
        /// </summary>
        [SerializeField] float lastShotTimeStamp;
        /// <summary>
        /// The force that is applied to the grenade 
        /// </summary>
        [SerializeField] float launchForce;
        /// <summary>
        /// Grenade Launcher specific Shoot method. Overrides GeneralGun Shoot()
        /// </summary>
        public override void Shoot()
        {
            if (Time.time - lastShotTimeStamp > shotDelay && currentClip > 0)
            {
                GameObject grenadeInstance = Instantiate(grenadePrefab, gameObject.transform.position, gameObject.transform.rotation);
                grenadeInstance.transform.SetParent(null);
                grenadeInstance.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * launchForce);
                lastShotTimeStamp = Time.time;
                currentClip--;
                UpdateUI();
            }
        }
        /// <summary>
        /// Grenade Launcher specific Awake(). Sets up ammo variables.
        /// </summary>
        protected override void Awake()
        {
            damage = 10;
            range = 0;
            maxClip = 6;
            //set the guns player location
            base.Awake();
        }
    }
}