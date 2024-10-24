using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // Assign the asteroid prefab in the Inspector
    public Collider pipeCollider; // Assign the pipe's collider in the Inspector
    public int asteroidCount = 100;

    void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 spawnPosition;
            int attempts = 0;
            do
            {
                // Generate a random position within the bounds of the collider
                spawnPosition = GetRandomPointInCollider(pipeCollider);
                attempts++;
            } while (!pipeCollider.bounds.Contains(spawnPosition) && attempts < 100);

            // Instantiate the asteroid
            Instantiate(asteroidPrefab, spawnPosition, Random.rotation);
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 boundsMin = collider.bounds.min;
        Vector3 boundsMax = collider.bounds.max;

        return new Vector3(
            Random.Range(boundsMin.x, boundsMax.x),
            Random.Range(boundsMin.y, boundsMax.y),
            Random.Range(boundsMin.z, boundsMax.z)
        );
    }
}