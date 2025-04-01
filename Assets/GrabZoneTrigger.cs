using UnityEngine;
using System;

public class GrabZoneTrigger : MonoBehaviour
{
    public Action<Collider> onEnter;
    public Action<Collider> onExit;

    private void OnTriggerEnter(Collider other)
    {
        onEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit?.Invoke(other);
    }
}
