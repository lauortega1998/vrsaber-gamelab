using UnityEngine;

public class EnemyAttacking : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    public int damageAmount = 10; 
    private PlayerHealth playerHealth;


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
            Debug.Log("Collision detected! Timer started.");
        }
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            Debug.Log("Timer: " + timer.ToString("F2") + " seconds remaining");

            if (timer <= 0f)
            {
                timerStarted = false;
                PerformAction();
            }
        }
    }

    private void PerformAction() //Appling the methods when the timer end 
    {
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
    }
}
