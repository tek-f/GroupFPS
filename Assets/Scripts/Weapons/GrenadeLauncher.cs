using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Weapons;

namespace GunBall.Weapons
{
    public class GrenadeLauncher : GeneralGun
    {
        [SerializeField] GameObject grenadePrefab;
        [SerializeField] float shotDelay;
        [SerializeField] float lastShotTimeStamp;
        [SerializeField] float launchForce;
        public override void Shoot()
        {
            if (Time.time - lastShotTimeStamp > shotDelay && currentClip > 0)
            {
                GameObject grenadeInstance = Instantiate(grenadePrefab, gameObject.transform.position, gameObject.transform.rotation);
                grenadeInstance.transform.SetParent(null);
                grenadeInstance.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * launchForce);
                lastShotTimeStamp = Time.time;
                currentClip--;
            }
        }
        protected override void Awake()
        {
            damage = 10;
            range = 0;
            maxClip = 30;
            //set the guns player location
            base.Awake();
        }
    }
}