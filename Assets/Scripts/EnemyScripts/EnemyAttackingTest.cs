using UnityEngine;
using System.Collections;


public class EnemyAttackingTest : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    public int damageAmount = 10;
    private PlayerHealth playerHealth;
    private int lastPrintedTime = -1; // For printing only when seconds change
    public bool isEnemyCollided = false;

    public float raycastDistance = 10f; // Maximum distance to check
    public LayerMask raycastLayers;     // Layers you want the ray to check (e.g., Default, Shield)
    public Transform raycastOrigin;     // The object inside the prefab (already assigned!)
    private Transform raycastTarget;    // Will find LookTarget automatically
    public GameObject enemyAttackEffect;
    public GameObject enemyAttackCollider
        ;


    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

        // Find the LookTarget automatically
        GameObject targetObject = GameObject.FindGameObjectWithTag("LookTarget");

        if (targetObject != null)
        {
            raycastTarget = targetObject.transform;
        }
        else
        {
            Debug.LogWarning("No object with tag 'LookTarget' found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementStopper") && !timerStarted)
        {
            isEnemyCollided = true;
            EnemyManager.Instance.RegisterAttacker();
            StartTimer();
        }
    }

    private void OnEnable()
    {
        ResetTimerState(); // Ensure fresh start
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;

            int currentSeconds = Mathf.CeilToInt(timer);

            if (currentSeconds != lastPrintedTime && currentSeconds >= 0)
            {
                Debug.Log($"{gameObject.name} Timer: {currentSeconds} seconds remaining");
                lastPrintedTime = currentSeconds;
            }

            if (timer <= 0f)
            {
                if (isEnemyCollided == true)
                {
                    PerformAction();
                    StartTimer();
                }
            }
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
        timer = countdownTime;
        lastPrintedTime = -1;
        Debug.Log($"{gameObject.name} Timer started!");
    }

    public void StopTimer()
    {
        timerStarted = false;
        timer = 0;
        Debug.Log($"{gameObject.name} Timer stopped/reset.");
    }

    public void ResetTimerState()
    {
        timerStarted = false;
        timer = 0;
        lastPrintedTime = -1;
    }

    private void PerformAction()
    {
        // Activate the attack effect (visuals)
        if (enemyAttackEffect != null)
        {
            enemyAttackEffect.SetActive(false);
            enemyAttackEffect.SetActive(true);
            StartCoroutine(DisableAttackEffectAfterDelay(enemyAttackEffect, 0.5f));
        }

        // Activate the attack collider (logic)
        if (enemyAttackCollider != null)
        {
            enemyAttackCollider.SetActive(false);
            enemyAttackCollider.SetActive(true);
            StartCoroutine(DisableAttackEffectAfterDelay(enemyAttackCollider, 0.5f));
        }

        Debug.Log("Attack performed!");
    }

    

    private System.Collections.IEnumerator DisableAttackEffectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            obj.SetActive(false);
        }
    }


}