using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;   // The pickup to spawn
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // Area to spawn in

    public void SpawnNewPickup()
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        Instantiate(pickupPrefab, randomPos, Quaternion.identity);
    }
}
