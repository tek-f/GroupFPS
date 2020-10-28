using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerManager : MonoBehaviour
{
    GameObject gunPrefab;//prefab for the gun that is assigned to this trigger
    int teamID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }
}
