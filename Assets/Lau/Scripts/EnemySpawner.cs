using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public Vector3 GetRandomSpawnPosition()
    {
        if (boxCollider == null)
        {
            Debug.LogError("EnemySpawner requires a BoxCollider!");
            return Vector3.zero;
        }

        // Get the bounds of the BoxCollider
        Vector3 center = boxCollider.bounds.center;
        Vector3 size = boxCollider.bounds.size;

        // Random position within the bounds
        float randomX = center.x;
        float randomY = center.y; // Keep Y consistent
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }
}
