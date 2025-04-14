using UnityEngine;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public float hoverHeight = 2.5f;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float attackInterval = 2f;

    private Transform player;
    private bool canMove = true;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;
    private Vector3 targetPosition;
    private Rigidbody rb;
   
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("LookTarget").transform;
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (canMove)
        {
            targetPosition = new Vector3(player.position.x, hoverHeight, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            Vector3 lookDir = player.position + transform.position;
            lookDir.z = 20;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 2f);
        }
        else if (isAttacking && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackInterval;
            AttackPlayer();
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper"))
        {
            Debug.Log($"{gameObject.name} hit a MovementStopper. Starting to attack.");
            canMove = false;
            isAttacking = true;
        }
    }

    private void AttackPlayer()
    {
        GameObject magnet = GameObject.FindGameObjectWithTag("ProjectileMagnet");

        if (magnet == null || projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector3 direction = (magnet.transform.position - transform.position).normalized;

        // Add some random inaccuracy
        float spreadAngle = 5f; // Max spread angle in degrees
        direction = Quaternion.Euler(
            Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle)
        ) * direction;

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            projectileRb.linearVelocity = direction * projectileSpeed;
        }

        Debug.Log($"{gameObject.name} attacks the ProjectileMagnet with a randomized projectile!");
    }

    // Optional method if you want to stop the enemy from other scripts
    public void StopMovement()
    {
        canMove = false;
        isAttacking = true;
    }
}
