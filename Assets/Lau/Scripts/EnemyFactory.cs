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
    public float spawnInterval = 2f;
    public float enemySpeed = 5f;

    [Header("Difficulty Settings")]
    public float difficultyIncreaseRate = 10f;
    public float spawnIntervalDecrease = 0.1f;
    public float enemySpeedIncrease = 0.5f;
    public float minSpawnInterval = 0.5f;

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

        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseRate);

            spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecrease, minSpawnInterval);
            enemySpeed += enemySpeedIncrease;

            Debug.Log($"Difficulty increased! New Spawn Interval: {spawnInterval}, Enemy Speed: {enemySpeed}");
        }
    }

    private void SpawnEnemy()
    {
        bool spawnFlying = Random.value < 0.5f;

        EnemySpawner selectedSpawner = null;
        Vector3 spawnPosition;

        if (spawnFlying)
        {
            if (spectreSpawners.Count > 0)
            {
                selectedSpawner = spectreSpawners[Random.Range(0, spectreSpawners.Count)];
            }
            else
            {
                Debug.LogWarning("Spectre spawner list is empty. Falling back to ground spawners.");
                selectedSpawner = spawners[Random.Range(0, spawners.Count)];
            }

            spawnPosition = selectedSpawner.GetRandomSpawnPosition();
            GameObject newSpectre = Instantiate(flyingSpectrePrefab, spawnPosition, Quaternion.identity);

            if (newSpectre.TryGetComponent<FlyingEnemyBehaviour>(out var spectre))
            {
                spectre.speed = enemySpeed;
            }
        }
        else
        {
            if (spawners.Count == 0) return;
            selectedSpawner = spawners[Random.Range(0, spawners.Count)];
            spawnPosition = selectedSpawner.GetRandomSpawnPosition();

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            if (newEnemy.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.speed = enemySpeed;
            }
        }
    }
}