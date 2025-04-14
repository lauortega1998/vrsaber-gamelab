using UnityEngine;

public class WeaponVelocityDamage : MonoBehaviour
{
    public float damageVelocityThreshold = 1.5f;
    public float pushForce = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       // if (rb != null)
//            Debug.Log($"[WeaponVelocityDamage] Current Velocity: {rb.linearVelocity.magnitude:F2}");
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = rb.linearVelocity.magnitude;

        if (collision.gameObject.CompareTag("Enemy"))
        {
           Debug.Log("CollidedWithEnemy");
          
            
            var enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                if (impactVelocity >= damageVelocityThreshold)
                {
                    Debug.Log("[WeaponVelocityDamage] Killing enemy.");
                    enemy.Die(transform);

                }
                else
                {
                    Debug.Log("[WeaponVelocityDamage] Pushing enemy.");
                    Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        // Calculate direction (FIXED: Use subtraction, not addition)
                        Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                        pushDir.y = 0; // Remove vertical influence from force
    
                        // Freeze Y position to prevent any movement on that axis
                        enemyRb.constraints = RigidbodyConstraints.FreezePositionY;
    
                        // Apply force (horizontal only)
                        enemyRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
                    }
                }
            }
        }
        else if (collision.gameObject.CompareTag("DestructibleUI"))
        {
            Debug.Log($"[WeaponVelocityDamage] Hit Column {collision.gameObject.name} | Velocity: {impactVelocity:F2}");

            var column = collision.gameObject.GetComponentInParent<MenuHitActivate>();
            if (column != null && impactVelocity >= damageVelocityThreshold)
            {
                Debug.Log("[WeaponVelocityDamage] Breaking column to start the game.");
                column.Die(transform);
            }
        }
    }
}

