using UnityEngine;

public class WeaponRuneDestroyed : MonoBehaviour
{
    public float damageVelocityThreshold = 1.5f;
    public float pushForce = 5f;
    public Enemy enemyscript;
    private Rigidbody rb;
    private bool isOnGround = false;
    public GameObject incorrectStrikeEffect;  // Particle effect to instantiate on incorrect strike
    private int attackSoundIndex = 0;



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
            return; // Early exit, no further logic when touching ground
        }

        if (isOnGround) return; // Don't damage enemies if on ground

        if (collision.gameObject.CompareTag("Enemy"))
        {  // Haptics again 
            HapticsManager.Instance.TriggerHaptics(0.5f, 0.1f);

            Debug.Log("CollidedWithEnemy");

            var enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                if (impactVelocity >= damageVelocityThreshold)
                {
                    Debug.Log("[WeaponVelocityDamage] Killing enemy.");
                    enemy.Die(transform);
                    FindAnyObjectByType<AudioManager>().Play("death1scream");

                }
                Debug.Log("[WeaponVelocityDamage] Pushing enemy.");
                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                    pushDir.y = 0;

                    float clampedForce = Mathf.Min(pushForce, 5f); // prevent over-push
                    enemyRb.linearDamping = 2f; // increase drag to reduce sliding
                    enemyRb.AddForce(pushDir * clampedForce, ForceMode.Impulse);
                    PlayNextAttackSound();
                }
            }
        }
        else if (collision.gameObject.CompareTag("DestructibleUI"))
        {   // Haptics again for the Destructible UI
            HapticsManager.Instance.TriggerHaptics(0.5f, 0.1f);
            Debug.Log($"[WeaponVelocityDamage] Hit Column {collision.gameObject.name} | Velocity: {impactVelocity:F2}");

            var column = collision.gameObject.GetComponentInParent<MenuHitActivate>();
            if (column != null && impactVelocity >= damageVelocityThreshold)
            {
                Debug.Log("[WeaponVelocityDamage] Breaking column to start the game.");
                column.Die(transform);
            }
        }
        //Logic for heavy enemy
        if (collision.gameObject.CompareTag("HeavyEnemy"))
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
                    FindAnyObjectByType<AudioManager>().Play("death2scream");
                }
                else
                {
                    Debug.Log("[Weapon] Incorrect strike direction or too slow.");
                    if (incorrectStrikeEffect != null && collision.contacts.Length > 0)
                    {
                        Vector3 impactPoint = collision.contacts[0].point;
                        Instantiate(incorrectStrikeEffect, impactPoint, Quaternion.identity);
                        PlayNextAttackSound();

                    }
                }

                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                    pushDir.y = 0;

                    float clampedForce = Mathf.Min(pushForce, 2f);
                    enemyRb.linearDamping = 2f;
                    enemyRb.AddForce(pushDir * clampedForce, ForceMode.Impulse);


                }
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
    private void PlayNextAttackSound()
    {
        string soundName = $"attack{attackSoundIndex + 1}"; // "Attack1", "Attack2", "Attack3"
        FindAnyObjectByType<AudioManager>()?.Play(soundName);

        attackSoundIndex = (attackSoundIndex + 1) % 3; // Loop: 0 → 1 → 2 → 0 ...
    }
}