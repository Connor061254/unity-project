using UnityEngine;

// This component must be on the same GameObject as a MeshFilter.
[RequireComponent(typeof(MeshFilter))]
public class WaveMeshDeformer : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;
    
    // Store the original vertex positions of the mesh
    private Vector3[] baseVertices; 

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        
        // Save the mesh's original shape
        baseVertices = mesh.vertices; 
    }

    void Update()
    {
        // If the WaterManager doesn't exist yet, do nothing.
        if (WaterManager.instance == null)
        {
            return; 
        }

        // Create a new array to hold the modified vertex positions
        Vector3[] modifiedVertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            // Convert vertex position from local space to world space
            Vector3 worldPosition = transform.TransformPoint(vertex);

            // Get the wave height from the WaterManager at this specific point
            float waveHeight = WaterManager.instance.GetWaterHeightAtPosition(worldPosition);

            // Apply the calculated height to the vertex's Y position
            vertex.y = waveHeight;

            modifiedVertices[i] = vertex;
        }

        // Apply the newly calculated vertex positions back to the mesh
        mesh.vertices = modifiedVertices;

        // Recalculate the mesh normals for correct lighting
        mesh.RecalculateNormals();
    }
}