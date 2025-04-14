using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy
    private Transform player;
    private bool canMove = true;
    [SerializeField] private Transform lookTarget; // <- this also works and keeps it private in code
    private Animator anim; //animator reference
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
      
    }


    void Update()
    {   
        if (canMove && !EnemyManager.Instance.isAnyEnemyAttacking)
        {     
            // Move towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
             anim.SetBool("isRunning",true); // run forrest run ! 
               
        }
        else
        {    
             anim.SetBool("isIdle", true);
        }
        
    }
    public void StopMovement()
    {    
        canMove = false;
      
    }

    public void ResumeMovement()
    {       
         
        canMove = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper"))
        {
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            canMove = false;

            // Face the player immediately
    
        }
    }
}
