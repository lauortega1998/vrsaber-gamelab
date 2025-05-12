using UnityEngine;

public class FlameCollisionHandler : MonoBehaviour
{
    private ParticleSystem flameParticles;

    void Start()
    {
        flameParticles = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        // Check if the other object has the EnemyHealth component
        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            // Use this flame's transform as the weaponHitPoint reference
            enemyHealth.Die(transform);
        }
    }
}
