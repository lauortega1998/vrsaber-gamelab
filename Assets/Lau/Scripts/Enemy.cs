using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy
    private Transform player;
    private bool canMove = true;
    [SerializeField] private Transform lookTarget; // private look target
    private Animator anim; // animator reference
    private bool isAtWall = false; // New variable to check if enemy is at wall

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lookTarget = GameObject.FindGameObjectWithTag("LookTransform").transform;
        FindAnyObjectByType<AudioManager>().Play("walking loop");

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove && !EnemyManager.Instance.isAnyEnemyAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
         
        }
        else
        {
            // anim.SetBool("isIdle", true);
        }

        if (isAtWall)
        {
            FacePlayer();
        }
    }

    public void StopMovement()
    {
        canMove = false;
       anim.SetBool("isWalking", false);
        anim.SetBool("IsIdle",true);
        FindAnyObjectByType<AudioManager>().Stop("walking loop");

    }

    public void ResumeMovement()
    {
        canMove = true;
       anim.SetBool("IsIdle", false);
        anim.SetBool("isWalking",true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementStopper"))
        {
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            StopMovement();
            isAtWall = true; // Start facing the player
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovementStopper"))
        {
            ResumeMovement();
            isAtWall = false; // Stop facing the player
        }
    }

    private void FacePlayer()
    {
        if (lookTarget == null) return;

        Vector3 direction = (lookTarget.position + transform.position).normalized;
        direction.y = 0f; // Keep only horizontal rotation (don't tilt up/down)

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth turn
        }
    }
}

/*using UnityEngine;

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
       // anim = GetComponent<Animator>();
      
    }


    void Update()
    {
        
       
        if (canMove  && !EnemyManager.Instance.isAnyEnemyAttacking)
        {     
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
             //anim.SetBool("isRunning",true); // run forrest run ! 
               
        }
        else
        {    
             //anim.SetBool("isIdle", true);
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
    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovementStopper"))
        {
            Debug.Log($"{gameObject.name} hit a MovementStopper. Stopping movement.");
            StopMovement();

            
    
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        {
            if (collision.gameObject.CompareTag("MovementStopper"))
            {
                
                ResumeMovement();
            }
        }

    }
}
*/