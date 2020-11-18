using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkStartPosition))]
public class SpawnOffset : MonoBehaviour
{
    public float radius = 10f;
    private NetworkStartPosition startPosition;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public Vector3 GetOffset()
    {
        Vector3 position = transform.position;
        Vector3 randomRange = Random.insideUnitSphere;
        randomRange.y = 0f;
        position += randomRange * radius;
        return position;
    }

    private void Awake()
    {
        startPosition = GetComponent<NetworkStartPosition>();
    }

}
