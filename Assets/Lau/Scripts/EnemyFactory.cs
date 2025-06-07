using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject flyingSpectrePrefab;
    public GameObject heavyenemyPrefab;

    [Header("Spawners")]
    public List<EnemySpawner> spawners = new List<EnemySpawner>();
    public List<EnemySpawner> spectreSpawners = new List<EnemySpawner>();

    [Header("Spawn Settings")]
    public float groundSpawnInterval = 2f;
    public float heavySpawnInterval = 5f;
    public float flyingSpawnInterval = 3f;
    public float enemySpeed = 5f;

    [Header("Difficulty Settings")]
    public float difficultyIncreaseRate = 10f;
    public float spawnIntervalDecrease = 0.1f;
    public float enemySpeedIncrease = 0.5f;
    public float minSpawnInterval = 0.5f;

    [Header("Spawn Limits")]
    public int maxGroundEnemies = 6;
    public int maxFlyingEnemies = 6;
    public int maxHeavyEnemies = 3; // NEW

    private int currentGroundEnemies = 0;
    private int currentFlyingEnemies = 0;
    private int currentHeavyEnemies = 0; // NEW

    private void Start()
    {
        if (spawners.Count == 0)
            Debug.LogError("EnemyFactory requires at least one ground EnemySpawner!");

        if (spectreSpawners.Count == 0)
            Debug.LogWarning("No spectre-specific spawners assigned. Flying spectres will use ground spawners instead.");

        if (enemyPrefab == null || flyingSpectrePrefab == null || heavyenemyPrefab == null)
        {
            Debug.LogError("EnemyFactory is missing prefab references!");
            return;
        }

        StartCoroutine(SpawnGroundEnemies());
        StartCoroutine(SpawnFlyingEnemies());
        StartCoroutine(SpawnHeavyGroundEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    private IEnumerator SpawnGroundEnemies()
    {
        while (true)
        {
            if (currentGroundEnemies < maxGroundEnemies)
                SpawnGroundEnemy();

            yield return new WaitForSeconds(groundSpawnInterval);
        }
    }

    private IEnumerator SpawnHeavyGroundEnemies()
    {
        while (true)
        {
            if (currentHeavyEnemies < maxHeavyEnemies)
                SpawnHeavyGroundEnemy();

            yield return new WaitForSeconds(heavySpawnInterval);
        }
    }

    private IEnumerator SpawnFlyingEnemies()
    {
        while (true)
        {
            if (currentFlyingEnemies < maxFlyingEnemies)
                SpawnFlyingEnemy();

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
            heavySpawnInterval = Mathf.Max(heavySpawnInterval - spawnIntervalDecrease, minSpawnInterval);
            enemySpeed += enemySpeedIncrease;

            Debug.Log($"Difficulty increased! Ground: {groundSpawnInterval:F2}, Flying: {flyingSpawnInterval:F2}, Heavy: {heavySpawnInterval:F2}, Speed: {enemySpeed:F2}");
        }
    }

    private void SpawnGroundEnemy()
    {
        if (spawners.Count == 0) return;

        var selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.tag = "Enemy";

        if (newEnemy.TryGetComponent<EnemyDamage>(out var damage))
            damage.damageAmount = 10;

        if (newEnemy.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            currentGroundEnemies++;
            enemyHealth.OnDeath += () => currentGroundEnemies--;
        }
    }

    private void SpawnHeavyGroundEnemy()
    {
        if (spawners.Count == 0) return;

        var selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();

        GameObject newEnemy = Instantiate(heavyenemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.tag = "Enemy";

        if (newEnemy.TryGetComponent<EnemyDamage>(out var damage))
            damage.damageAmount = 25;

        if (newEnemy.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            currentGroundEnemies++;
            currentHeavyEnemies++; // Track separately

            enemyHealth.OnDeath += () =>
            {
                currentGroundEnemies--;
                currentHeavyEnemies--;
            };
        }
    }

    private void SpawnFlyingEnemy()
    {
        EnemySpawner selectedSpawner;

        if (spectreSpawners.Count > 0)
            selectedSpawner = spectreSpawners[Random.Range(0, spectreSpawners.Count)];
        else if (spawners.Count > 0)
        {
            Debug.LogWarning("Spectre spawner list is empty. Falling back to ground spawners.");
            selectedSpawner = spawners[Random.Range(0, spawners.Count)];
        }
        else return;

        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();
        GameObject newSpectre = Instantiate(flyingSpectrePrefab, spawnPosition, Quaternion.identity);

        if (newSpectre.TryGetComponent<EnemyHealth>(out var spectreHealth))
        {
            currentFlyingEnemies++;
            spectreHealth.OnDeath += () => currentFlyingEnemies--;
        }
    }
}