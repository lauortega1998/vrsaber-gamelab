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
        if (rb != null)
            Debug.Log($"[WeaponVelocityDamage] Current Velocity: {rb.linearVelocity.magnitude:F2}");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float impactVelocity = rb.linearVelocity.magnitude;
            Debug.Log($"[WeaponVelocityDamage] Hit {collision.gameObject.name} | Velocity: {impactVelocity:F2}");

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
                        Vector3 pushDir = (collision.transform.position + transform.position).normalized;
                        pushDir.y = 0;
                        enemyRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}

