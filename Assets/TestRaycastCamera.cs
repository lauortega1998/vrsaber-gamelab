using UnityEngine;

public class SimpleRayDebug : MonoBehaviour
{
    public float distance = 5f;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * distance, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }
    }
}