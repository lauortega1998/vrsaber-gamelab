using UnityEngine;
using System.Collections;

public class SmashableUI : MonoBehaviour
{
    public enum RuneType { Play, Exit, Scoreboard, GoBack }
    public GazePickupDetector gazePickupDetectorscript;
    [Header("Rune Settings")]
    public RuneType runeType;

    [Header("Smash Settings")]
    [SerializeField] private float speedThreshold = 2.0f; // Speed needed to smash
    [SerializeField] private float pushForce = 5.0f;       // Force when not smashed
    [SerializeField] private float returnDelay = 2.0f;     // Time before returning
    [SerializeField] private float returnSpeed = 2.0f;     // Speed of floating back

    [Header("Broken Rune")]
    [SerializeField] private GameObject brokenRune; // Drag your broken rune here in Inspector!

    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false;
    private UIManager uiManager;

    private bool isActive = true;

    private Vector3 brokenRuneOriginalPosition;
    private Quaternion brokenRuneOriginalRotation;

    [Header("Destruction Settings")]
    [SerializeField] private float destructionForceMin = 15f; // Minimum random force
    [SerializeField] private float destructionForceMax = 30f; // Maximum random force
    [SerializeField] private float destructionTorqueStrength = 10f; // Spin strength

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        uiManager = FindObjectOfType<UIManager>();

        if (brokenRune != null)
        {
            brokenRuneOriginalPosition = brokenRune.transform.localPosition;
            brokenRuneOriginalRotation = brokenRune.transform.localRotation;
            brokenRune.SetActive(false); // Make sure broken rune starts hidden
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return; // Skip if not active yet

        if (other.CompareTag("Weapon"))
        {
            Rigidbody weaponRb = other.attachedRigidbody;
            
            
            float impactSpeed = 0f;
            if (weaponRb != null)
            {
                impactSpeed = weaponRb.linearVelocity.magnitude;
            }

            if (impactSpeed >= speedThreshold)
            {
                Smash();
            }
            else
            {
                PushAway(other);
            }
        }
    }

    private void Smash()
    {
        Debug.Log($"{gameObject.name} was smashed!");

        if (uiManager != null)
        {
            switch (runeType)
            {
                case RuneType.Play:
                    uiManager.OnPlayRuneSmashed();
                    break;
                case RuneType.Exit:
                    uiManager.OnExitRuneSmashed();
                    break;
                case RuneType.Scoreboard:
                    uiManager.OnScoreboardRuneSmashed();
                    break;
                case RuneType.GoBack:
                    uiManager.OnGoBackRuneSmashed();
                    break;
            }
        }

        if (brokenRune != null)
        {
            brokenRune.SetActive(true);

            // Apply small random impulse if it has a Rigidbody
            Rigidbody[] brokenRigidbodies = brokenRune.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody brokenRb in brokenRigidbodies)
            {
                if (brokenRb != null)
                {
                    brokenRb.isKinematic = false; // Allow movement

                    Vector3 randomDirection = (Vector3.up + Random.insideUnitSphere * 1f).normalized;
                    float randomForce = Random.Range(destructionForceMin, destructionForceMax);
                    brokenRb.AddForce(randomDirection * randomForce, ForceMode.Impulse);

                    // Optional: Add random spin based on inspector value
                    brokenRb.AddTorque(Random.insideUnitSphere * destructionTorqueStrength, ForceMode.Impulse);
                }
            }
            StartCoroutine(DisableBrokenRuneAfterDelay(brokenRune));
        }
        else
        {
            Debug.LogWarning("No broken rune assigned on " + gameObject.name);
        }

        // Hide this rune
        gameObject.SetActive(false);
    }

    private IEnumerator DisableBrokenRuneAfterDelay(GameObject brokenRune)
    {
        yield return new WaitForSeconds(1f);
        brokenRune.SetActive(false);
    }

    private void PushAway(Collider other)
    {
        Vector3 pushDirection = (transform.position - other.transform.position).normalized;
        rb.isKinematic = false; // Allow movement
        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

        if (!isReturning)
        {
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsed);
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.isKinematic = true;
        isReturning = false;
    }

    public void ResetRune()
    {
        // Reactivate the main rune
        gameObject.SetActive(true);

        if (brokenRune != null)
        {
            brokenRune.SetActive(false);
            brokenRune.transform.localPosition = brokenRuneOriginalPosition;
            brokenRune.transform.localRotation = brokenRuneOriginalRotation;

            Rigidbody brokenRb = brokenRune.GetComponent<Rigidbody>();
            if (brokenRb != null)
            {
                brokenRb.linearVelocity = Vector3.zero;
                brokenRb.angularVelocity = Vector3.zero;
            }
        }
    }
}



/*using UnityEngine;
using System.Collections;

public class SmashableUI : MonoBehaviour
{
    public enum RuneType { Play, Exit, Scoreboard, GoBack }
    [Header("Rune Settings")]
    public RuneType runeType;

    [Header("Smash Settings")]
    [SerializeField] private float speedThreshold = 2.0f; // Speed needed to smash
    [SerializeField] private float pushForce = 5.0f;       // Force when not smashed
    [SerializeField] private float returnDelay = 2.0f;     // Time before returning
    [SerializeField] private float returnSpeed = 2.0f;     // Speed of floating back

    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false;
    private UIManager uiManager;

    private bool isActive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return; // Skip if not active yet

        if (other.CompareTag("Weapon"))
        {
            Rigidbody weaponRb = other.attachedRigidbody;

            float impactSpeed = 0f;
            if (weaponRb != null)
            {
                impactSpeed = weaponRb.linearVelocity.magnitude;
            }

            if (impactSpeed >= speedThreshold)
            {
                Smash();
            }
            else
            {
                PushAway(other);
            }
        }
    }

    private void Smash()
    {
        Debug.Log($"{gameObject.name} was smashed!");

        if (uiManager != null)
        {
            switch (runeType)
            {
                case RuneType.Play:
                    uiManager.OnPlayRuneSmashed();
                    break;
                case RuneType.Exit:
                    uiManager.OnExitRuneSmashed();
                    break;
                case RuneType.Scoreboard:
                    uiManager.OnScoreboardRuneSmashed();
                    break;
                case RuneType.GoBack:
                    uiManager.OnGoBackRuneSmashed();
                    break;
            }
        }

        // Instead of destroying, just hide it
        gameObject.SetActive(false);
    }

    private void PushAway(Collider other)
    {
        Vector3 pushDirection = (transform.position - other.transform.position).normalized;
        rb.isKinematic = false; // Allow movement
        rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

        if (!isReturning)
        {
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsed);
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.isKinematic = true;
        isReturning = false;
    }
}*/