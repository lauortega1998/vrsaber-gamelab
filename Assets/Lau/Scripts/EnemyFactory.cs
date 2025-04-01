using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<EnemySpawner> spawners = new List<EnemySpawner>(); // List of spawners

    public float spawnInterval = 2f;
    public float enemySpeed = 5f;

    [Header("Difficulty Settings")]
    public float difficultyIncreaseRate = 10f; // How often (in seconds) difficulty increases
    public float spawnIntervalDecrease = 0.1f; // How much the spawn interval decreases
    public float enemySpeedIncrease = 0.5f; // How much enemy speed increases
    public float minSpawnInterval = 0.5f; // The lowest possible spawn interval

    private void Start()
    {
        if (spawners == null || spawners.Count == 0)
        {
            Debug.LogError("EnemyFactory requires at least one assigned EnemySpawner!");
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

            // Reduce spawn interval but keep it above the minimum limit
            spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecrease, minSpawnInterval);

            // Increase enemy speed
            enemySpeed += enemySpeedIncrease;

            Debug.Log($"Difficulty increased! New Spawn Interval: {spawnInterval}, Enemy Speed: {enemySpeed}");
        }
    }

    private void SpawnEnemy()
    {
        if (spawners == null || spawners.Count == 0) return;

        // Choose a random spawner from the list
        EnemySpawner selectedSpawner = spawners[Random.Range(0, spawners.Count)];

        // Get random spawn position from the selected spawner
        Vector3 spawnPosition = selectedSpawner.GetRandomSpawnPosition();

        // Instantiate the enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().speed = enemySpeed;
    }
}