using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    public int damageAmount = 10; 
    private PlayerHealth playerHealth;
    private int lastPrintedTime = -1; // For printing only when seconds change


    public void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper") && !timerStarted)
        {
            timerStarted = true;
            timer = countdownTime;
            lastPrintedTime = -1;
            Debug.Log($"{gameObject.name} collided! Timer started.");
        }
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
                timerStarted = false;
                PerformAction();
                StartTimer();
            }
        }
    }
    private void StartTimer()
    {
        timerStarted = true;
        timer = countdownTime;
        lastPrintedTime = -1;
        Debug.Log($"{gameObject.name} Timer restarted!");
    }
    private void PerformAction() //Appling the methods when the timer end 
    {
            playerHealth.TakeDamage(damageAmount);
          //  Destroy(gameObject);
    }
}
