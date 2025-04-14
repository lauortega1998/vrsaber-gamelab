using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy
    private Transform player;
    private bool canMove = true;
    [SerializeField] private Transform lookTarget; // <- this also works and keeps it private in code

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

      
    }


    void Update()
    {
        if (canMove && !EnemyManager.Instance.isAnyEnemyAttacking)
        {
            // Move towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        
    }
    public void StopMovement()
    {
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper"))
        {
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            canMove = false;

            // Face the player immediately
        
        }
    }
}
