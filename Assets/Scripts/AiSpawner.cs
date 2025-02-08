using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnRadius = 10f; // Random range for X and Z spawn positions

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Pick a random enemy prefab
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Generate a random position around the spawner
            Vector3 randomPosition = new Vector3(
                transform.position.x + Random.Range(-spawnRadius, spawnRadius),
                transform.position.y,
                transform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            // Spawn the enemy and apply random stats
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
}
