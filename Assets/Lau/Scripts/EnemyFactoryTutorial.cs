using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryTutorialScript : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject flyingEnemyPrefab;
    public GameObject strongEnemyPrefab;
    public GameObject rageEnemyPrefab;

    [Header("Spawn Points")]
    public Transform[] normalSpawnPoints;
    public Transform flyingSpawnPoint;
    public Transform strongSpawnPoint;

    [Header("Rage Settings")]
    public GameObject rageUIElement;
    public RageSystem rageSystem;

    [Header("Next Factory")]
    public GameObject nextEnemyFactoryPrefab;
    public Transform nextFactorySpawnPoint;
    public float delayBeforeNextFactory = 3f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> rageWaveEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(EnemyWaveFlow());
    }

    private IEnumerator EnemyWaveFlow()
    {
        // ⏱️ 0s: Spawn 2 Normal Enemies
        SpawnNormalEnemies();

        // ⏱️ Wait 15s
        yield return new WaitForSeconds(15f);
        SpawnFlyingEnemy();

        // ⏱️ Wait 15s more (30s total)
        yield return new WaitForSeconds(15f);
        SpawnGiantEnemy();

        // ⏱️ Wait 10s more (40s total)
        yield return new WaitForSeconds(10f);
        TriggerRagePhase();

        // ⏱️ Wait until rage wave is defeated (optional)
        StartCoroutine(CheckRageWaveDefeated());
    }

    private void SpawnNormalEnemies()
    {
        for (int i = 0; i < 2 && i < normalSpawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, normalSpawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    private void SpawnFlyingEnemy()
    {
        GameObject flying = Instantiate(flyingEnemyPrefab, flyingSpawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(flying);
    }

    private void SpawnGiantEnemy()
    {
        GameObject strong = Instantiate(strongEnemyPrefab, strongSpawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(strong);
    }

    private void TriggerRagePhase()
    {
        if (rageSystem != null)
        {
            rageSystem.currentRage = rageSystem.rageMax;
            rageSystem.StartDepletingRage();
        }

        if (rageUIElement != null)
            rageUIElement.SetActive(true);

        SpawnRageEnemies();
    }

    private void SpawnRageEnemies()
    {
        for (int i = 0; i < 4 && i < normalSpawnPoints.Length; i++)
        {
            GameObject rageEnemy = Instantiate(rageEnemyPrefab, normalSpawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(rageEnemy);
            rageWaveEnemies.Add(rageEnemy);
        }
    }

    private IEnumerator CheckRageWaveDefeated()
    {
        yield return new WaitUntil(() => RageEnemiesDefeated());
        yield return new WaitForSeconds(delayBeforeNextFactory);

        if (nextEnemyFactoryPrefab != null && nextFactorySpawnPoint != null)
        {
            nextEnemyFactoryPrefab.SetActive(true);
            rageUIElement.SetActive(false);
        }
    }

    private bool RageEnemiesDefeated()
    {
        rageWaveEnemies.RemoveAll(e => e == null);
        return rageWaveEnemies.Count == 0;
    }
}
