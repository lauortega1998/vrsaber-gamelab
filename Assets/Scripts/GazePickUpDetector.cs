using UnityEngine;
using UnityEngine.UI;

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

    private GameObject currentTarget;

    void Update()
    {
        Ray gazeRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(gazeRay, out hit, gazeDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentTarget)
            {
                // Clear the previous indicator (shrink + fade it)
                if (currentTarget != null)
                    TogglePickupIndicator(currentTarget, false);

                currentTarget = hitObject;

                // Expand + brighten the new target's indicator
                TogglePickupIndicator(currentTarget, true);
                Debug.Log("Gazing at: " + hitObject.name);
            }
        }
        else
        {
            // Raycast hit nothing — fade/shrink the previous
            if (currentTarget != null)
            {
                TogglePickupIndicator(currentTarget, false);
                currentTarget = null;
            }
        }
    }

    void TogglePickupIndicator(GameObject target, bool state)
    {
        Transform indicator = target.transform.Find("PickupIndicator");

        if (indicator != null)
        {
            GameObject indicatorObj = indicator.gameObject;

            // No SetActive — we keep it always on
            // Find the Image inside the canvas
            UnityEngine.UI.Image image = indicatorObj.GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null)
            {
                RectTransform imgRect = image.GetComponent<RectTransform>();
                if (imgRect != null)
                {
                    imgRect.sizeDelta =
                        state ? new Vector2(150, 150) : new Vector2(100, 100); // adjust values to your liking
                }

                // Optional: also change color/opacity
                Color color = image.color;
                color.a = state ? 1f : 0.3f;
                image.color = color;
            }

            // Optional: you can still use CanvasGroup for global fading (if needed)
            // But not necessary if you control the image color directly
        }
        else
        {
            Debug.LogWarning("PickupIndicator not found on: " + target.name);
        }
    }
}

