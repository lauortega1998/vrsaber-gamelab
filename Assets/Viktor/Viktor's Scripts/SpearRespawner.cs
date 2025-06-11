using UnityEngine;

public class SpearRespawner : MonoBehaviour
{
    public GameObject spearPrefab;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool wasThrown = false;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void MarkAsThrown()
    {
        wasThrown = true;
    }

    void OnDestroy()
    {
        if (wasThrown && spearPrefab != null)
        {
            Instantiate(spearPrefab, originalPosition, originalRotation);
        }
    }
}