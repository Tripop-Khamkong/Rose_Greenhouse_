using UnityEngine;

public class PenroseTilingGenerator : MonoBehaviour
{
    public Material material;
    public float size = 10f;
    public int depth = 3;

    private Mesh mesh;

    void Start()
{
    // Add MeshFilter component if it doesn't exist
    MeshFilter meshFilter = GetComponent<MeshFilter>();
    if (meshFilter == null)
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
    }
    
    // Add MeshRenderer component if it doesn't exist
    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    if (meshRenderer == null)
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    mesh = new Mesh();
    meshFilter.mesh = mesh;

    GenerateTiling();
}


    void GenerateTiling()
{
    // Define the initial rhombus shapes
    Vector3[] vertices =
    {
        new Vector3(-1f, 0f, 0f),
        new Vector3(0.5f, 0.5f * Mathf.Sqrt(3f), 0f),
        new Vector3(0.5f, -0.5f * Mathf.Sqrt(3f), 0f),
        new Vector3(1f, 0f, 0f)
    };

    int[] triangles =
    {
        0, 1, 2,
        0, 2, 3
    };

    // Apply the initial shape to the mesh
    mesh.vertices = vertices;
    mesh.triangles = triangles;

    // Recursively subdivide the shape to create the tiling
    for (int i = 0; i < depth; i++)
    {
        Vector3[] newVertices = new Vector3[mesh.vertices.Length * 3];
        int[] newTriangles = new int[mesh.triangles.Length * 3];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int j = 0; j < mesh.triangles.Length; j += 3)
        {
            int v1 = mesh.triangles[j];
            int v2 = mesh.triangles[j + 1];
            int v3 = mesh.triangles[j + 2];

            if (v1 >= mesh.vertices.Length || v2 >= mesh.vertices.Length || v3 >= mesh.vertices.Length)
            {
                Debug.LogError("Invalid triangle vertex indices: " + v1 + ", " + v2 + ", " + v3);
                continue;
            }

            Vector3 a = mesh.vertices[v1];
            Vector3 b = mesh.vertices[v2];
            Vector3 c = mesh.vertices[v3];

            Vector3 ab = Vector3.Lerp(a, b, 0.5f);
            Vector3 ac = Vector3.Lerp(a, c, 0.5f);
            Vector3 bc = Vector3.Lerp(b, c, 0.5f);

            newVertices[vertexIndex++] = a;
            newVertices[vertexIndex++] = ab;
            newVertices[vertexIndex++] = ac;

            newVertices[vertexIndex++] = ab;
            newVertices[vertexIndex++] = b;
            newVertices[vertexIndex++] = bc;

            newVertices[vertexIndex++] = ac;
            newVertices[vertexIndex++] = bc;
            newVertices[vertexIndex++] = c;

            newVertices[vertexIndex++] = ab;
            newVertices[vertexIndex++] = ac;
            newVertices[vertexIndex++] = bc;

            int v4 = vertexIndex - 4;

            newTriangles[triangleIndex++] = v1;
            newTriangles[triangleIndex++] = v4;
            newTriangles[triangleIndex++] = v2;

            newTriangles[triangleIndex++] = v4;
            newTriangles[triangleIndex++] = v3;
            newTriangles[triangleIndex++] = v2;

            newTriangles[triangleIndex++] = v4 + 0;
            newTriangles[triangleIndex++] = v4 + 1;
            newTriangles[triangleIndex++] = v4 + 2;

            newTriangles[triangleIndex++] = v4 + 1;
            newTriangles[triangleIndex++] = v4 + 3;
            newTriangles[triangleIndex++] = v4 + 2;
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
    }

    // Scale the tiling and center it in


        // Scale the tiling and center it in the scene
        mesh.RecalculateBounds();
        transform.localScale = Vector3.one * size / mesh.bounds.size.x;
        transform.position = -mesh.bounds.center * size / mesh.bounds.size.x;
    }
}
