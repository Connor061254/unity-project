using UnityEngine;

// This tells Unity to automatically add these components when you add this script.
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CreatePlane : MonoBehaviour
{
    // The number of subdivisions along the X and Z axes.
    // Higher numbers = smaller triangles.
    public int xSize = 20;
    public int zSize = 20;

    private Mesh mesh;

    void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        // Get the MeshFilter component and create a new mesh.
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Create arrays to hold the mesh data.
        // We need (size+1) vertices to create 'size' number of quads.
        Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        int[] triangles = new int[xSize * zSize * 6]; // 6 vertices per quad (2 triangles)
        Vector2[] uv = new Vector2[vertices.Length];

        // --- Generate the Vertices and UVs ---
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                // Position the vertices in a grid.
                vertices[i] = new Vector3(x, 0, z);
                // Set the UV coordinates for texturing.
                uv[i] = new Vector2((float)x / xSize, (float)z / zSize);
            }
        }

        // --- Generate the Triangles ---
        for (int vert = 0, tris = 0, z = 0; z < zSize; z++, vert++)
        {
            for (int x = 0; x < xSize; x++, vert++, tris += 6)
            {
                // First triangle of the quad
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                // Second triangle of the quad
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
            }
        }

        // --- Assign the data to the mesh ---
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.name = "Procedural Plane";
    }
}