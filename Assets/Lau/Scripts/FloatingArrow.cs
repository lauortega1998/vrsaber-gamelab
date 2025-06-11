using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float floatSpeed = 2f;      // Speed of the up-down motion
    public float floatAmplitude = 0.5f; // Height of the motion

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
