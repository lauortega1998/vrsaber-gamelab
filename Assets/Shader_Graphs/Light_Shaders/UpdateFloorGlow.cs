using UnityEngine;

public class UpdateFloorGlow : MonoBehaviour 
{
    public Transform player;         // Drag your player object here
    public Renderer glowRenderer;    // Drag the plane's Renderer here

    private Material _mat;

    void Start()
    {
        _mat = glowRenderer.material;
    }

    void Update()
    {
        Vector3 pos = player.position;
        _mat.SetVector("_CenterPosition", new Vector4(pos.x, pos.y, pos.z, 1));
    }
}
