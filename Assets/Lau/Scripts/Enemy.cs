using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy
    public int damageAmount = 10; // How much damage the enemy deals
    private PlayerHealth playerHealth;
    private Transform player;
    private bool canMove = true;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && canMove)
        {
            // Move towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper"))
        {
            playerHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            canMove = false;
        }
    }
}
