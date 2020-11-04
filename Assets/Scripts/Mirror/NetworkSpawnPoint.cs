using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunBall.Mirror
{
    public class NetworkSpawnPoint : MonoBehaviour
    {
        private void Awake()
        {
            NetworkSpawnSystem.AddSpawnPoint(transform);
        }
        private void OnDestroy()
        {
            NetworkSpawnSystem.RemoveSpawnPoint(transform);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        }
    }
}