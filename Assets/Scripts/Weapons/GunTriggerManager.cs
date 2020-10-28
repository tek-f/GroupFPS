using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GunBall.Player;

namespace GunBall.Weapons
{
    public class GunTriggerManager : MonoBehaviour
    {
        [SerializeField] GameObject gunPrefab;//prefab for the gun that is assigned to this trigger
        int teamID;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if(player.LoadoutCount < 2)
                {
                    Camera playerCamera = player.GetComponentInChildren<Camera>();
                    GeneralGun newGun = Instantiate(gunPrefab, playerCamera.transform).GetComponent<GeneralGun>();
                    newGun.PlayerSetUp(other.gameObject);
                    other.GetComponent<PlayerController>().PickUpWeapon(newGun);
                }
            }
        }
    }
}