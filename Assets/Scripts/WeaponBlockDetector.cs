using UnityEngine;

public class WeaponBlockDetector : MonoBehaviour
{
    [Tooltip("Optional: Perfect block angular tolerance")]
    public float perfectBlockTolerance = 10f;

    private DefendZone[] defendZones;

    public delegate void BlockEvent(bool perfect);
    public event BlockEvent OnBlock;

    private void Awake()
    {
        defendZones = GetComponentsInChildren<DefendZone>();
    }

    /// <summary>
    /// Called by enemy when their attack hits the weapon.
    /// </summary>
    /// <param name="incomingAttackDirection">Attack direction in world space.</param>
    public bool TryBlock(Vector3 incomingAttackDirection)
    {
        foreach (var zone in defendZones)
        {
            if (zone.IsBlockSuccessful(incomingAttackDirection))
            {
                bool perfect = zone.angleTolerance <= perfectBlockTolerance;

                OnBlock?.Invoke(perfect);

                Debug.Log(perfect ? "Perfect Block!" : "Block Successful");
                return true;
            }
        }

        Debug.Log("Block Failed");
        return false;
    }
}
