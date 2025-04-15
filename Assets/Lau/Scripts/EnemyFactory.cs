using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject flyingSpectrePrefab;

    [Header("Spawners")]
    public List<EnemySpawner> spawners = new List<EnemySpawner>(); // Ground spawners
    public List<EnemySpawner> spectreSpawners = new List<EnemySpawner>(); // Airborne spawners for spectres

    [Header("Spawn Settings")]
    public float groundSpawnInterval = 2f;  // New: Ground enemies spawn interval
    public float flyingSpawnInterval = 3f;  // New: Flying enemies spawn interval
    public float enemySpeed = 5f;

    [Header("Difficulty Settings")]
    public float difficultyIncreaseRate = 10f;
    public float spawnIntervalDecrease = 0.1f;
    public float enemySpeedIncrease = 0.5f;
    public float minSpawnInterval = 0.5f;

    [Header("Spawn Limits")]
    public int maxGroundEnemies = 6;
    public int maxFlyingEnemies = 6;

    private int currentGroundEnemies = 0;
    private int currentFlyingEnemies = 0;
    private EnemyHealth healthscript;

    private void Start()
    {
        if (spawners.Count == 0)
        {
            Debug.LogError("EnemyFactory requires at least one ground EnemySpawner!");
        }

        if (spectreSpawners.Count == 0)
        {
            Debug.LogWarning("No spectre-specific spawners assigned. Flying spectres will use ground spawners instead.");
        }

        if (enemyPrefab == null || flyingSpectrePrefab == null)
        {
            Debug.LogError("EnemyFactory is missing prefab references!");
            return;
        }

        StartCoroutine(SpawnGroundEnemies());
        StartCoroutine(SpawnFlyingEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    private IEnumerator SpawnGroundEnemies()
    {
        while (true)
        {
            if (currentGroundEnemies < maxGroundEnemies)
            {
                SpawnGroundEnemy();
                
            }
            yield return new WaitForSeconds(groundSpawnInterval);
        }
    }

    private IEnumerator SpawnFlyingEnemies()
    {
        while (true)
        {
            if (currentFlyingEnemies < maxFlyingEnemies)
            {
                SpawnFlyingEnemy();
            }
            yield return new WaitForSeconds(flyingSpawnInterval);
        }
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseRate);

            groundSpawnInterval = Mathf.Max(groundSpawnInterval - spawnIntervalDecrease, minSpawnInterval);
            flyingSpawnInterval = Mathf.Max(flyingSpawnInterval - spawnIntervalDecrease, minSpawnInterval);
            enemySpeed += enemySpeedIncrease;

            Debug.Log($"Difficulty increased! Ground Interval: {groundSpawnInterval}, Flying Interval: {flyingSpawnInterval}, Enemy Speed: {enemySpeed}");
        }
    }

    private void SpawnGroundEnemy()
    {
        if (spawners.Count == 0) return;

        // Select a random spawner and get a spawn position
        EnemySpawner selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();

        // Instantiate the enemy
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Check if the enemy has the EnemyHealth component
        if (newEnemy.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            // Optionally set speed or other properties on the enemyHealth (if needed)
            // enemyHealth.speed = enemySpeed;  // Uncomment if there's a speed property

            // Increment the count of ground enemies
            currentGroundEnemies++;

            // Subscribe to the OnDeath event
            enemyHealth.OnDeath += () =>
            {
                currentGroundEnemies--; // Decrease the count when this enemy dies
            };
        }
    }

    private void SpawnFlyingEnemy()
    {
        EnemySpawner selectedSpawner;

        if (spectreSpawners.Count > 0)
        {
            selectedSpawner = spectreSpawners[Random.Range(0, spectreSpawners.Count)];
        }
        else if (spawners.Count > 0)
        {
            Debug.LogWarning("Spectre spawner list is empty. Falling back to ground spawners.");
            selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        }
        else
        {
            return; // No spawners available
        }

        // Get the spawn position
        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();

        // Instantiate the flying enemy (spectre)
        GameObject newSpectre = Instantiate(flyingSpectrePrefab, spawnPosition, Quaternion.identity);

        // Check if the flying spectre has the EnemyHealth component
        if (newSpectre.TryGetComponent<EnemyHealth>(out var spectreHealth))
        {
            // Optionally set speed or other properties on the spectreHealth (if needed)
            // spectreHealth.speed = enemySpeed;  // Uncomment if there's a speed property

            // Increment the count of flying enemies
            currentFlyingEnemies++;

            // Subscribe to the OnDeath event
            spectreHealth.OnDeath += () =>
            {
                currentFlyingEnemies--; // Decrease the count when this flying enemy dies
            };
        }
    }
}
