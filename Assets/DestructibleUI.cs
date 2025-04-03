using UnityEngine;

public class DestructibleActivator : MonoBehaviour
{
    public GameObject objectToActivate;      // e.g. timer UI
    public GameObject objectToDeactivate;    // e.g. menu UI

    public string destructibleTag = "DestructibleUI";
    public float requiredVelocity = 1f;
    public float activationDuration = 3f;    // time before objectToActivate is hidden

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (objectToActivate != null)
            objectToActivate.SetActive(false); // ensure it's off at start
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(destructibleTag))
        {
            float impactVelocity = rb.linearVelocity.magnitude;
            Debug.Log($"[DestructibleActivator] Collision with {collision.gameObject.name} | Velocity: {impactVelocity:F2}");

            if (impactVelocity >= requiredVelocity)
            {
                Debug.Log("[DestructibleActivator] Activating and deactivating target objects!");

                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                    Invoke(nameof(HideActivatedObject), activationDuration);
                }
                else
                {
                    Debug.LogWarning("[DestructibleActivator] No object assigned to activate.");
                }

                if (objectToDeactivate != null)
                {
                    objectToDeactivate.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("[DestructibleActivator] No object assigned to deactivate.");
                }
            }
        }
    }

    private void HideActivatedObject()
    {
        if (objectToActivate != null)
            objectToActivate.SetActive(false);
    }
}