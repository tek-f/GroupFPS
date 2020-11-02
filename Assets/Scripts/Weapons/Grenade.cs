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
        bool exploded = false;

        void Explosion()
        {
            print("boom");
            Collider[] temp = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

            if (temp.Length > 0)
            {
                foreach (var thing in temp)
                {
                    //add force to rigidbodies within sphere
                    if (thing.GetComponent<Rigidbody>())
                    {
                        thing.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, gameObject.transform.position, explosionRadius);
                    }
                    //add force to character controllers within the sphere
                    if (thing.GetComponent<ImpactReceiver>())
                    {

                    }
                    //do damage to players within the sphere

                }
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(!exploded && Input.GetKeyDown(KeyCode.B))
            {
                Explosion();
                exploded = true;
            }
        }
    }
}