using UnityEngine;

public class GazePickupDetector : MonoBehaviour
{
    public float gazeDistance = 5f;
    public LayerMask interactableLayer = ~0; // Default to everything

    private GameObject currentTarget;

    void Update()
    {
        Ray gazeRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        float sphereRadius = 5f; // Adjust this for "thickness"

        if (Physics.Raycast(gazeRay, out hit, gazeDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentTarget)
            {
                ClearPreviousTarget();

                currentTarget = hitObject;
                TogglePickupIndicator(currentTarget, true);
                Debug.Log("Gazing at: " + hitObject.name);
            }
        }
        else
        {
            ClearPreviousTarget();
        }
    }

    void ClearPreviousTarget()
    {
        if (currentTarget != null)
        {
            TogglePickupIndicator(currentTarget, false);
            currentTarget = null;
        }
    }

    void TogglePickupIndicator(GameObject target, bool state)
    {
        // Search only within this target�s hierarchy
        Canvas foundCanvas = target.GetComponentInChildren<Canvas>(true);

        if (foundCanvas != null)
        {
            foundCanvas.gameObject.SetActive(state);
        }
        else
        {
            Debug.LogWarning("PickupIndicator not found on: " + target.name);
        }
        
        
        
    }
}
