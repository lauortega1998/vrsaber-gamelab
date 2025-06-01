using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryTutorialScript : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject normalEnemyPrefab;
    public GameObject flyingEnemyPrefab;
    public GameObject strongEnemyPrefab;
    public GameObject rageEnemyPrefab; // ✅ NEW: separate rage wave prefab

    [Header("Spawn Points")]
    public Transform[] normalSpawnPoints;
    public Transform flyingSpawnPoint;
    public Transform strongSpawnPoint;

    [Header("Rage Settings")]
    public GameObject rageUIElement;
    // public RageController rageController;

    [Header("Next Factory")]
    public GameObject nextEnemyFactoryPrefab;
    public Transform nextFactorySpawnPoint;
    public float delayBeforeNextFactory = 3f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<GameObject> normalEnemies = new List<GameObject>();
    private List<GameObject> rageWaveEnemies = new List<GameObject>();
    private GameObject flyingEnemy;

    public RageSystem rageSystem; // drag your RageSystem object into this in the Inspector


    void Start()
    {
        StartCoroutine(SpawnTutorialEnemies());
    }

    IEnumerator SpawnTutorialEnemies()
    {
        // Step 1: Spawn 2 normal enemies
        for (int i = 0; i < 2 && i < normalSpawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, normalSpawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            normalEnemies.Add(enemy);
        }

        // Step 2: Wait for normal enemies to be defeated
        yield return new WaitUntil(() => NormalEnemiesDefeated());

        // Step 3: Spawn flying enemy
        flyingEnemy = Instantiate(flyingEnemyPrefab, flyingSpawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(flyingEnemy);

        // Step 4: Wait for flying enemy to be defeated
        yield return new WaitUntil(() => flyingEnemy == null);

        // Step 5: Spawn strong (giant) enemy
        GameObject strong = Instantiate(strongEnemyPrefab, strongSpawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(strong);

        // Step 6: Wait for giant to be defeated
        yield return new WaitUntil(() => strong == null);

        if (rageSystem != null) 
        {
            rageSystem.currentRage = rageSystem.rageMax;   // fill to 100%
            rageSystem.StartDepletingRage();               // trigger rage effects manually
        }

        if (rageUIElement != null)
        {
            rageUIElement.SetActive(true);
        }

        // ✅ Step 8: Spawn 4 Rage enemies using a separate prefab
        for (int i = 0; i < 4 && i < normalSpawnPoints.Length; i++)
        {
            GameObject rageEnemy = Instantiate(rageEnemyPrefab, normalSpawnPoints[i].position, Quaternion.identity);
            spawnedEnemies.Add(rageEnemy);
            rageWaveEnemies.Add(rageEnemy);
        }

        // Step 9: Wait for rage wave enemies to be defeated
        yield return new WaitUntil(() => RageEnemiesDefeated());

        // Step 10: Wait before spawning next factory
        yield return new WaitForSeconds(delayBeforeNextFactory);

        // Step 11: Spawn next enemy factory
        if (nextEnemyFactoryPrefab != null && nextFactorySpawnPoint != null)
        {
            nextEnemyFactoryPrefab.SetActive(true);
            rageUIElement.SetActive(false);

        }

        // Step 12: Destroy this factory
        //Destroy(gameObject);
    }

    private bool NormalEnemiesDefeated()
    {
        normalEnemies.RemoveAll(e => e == null);
        return normalEnemies.Count == 0;
    }

    private bool RageEnemiesDefeated()
    {
        rageWaveEnemies.RemoveAll(e => e == null);
        return rageWaveEnemies.Count == 0;

    }

    private bool AllEnemiesDefeated()
    {
        spawnedEnemies.RemoveAll(e => e == null);
        return spawnedEnemies.Count == 0;
    }
}

