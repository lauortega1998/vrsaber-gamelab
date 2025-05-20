using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnEvent
{
    public float spawnTime; // When this event happens
    public GameObject enemyPrefab; // Which enemy to spawn
    public int amount = 1; // How many enemies to spawn
    public Transform[] spawnPoints; // Where to spawn them
}

[System.Serializable]
public class EnemyWave
{
    public List<SpawnEvent> spawnEvents = new List<SpawnEvent>();
}



public class EnemyFactoryFirstLevel : MonoBehaviour
{
    public List<EnemyWave> waves = new List<EnemyWave>();
    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private bool waveRunning = false;

    public float delayBetweenWaves = 5f;

    public void StartWaves()
    {
        if (!waveRunning)
        {
            StartCoroutine(RunWave(waves[currentWaveIndex]));
        }
    }

    private IEnumerator RunWave(EnemyWave wave)
    {
        waveRunning = true;
        waveTimer = 0f;

        List<SpawnEvent> events = new List<SpawnEvent>(wave.spawnEvents);
        events.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));

        while (events.Count > 0)
        {
            SpawnEvent nextEvent = events[0];

            if (waveTimer >= nextEvent.spawnTime)
            {
                for (int i = 0; i < nextEvent.amount; i++)
                {
                    Transform spawnPoint = nextEvent.spawnPoints[Random.Range(0, nextEvent.spawnPoints.Length)];
                    Instantiate(nextEvent.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                }

                events.RemoveAt(0);
            }

            waveTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(delayBetweenWaves);

        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            StartCoroutine(RunWave(waves[currentWaveIndex]));
        }
        else
        {
            Debug.Log("All waves completed.");
        }

        waveRunning = false;
    }
    void Start()
    {
        StartWaves();

    }
}