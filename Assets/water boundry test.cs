using UnityEngine;

// This script forces the mesh bounds to be large every frame.
// Useful for procedural assets that reset their mesh.
[RequireComponent(typeof(MeshFilter))]
public class ForceBoundsUpdate : MonoBehaviour
{
    public Vector3 expansionSize = new Vector3(10000f, 10000f, 10000f);
    private MeshFilter meshFilter;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // LateUpdate runs after all other calculations in a frame.
    // This is the perfect place to override the bounds.
    void LateUpdate()
    {
        if (meshFilter != null && meshFilter.mesh != null)
        {
            // Forcibly re-apply the expanded bounds every frame.
            meshFilter.mesh.bounds = new Bounds(meshFilter.mesh.bounds.center, expansionSize);
        }
    }
}