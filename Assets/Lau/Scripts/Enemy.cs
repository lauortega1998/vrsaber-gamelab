using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy

    private Transform player;
    private bool canMove = true;

    void Start()
    {
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
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            canMove = false;
        }
    }
}
