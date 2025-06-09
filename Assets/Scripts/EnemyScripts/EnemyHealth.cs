using System;
using UnityEngine;
using System.Collections;


public class EnemyHealth : MonoBehaviour
{
    [Header("Hit Effect")]
    public GameObject hitEffectPrefab;

    [Header("Broken Version")]
    public GameObject brokenSkeletonPrefab;
    public Transform skeletonSpawnPoint; // ← assign in Inspector


    [Header("Death Effects")]
    public float knockbackForce = 5f;
    public float enemyDestroyDelay = 2f;
    public event Action OnDeath;

    private bool hasDied = false;

    public TimeManager timeManager;
    public enum EnemyType { Normal, Flying, Heavy }

    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Normal;




    public void Die(Transform weaponHitPoint)
    {
        if (hasDied) return;
        hasDied = true;

        GameObject enemyRoot = this.gameObject;
        Debug.Log($"[EnemyHealth] Dying: {enemyRoot.name}");

        // Disable AI, animation, and collider
        Animator animator = GetComponent<Animator>();
        FindAnyObjectByType<AudioManager>().Stop("walking loop");

        if (animator) animator.enabled = false;

        // Instantly hide the enemy visuals
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
            col.enabled = false;

        if (animator) animator.enabled = false;
        EnemyManager.Instance?.UnregisterAttacker();
        int points = enemyType switch
        {
            EnemyType.Normal => 1,
            EnemyType.Flying => 3,
            EnemyType.Heavy => 5,
            _ => 1
        };
        KillCounter.Instance?.AddKill(points);

        TutorialEnemy tutorialEnemy = GetComponent<TutorialEnemy>();
        tutorialEnemy?.ActivateEnemyFactory();

        FindAnyObjectByType<PlayerStats>()?.GainMana(20);
        FindAnyObjectByType<RageSystem>()?.OnEnemyKilled();
        



        // 🔥 Spawn hit effect
        if (hitEffectPrefab != null && weaponHitPoint != null)
        {
            Vector3 hitPoint = GetComponent<Collider>().ClosestPoint(weaponHitPoint.position);
            GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
            Destroy(hitEffect, 2f);
            Debug.Log("[EnemyHealth] Hit effect spawned.");
        }

        // 🦴 Spawn broken prefab
        if (brokenSkeletonPrefab != null)
        {
            Vector3 spawnPosition = skeletonSpawnPoint ? skeletonSpawnPoint.position : transform.position;
            Quaternion spawnRotation = skeletonSpawnPoint ? skeletonSpawnPoint.rotation : transform.rotation;

            GameObject brokenInstance = Instantiate(brokenSkeletonPrefab, spawnPosition, spawnRotation);
            Debug.Log("[EnemyHealth] Broken skeleton spawned.");

            Rigidbody[] ragdollRigidbodies = brokenInstance.GetComponentsInChildren<Rigidbody>();
            Vector3 pushDirection = (transform.position - weaponHitPoint.position).normalized;
            pushDirection.y = 0;

            foreach (Rigidbody rb in ragdollRigidbodies)
            {
                if (rb)
                {
                    rb.isKinematic = false;
                    rb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
                }
            }

            Destroy(brokenInstance, enemyDestroyDelay);
        }

        FindAnyObjectByType<TimeManager>()?.SlowDown();

        // Delay destroy so you can see the effects before the root is nuked
        StartCoroutine(DestroyAfterDelay(enemyRoot, 0.1f));
    }
    private IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
        Debug.Log($"[EnemyHealth] Destroyed {target.name} after delay.");
    }
}
