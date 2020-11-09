using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Player;

namespace GunBall.Weapons
{
    public class Shotgun : GeneralGun
    {
        /// <summary>
        /// Used to randomise the direction of the individual raycasts when shooting
        /// </summary>
        [SerializeField] float shotRandomiseModifier;
        /// <summary>
        /// Randomises the x and y values of the Vector3 cameraForward, 
        /// </summary>
        /// <param name="cameraForward"></param>
        /// <returns></returns>
        Vector3 RandomiseRaycastDirection(Vector3 cameraForward)
        {
            Vector3 result = cameraForward;
            result.y += Random.Range(-shotRandomiseModifier, shotRandomiseModifier);
            result.z += Random.Range(-shotRandomiseModifier, shotRandomiseModifier);
            return result;
        }
        public override void Shoot()
        {
            if (currentClip > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    RaycastHit hit;
                    Vector3 pelletDirection = RandomiseRaycastDirection(playerCamera.transform.forward);
                    Debug.DrawRay(playerCamera.transform.position, pelletDirection * range, Color.red, range);
                    if (Physics.Raycast(playerCamera.transform.position, pelletDirection, out hit, range))
                    {
                        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, Color.red, range);
                        if (hit.transform.GetComponent<Killable>())
                        {
                            Debug.Log("target hit, " + damage + " damage taken");
                            hit.transform.GetComponent<Killable>().TakeDamage(damage, player);
                        }
                        if (hit.transform.GetComponent<Rigidbody>())
                        {
                            hit.transform.GetComponent<Rigidbody>().AddForce((hit.transform.position - gameObject.transform.position) * 10f, ForceMode.Impulse);
                        }
                    }
                }
                currentClip--;
                UpdateUI();
            }
        }
        protected override void Awake()
        {
            damage = 10;
            range = 25;
            maxClip = 10;
            //set the guns player location
            base.Awake();
        }
    }
}