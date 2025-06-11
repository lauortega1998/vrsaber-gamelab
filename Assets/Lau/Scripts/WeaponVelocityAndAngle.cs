using UnityEngine;

public class WeaponVelocityAndDirectionDamage : MonoBehaviour
{
    public float damageVelocityThreshold = 1.5f;
    public float pushForce = 5f;
    private Rigidbody rb;
    private bool isOnGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = rb.linearVelocity.magnitude;

        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = true;
            return;
        }

        if (isOnGround) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy");

            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            StrikeZone strikeZone = collision.gameObject.GetComponentInChildren<StrikeZone>();

            if (enemy != null && strikeZone != null)
            {
                if (impactVelocity >= damageVelocityThreshold && strikeZone.IsCorrectStrike(rb.linearVelocity))
                {
                    Debug.Log("[Weapon] Correct strike � Enemy killed.");
                    enemy.Die(transform);
                }
                else
                {
                    Debug.Log("[Weapon] Incorrect strike direction or too slow.");
                }

                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                    pushDir.y = 0;

                    float clampedForce = Mathf.Min(pushForce, 5f);
                    enemyRb.linearDamping = 2f;
                    enemyRb.AddForce(pushDir * clampedForce, ForceMode.Impulse);
                }
            }
        }
        else if (collision.gameObject.CompareTag("DestructibleUI"))
        {
            Debug.Log($"[Weapon] Hit UI Element {collision.gameObject.name} | Velocity: {impactVelocity:F2}");

            MenuHitActivate column = collision.gameObject.GetComponentInParent<MenuHitActivate>();
            if (column != null && impactVelocity >= damageVelocityThreshold)
            {
                column.Die(transform);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isOnGround = false;
        }
    }
}
