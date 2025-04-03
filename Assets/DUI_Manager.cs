using UnityEngine;

public class DUI_Manager : MonoBehaviour
{
    public GameObject objectToWatch;       // The object you're monitoring
    public GameObject objectToActivate;    // The UI or object to activate (after delay)
    public GameObject torchToActivate;     // Torch or flame object to activate (immediately)

    public float delayBeforeActivation = 4f;

    private bool hasActivated = false;

    void Update()
    {
        if (!hasActivated && objectToWatch != null && !objectToWatch.activeInHierarchy)
        {
            hasActivated = true;

            Debug.Log("[DUI_Manager] Watched object became inactive.");

            // Activate torch immediately
            if (torchToActivate != null)
            {
                torchToActivate.SetActive(true);
                Debug.Log("[DUI_Manager] Activated torch immediately.");
            }
            else
            {
                Debug.LogWarning("[DUI_Manager] No torch object assigned to activate.");
            }

            // ⏳ Delay activation of main object
            Invoke(nameof(ActivateDelayedObject), delayBeforeActivation);
        }
    }

    private void ActivateDelayedObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("[DUI_Manager] Activated main object after delay.");
        }
        else
        {
            Debug.LogWarning("[DUI_Manager] No object assigned to activate after delay.");
        }
    }
}
