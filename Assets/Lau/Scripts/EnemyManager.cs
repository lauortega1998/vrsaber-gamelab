using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public bool isAnyEnemyAttacking = false;

    private int currentAttackers = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterAttacker()
    {
        currentAttackers++;
        Debug.Log("Attacker registered. Total: " + currentAttackers);

        if (currentAttackers >= 4)
            isAnyEnemyAttacking = true;
    }

    public void UnregisterAttacker()
    {
        currentAttackers = Mathf.Max(0, currentAttackers - 1);
        Debug.Log("Attacker removed. Total: " + currentAttackers);

        if (currentAttackers < 4)
            isAnyEnemyAttacking = false;
    }
}