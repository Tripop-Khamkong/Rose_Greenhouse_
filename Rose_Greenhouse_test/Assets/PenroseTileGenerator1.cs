using UnityEngine;
using System.Collections.Generic;

public class PenroseTileGenerator1 : MonoBehaviour
{
    public GameObject tilePrefab;
    public int numIterations;
    public float tileScale = 1f;
    public Color[] tileColors = { Color.white };

    private PenroseTile tile;
    private int colorIndex = 0;

    private void Start()
    {
        tile = CreateInitialTile();
        GenerateTiles();
    }

    private PenroseTile CreateInitialTile()
    {
        Vector2[] vertices = new Vector2[] {
            new Vector2(-1f, 0f),
            new Vector2(-0.5f, Mathf.Sqrt(3) / 2f),
            new Vector2(0.5f, Mathf.Sqrt(3) / 2f),
            new Vector2(1f, 0f),
            new Vector2(0.5f, -Mathf.Sqrt(3) / 2f),
            new Vector2(-0.5f, -Mathf.Sqrt(3) / 2f)
        };

        PenroseTile tile = new PenroseTile(vertices);

        GameObject tileObject = Instantiate(tilePrefab, Vector2.zero, Quaternion.identity);
        tileObject.transform.localScale = new Vector3(tileScale, tileScale, 1f);
        tileObject.GetComponent<SpriteRenderer>().color = tileColors[colorIndex];
        DrawTile(tileObject, tile.vertices);

        colorIndex = (colorIndex + 1) % tileColors.Length;

        return tile;
    }

    private void GenerateTiles()
    {
        PenroseTile[] newTiles = new PenroseTile[1] { tile };
        for (int iter = 0; iter < numIterations; iter++)
        {
            int numNewTiles = 0;
            for (int i = 0; i < newTiles.Length; i++)
            {
                PenroseTile[] subTiles = newTiles[i].Subdivide();
                numNewTiles += subTiles.Length;
            }
            PenroseTile[] allNewTiles = new PenroseTile[numNewTiles];
            int tileIndex = 0;
            for (int i = 0; i < newTiles.Length; i++)
            {
                PenroseTile[] subTiles = newTiles[i].Subdivide();
                for (int j = 0; j < subTiles.Length; j++)
                {
                    allNewTiles[tileIndex] = subTiles[j];
                    tileIndex++;
                }
            }
            newTiles = allNewTiles;

            for (int i = 0; i < newTiles.Length; i++)
            {
                GameObject tileObject = Instantiate(tilePrefab, newTiles[i].center, Quaternion.identity);
                tileObject.transform.localScale = new Vector3(tileScale, tileScale, 1f);
                tileObject.GetComponent<SpriteRenderer>().color = tileColors[colorIndex];
                DrawTile(tileObject, newTiles[i].vertices);

                colorIndex = (colorIndex + 1) % tileColors.Length;
            }
        }
    }

    private void DrawTile(GameObject tileObject, Vector2[] vertices)
    {
        LineRenderer lineRenderer = tileObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertices.Length + 1;
        for (int i = 0; i < vertices.Length; i++)
        {
            lineRenderer.SetPosition(i, vertices[i]);
        }
        lineRenderer.SetPosition(vertices.Length, vertices[0]);
    }

    public void SetTileScale(float scale)
    {
        tileScale = scale;
        GenerateTiles();
    }

    public void SetTileColors(Color[] colors)
    {
    tileColors = colors;
    }

public class PenroseTile
{
    public Vector2[] vertices;
    public Vector2 center;

    private static float goldenRatio = (1f + Mathf.Sqrt(5f)) / 2f;

    public PenroseTile(Vector2[] vertices)
    {
        this.vertices = vertices;
        center = Vector2.zero;
        for (int i = 0; i < vertices.Length; i++)
        {
            center += vertices[i];
        }
        center /= vertices.Length;
    }

    public PenroseTile[] Subdivide()
    {
        PenroseTile[] subTiles = new PenroseTile[10];

        // Define matrices for transformations
        Matrix4x4 rotation = Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, 36f));
        Matrix4x4 reflection = Matrix4x4.Scale(new Vector3(-1f, 1f, 1f));

        // Subdivide into ten tiles
        for (int i = 0; i < 10; i++)
        {
            Vector2[] newVertices = new Vector2[3];
            if (i < 5)
            {
                newVertices[0] = vertices[i];
                newVertices[1] = vertices[(i + 1) % 5];
                newVertices[2] = newVertices[1] + goldenRatio * (newVertices[1] - vertices[(i + 4) % 5]);
            }
            else
            {
                newVertices[0] = vertices[i - 5];
                newVertices[1] = vertices[(i - 4) % 5];
                newVertices[2] = newVertices[0] + goldenRatio * (newVertices[0] - vertices[(i - 6) % 5]);
            }

            // Apply transformation
            if (i % 2 == 0)
            {
                newVertices = ApplyMatrix(reflection, newVertices);
            }
            newVertices = ApplyMatrix(rotation, newVertices);

            subTiles[i] = new PenroseTile(newVertices);
        }

        return subTiles;
    }

    private Vector2[] ApplyMatrix(Matrix4x4 matrix, Vector2[] points)
{
    Vector2[] transformedPoints = new Vector2[points.Length];
    for (int i = 0; i < points.Length; i++)
    {
        Vector4 homogeneousPoint = new Vector4(points[i].x, points[i].y, 0f, 1f);
        Vector4 transformedPoint = matrix * homogeneousPoint;
        transformedPoints[i] = new Vector2(transformedPoint.x, transformedPoint.y);
    }
    return transformedPoints;
}

}
}