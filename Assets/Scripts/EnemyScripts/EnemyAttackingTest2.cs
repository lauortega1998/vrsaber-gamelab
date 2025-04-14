using UnityEngine;

public class EnemyAttackingTest2 : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    public int damageAmount = 10;
    private PlayerHealth playerHealth;
    private int lastPrintedTime = -1; // For printing only when seconds change

    public float raycastDistance = 10f; // Maximum distance to check
    public LayerMask raycastLayers;     // Layers you want the ray to check (you can set it to "Default" and "Shield")
    public Transform raycastOrigin;   // Where the ray starts
    public Transform raycastTarget;   // Where the ray points to


    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper") && !timerStarted)
        {
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
                PerformAction();
                StartTimer(); // Optional: keep looping
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
        if (playerHealth == null) return;

        Vector3 origin = raycastOrigin != null ? raycastOrigin.position : transform.position;
        Vector3 targetPosition = playerHealth.transform.position;

        // Calculate direction ONCE based on current player position
        Vector3 direction = (targetPosition - origin).normalized;

        float distance = 100f; // Straight ray distance, you can set it to what you want

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        Debug.DrawRay(origin, direction * distance, Color.red, 1f); // Visualize the straight ray

        if (Physics.Raycast(ray, out hit, distance, raycastLayers))
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
                return; // Shield blocked the attack
            }
        }

        // No shield blocking, apply damage to player
        playerHealth.TakeDamage(damageAmount);
        Debug.Log("Player took damage!");
    }
}
