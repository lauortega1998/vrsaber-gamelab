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

            if (timer <= 0f )
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
        if (playerHealth == null || raycastOrigin == null || raycastTarget == null) return;

        Vector3 origin = raycastOrigin.position;
        Vector3 direction = (raycastTarget.position - origin).normalized;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        Debug.DrawRay(origin, direction * raycastDistance, Color.red, 1f); // Draw the ray for visualization

        // Activate the attack effect
        if (enemyAttackEffect != null)
        {
            enemyAttackEffect.SetActive(false); // Reset in case it was already active
            enemyAttackEffect.SetActive(true);  // Re-activate
            StartCoroutine(DisableAttackEffectAfterDelay(0.5f)); // Turn it off after 0.5 seconds (you can tweak)
        }

        if (Physics.Raycast(ray, out hit, raycastDistance, raycastLayers))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            if (hit.collider.CompareTag("Shield"))
            {
                ShieldHealth shield = hit.collider.GetComponent<ShieldHealth>();

                if (shield != null)
                {
                    shield.TakeDamage(damageAmount);
                    Debug.Log("Shield took damage!");
                }
                else
                {
                    Debug.LogWarning("Hit a shield but no ShieldHealth component found!");
                }

                return; // Stop here if a shield blocked it
            }
        }

        // No shield blocking, apply damage to player
        playerHealth.TakeDamage(damageAmount);
        Debug.Log("Player took damage!");
    }

    private System.Collections.IEnumerator DisableAttackEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyAttackEffect != null)
        {
            enemyAttackEffect.SetActive(false);
        }
    }
}