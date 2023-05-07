using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenroseTileGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int numIterations;
    public float tileScale = 1f;
    public Color[] tileColors = { Color.white };

    public PenroseTile tile;
    private int colorIndex = 0;

    private List<PenroseTile> tiles = new List<PenroseTile>();

    private void Start()
    {  
            tile = CreateInitialTile();
            GenerateTiles(); 
    }

    public PenroseTile CreateInitialTile()
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
        
        DrawTile(tileObject, tile.vertices);
        tileObject.GetComponent<SpriteRenderer>().sharedMaterial.color = tileColors[colorIndex];

        

        return tile;
    }

    public void GenerateTiles()
    {
        tiles.Clear();
        tiles.Add(tile);
        for (int iter = 0; iter < numIterations; iter++)
        {
            List<PenroseTile> newTiles = new List<PenroseTile>();
            foreach (PenroseTile currentTile in tiles)
            {
                PenroseTile[] subTiles = currentTile.Subdivide();
                newTiles.AddRange(subTiles);
            }
            tiles = newTiles;

            for (int i = 0; i < tiles.Count; i++)
            {
                GameObject tileObject = Instantiate(tilePrefab, tiles[i].center, Quaternion.identity);
                tileObject.transform.localScale = new Vector3(tileScale, tileScale, 1f);
                //tileObject.GetComponent<MeshRenderer>().material.color = tileColors[colorIndex];
                DrawTile(tileObject, tiles[i].vertices);
                tileObject.GetComponent<SpriteRenderer>().sharedMaterial.color = tileColors[colorIndex];

                //colorIndex = (colorIndex + 1) % tileColors.Length;
            }
        }
    }

    private void DrawTile(GameObject tileObject, Vector2[] vertices)
    {
        Mesh mesh = new Mesh();
        tileObject.GetComponent<MeshFilter>().mesh = mesh;
        List<Vector3> vertexList = new List<Vector3>();
        for (int i = 0; i < vertices.Length; i++)
        {
            vertexList.Add(vertices[i]);
        }
        mesh.SetVertices(vertexList);
        List<int> indexList = new List<int>();
        for (int i = 0; i < vertices.Length - 2; i++)
        {
            indexList.Add(0);
            indexList.Add(i + 1);
            indexList.Add(i + 2);
        }
        mesh.SetTriangles(indexList, 0);
    }

    public void SetTileScale(float scale)
    {
        tileScale = scale;
        GenerateTiles();
    }

    public void SetTileColors(Color[] colors)
    {
        tileColors = colors;
        GenerateTiles();
    }
    
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

public Vector2[] ApplyMatrix(Matrix4x4 matrix, Vector2[] points)
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


