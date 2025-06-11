using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    public GameObject enemyFactory; // Reference to the enemy factory to activate when the tutorial enemy dies

    // Method to activate the enemy factory
    public void ActivateEnemyFactory()
    {
        if (enemyFactory != null)
        {
            enemyFactory.SetActive(true); // Activate the enemy factory
        }
    }
}