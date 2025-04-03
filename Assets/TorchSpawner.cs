using System.Collections;
using UnityEngine;

public class SequentialActivator : MonoBehaviour
{
    public GameObject[] objectsToActivate; // Assign 6 GameObjects in the Inspector
    public float interval = 1f;            // Time between each activation step (in seconds)

    void Start()
    {
        if (objectsToActivate.Length != 6)
        {
            Debug.LogWarning("[SequentialActivator] Please assign exactly 6 GameObjects.");
            return;
        }

        StartCoroutine(ActivateInPairs());
    }

    IEnumerator ActivateInPairs()
    {
        for (int i = 0; i < objectsToActivate.Length; i += 2)
        {
            if (i < objectsToActivate.Length)
                objectsToActivate[i].SetActive(true);

            if (i + 1 < objectsToActivate.Length)
                objectsToActivate[i + 1].SetActive(true);

            Debug.Log($"[SequentialActivator] Activated objects {i + 1} and {i + 2}");

            yield return new WaitForSeconds(interval);
        }
    }
}