using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyAttackingTest : MonoBehaviour
{
    public float countdownTime = 3f;
    private bool timerStarted = false;
    private float timer;
    
    private PlayerHealth playerHealth;
    private int lastPrintedTime = -1;
    public bool isEnemyCollided = false;
    public LevelManager levelManager;
    public float raycastDistance = 10f;
    public LayerMask raycastLayers;
    public Transform raycastOrigin;
    private Transform raycastTarget;
    public GameObject enemyAttackEffect;
    public GameObject enemyAttackCollider;
    private Animator anim;
    public EnemyAttacCollision enemyattackScript;
    public bool enemy;
    public bool heavyenemy;
    
    [Header("Block Indicator UI")]
    public GameObject blockIndicatorCanvas;
    public Slider blockSlider;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        anim = GetComponent<Animator>();

        GameObject targetObject = GameObject.FindGameObjectWithTag("LookTarget");
        if (targetObject != null)
        {
            raycastTarget = targetObject.transform;
        }
        else
        {
            Debug.LogWarning("No object with tag 'LookTarget' found in the scene!");
        }

        if (blockIndicatorCanvas != null)
            blockIndicatorCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementStopper") && !timerStarted)
        {
            
            isEnemyCollided = true;
            EnemyManager.Instance.RegisterAttacker();
            StartTimer();
        }
        if (other.CompareTag("MovementStopper")  && !timerStarted)
        {
            
            isEnemyCollided = true;
            EnemyManager.Instance.RegisterAttacker();
            StartTimer();
        }
        
    }

    private void OnEnable()
    {
        ResetTimerState();
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            float progress = 1f - (timer / countdownTime);

            // Update UI slider
            if (blockSlider != null)
                blockSlider.value = progress;

            int currentSeconds = Mathf.CeilToInt(timer);
            if (currentSeconds != lastPrintedTime && currentSeconds >= 0)
            {
                //Debug.Log($"{gameObject.name} Timer: {currentSeconds} seconds remaining");
                lastPrintedTime = currentSeconds;
            }

            if (timer <= 0f)
            {
                if (isEnemyCollided)
                {
                    PerformAction();
                    StartTimer();
                }
            }
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
        timer = countdownTime;
        lastPrintedTime = -1;

        // Show block indicator UI
        if (blockIndicatorCanvas != null)
            blockIndicatorCanvas.SetActive(true);

        if (blockSlider != null)
        {
            blockSlider.minValue = 0;
            blockSlider.maxValue = 1;
            blockSlider.value = 0;
        }

        //Debug.Log($"{gameObject.name} Timer started!");
    }

    public void StopTimer()
    {
        timerStarted = false;
        timer = 0;

        if (blockIndicatorCanvas != null)
            blockIndicatorCanvas.SetActive(false);

       // Debug.Log($"{gameObject.name} Timer stopped/reset.");
    }

    public void ResetTimerState()
    {
        timerStarted = false;
        timer = 0;
        lastPrintedTime = -1;

        if (blockIndicatorCanvas != null)
            blockIndicatorCanvas.SetActive(false);
    }

    private void PerformAction()
    {
        anim.SetTrigger("attack");

        if (enemyAttackEffect != null)
        {
            enemyAttackEffect.SetActive(false);
            enemyAttackEffect.SetActive(true);
            StartCoroutine(DisableAttackEffectAfterDelay(enemyAttackEffect, 0.5f));
        }

        if (enemyAttackCollider != null)
        {
            enemyAttackCollider.SetActive(false);
            enemyAttackCollider.SetActive(true);

            enemyAttackCollider.GetComponent<EnemyAttacCollision>().ResetProtectionStatus();
            StartCoroutine(DisableAttackEffectAfterDelay(enemyAttackCollider, 0.5f));
        }

        if (blockIndicatorCanvas != null)
            blockIndicatorCanvas.SetActive(false);

        //Debug.Log("Attack performed!");
    }

    private IEnumerator DisableAttackEffectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
            obj.SetActive(false);
    }
}