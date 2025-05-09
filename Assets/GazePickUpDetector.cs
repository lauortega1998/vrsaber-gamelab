using UnityEngine;

public class GazePickupDetector : MonoBehaviour
{
   /* public float gazeDistance = 5f;
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
        
        
        
    }*/
   
   
   
   
  public float gazeDistance = 5f;
    public LayerMask interactableLayer = ~0;

    public bool pickedUp = false;
    private GameObject currentTarget;

    private bool hasClearedUI = false;

    void Update()
    {
        // 1. Gaze logic
        if (!pickedUp)
        {
            Ray gazeRay = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(gazeRay, out hit, gazeDistance, interactableLayer))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject != currentTarget)
                {
                    if (currentTarget != null)
                        TogglePickupIndicator(currentTarget, false);

                    currentTarget = hitObject;
                    TogglePickupIndicator(currentTarget, true);
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    TogglePickupIndicator(currentTarget, false);
                    currentTarget = null;
                }
            }
        }

        // 2. Watch for bool from Inspector
        if (pickedUp && !hasClearedUI)
        {
            DisableAllPickupIndicators();
            hasClearedUI = true;
        }
    }

    void DisableAllPickupIndicators()
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject obj in all)
        {
            Transform indicator = obj.transform.Find("PickupIndicator");
            if (indicator != null)
                indicator.gameObject.SetActive(false);
        }

        Debug.Log("All pickup UIs disabled.");
    }

    void TogglePickupIndicator(GameObject target, bool state)
    {
        Transform indicator = target.transform.Find("PickupIndicator");

        if (indicator != null)
        {
            UnityEngine.UI.Image image = indicator.GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, state ? 1f : 0.3f);
                RectTransform imgRect = image.GetComponent<RectTransform>();
                imgRect.sizeDelta = state ? new Vector2(150, 150) : new Vector2(100, 100);
            }
        }
    }
}


