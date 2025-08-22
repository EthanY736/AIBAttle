using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs to spawn
    public Material[] Materials; // Array of random materials/textures
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnRadius = 10f; // Random range for X and Z spawn positions
    public float SpawnHeight = 0;

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
                SpawnHeight,
                transform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            // Spawn the enemy
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            // Apply a random material if available
            if (Materials.Length > 0)
            {
                Material randomMat = Materials[Random.Range(0, Materials.Length)];
                Renderer rend = spawnedEnemy.GetComponent<Renderer>();

                // If the prefab has a renderer directly
                if (rend != null)
                {
                    rend.material = randomMat;
                }
                else
                {
                    // If the prefab has child meshes
                    Renderer[] childRenderers = spawnedEnemy.GetComponentsInChildren<Renderer>();
                    foreach (Renderer childRend in childRenderers)
                    {
                        childRend.material = randomMat;
                    }
                }
            }
        }
    }
}
