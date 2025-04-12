using UnityEngine;
using System.Collections;

public class SmashableUI : MonoBehaviour
{
    [SerializeField] private float speedThreshold = 2.0f; // Speed needed to smash
    [SerializeField] private float pushForce = 5.0f;       // Force when not smashed
    [SerializeField] private float returnDelay = 2.0f;     // Time before returning
    [SerializeField] private float returnSpeed = 2.0f;     // Speed of floating back

    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.name);

        if (other.CompareTag("Weapon"))
        {
            Rigidbody weaponRb = other.attachedRigidbody;

            float impactSpeed = 0f;
            if (weaponRb != null)
            {
                impactSpeed = weaponRb.linearVelocity.magnitude;
            }

            Debug.Log("Weapon speed: " + impactSpeed);

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
        Destroy(gameObject);
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
}