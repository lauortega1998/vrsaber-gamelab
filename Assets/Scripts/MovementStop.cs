using UnityEngine;

public class FlyingEnemyWallStop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FlyingEnemyBehaviour enemy = other.GetComponent<FlyingEnemyBehaviour>();
        if (enemy != null)
        {
            enemy.speed = 0f;
            Debug.Log($"{other.name} hit the wall and was stopped.");
        }
        
        
    }
    
    
}
