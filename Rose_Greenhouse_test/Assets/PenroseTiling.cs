using UnityEngine;
using System.Linq;

public static class PenroseTiling
{
    public static void Generate(Transform parent, float triangleSize, int iterations, Material material)
    {
        Vector2[] points = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(0, Mathf.Sqrt(3)),
            new Vector2(-Mathf.Sqrt(3), -2),
            new Vector2(Mathf.Sqrt(3), -2),
        };

        int[][] triangles = new int[][]
        {
            new int[] { 0, 2, 1 },
            new int[] { 3, 1, 2 },
            new int[] { 3, 4, 1 },
            new int[] { 0, 1, 4 },
            new int[] { 0, 4, 2 },
        };

        for (int i = 0; i < iterations; i++)
        {
            Vector2[] newPoints = new Vector2[points.Length + 10];
            int[][] newTriangles = new int[triangles.Length * 3][];

            for (int j = 0; j < points.Length; j++)
            {
                newPoints[j] = points[j];
            }

            int p = points.Length;

            for (int j = 0; j < triangles.Length; j++)
            {
                int[] t = triangles[j];
                Vector2 p1 = points[t[0]];
                Vector2 p2 = points[t[1]];
                Vector2 p3 = points[t[2]];
                Vector2 p4 = p1 + (p2 - p1) /3f;
                Vector2 p5 = p2 + (p3 - p2) / 3f;
                Vector2 p6 = p1 + (p2 - p1) / 3f;
                Vector2 p7 = p1 + (p3 - p1) / 3f;
                Vector2 p8 = p1 + (p3 - p1) / 2f + (p2 - p1) / 2f / Mathf.Sqrt(3);
                Vector2 p9 = p8 + (p3 - p2) / 3f;
                Vector2 p10 = p8 + (p2 - p1) / 3f;
newPoints[p++] = p4;
newPoints[p++] = p6;
newPoints[p++] = p5;
newPoints[p++] = p7;
newPoints[p++] = p8;
newPoints[p++] = p10;
newPoints[p++] = p9;
newTriangles[j * 3] = new int[] { t[0], p + 1, p };
newTriangles[j * 3 + 1] = new int[] { p + 2, t[1], p + 1 };
newTriangles[j * 3 + 2] = new int[] { p + 2, p + 3, t[2] };
}
points = newPoints;
        triangles = newTriangles;
    }

    GameObject meshObject = new GameObject("Penrose Tiling");
    meshObject.transform.parent = parent;

    MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();

    Mesh mesh = new Mesh();
    mesh.vertices = System.Array.ConvertAll(points, v => new Vector3(v.x, v.y, 0f));
    mesh.triangles = System.Array.ConvertAll(triangles, t => new int[] { t[0], t[1], t[2] }).SelectMany(x => x).ToArray();
    mesh.RecalculateNormals();
    meshFilter.mesh = mesh;
    meshRenderer.material = material;
}
}
