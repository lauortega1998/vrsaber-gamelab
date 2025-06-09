using UnityEngine;

public class ActivateObjectsWithDelay : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float delay = 2f; // Delay in seconds

    void Start()
    {
        // Start the coroutine that waits and activates the objects
        StartCoroutine(ActivateAfterDelay());
    }

    private System.Collections.IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (object1 != null) object1.SetActive(true);
        if (object2 != null) object2.SetActive(true);
    }
}
