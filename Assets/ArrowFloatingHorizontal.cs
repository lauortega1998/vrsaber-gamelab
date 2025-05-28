using UnityEngine;

public class FloatingArrowHorizontal : MonoBehaviour
{
    public float floatSpeed = 2f;       // Speed of side-to-side motion
    public float floatAmplitude = 0.5f; // Distance of side-to-side motion

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newX = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(newX, 0, 0);
    }
}
