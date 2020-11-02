using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Player;

namespace GunBall.Weapons
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] float explosionRadius;
        [SerializeField] float explosionForce;
        [SerializeField] float damage;
        [SerializeField] float explosionDelay;
        [SerializeField] float instantiationTimeStamp;
        bool exploded = false;

        void Explosion()
        {
            print("boom");
            Collider[] temp = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

            if (temp.Length > 0)
            {
                foreach (var thing in temp)
                {
                    if (thing != this.GetComponent<Collider>())
                    {
                        //add force to rigidbodies within sphere
                        if (thing.GetComponent<Rigidbody>())
                        {
                            thing.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, gameObject.transform.position, explosionRadius);
                        }
                        //add force to character controllers within the sphere
                        if (thing.GetComponent<ImpactReceiver>())
                        {
                            thing.GetComponent<ImpactReceiver>().AddImpact(thing.transform.position - gameObject.transform.position, explosionForce);
                        }
                        //do damage to players within the sphere

                    }
                }
            }
        }
        void Start()
        {
            instantiationTimeStamp = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - instantiationTimeStamp > explosionDelay)
            {
                Explosion();
                exploded = true;
                Destroy(gameObject);
            }
        }
    }
}