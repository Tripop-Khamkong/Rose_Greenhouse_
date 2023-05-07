using UnityEngine;

public class Example : MonoBehaviour
{
    public GameObject penrosePrefab;
    private PenroseTileGenerator penrose;

    private void Start()
    {
        penrose = gameObject.AddComponent<PenroseTileGenerator>();
        penrose.tilePrefab = penrosePrefab;
        penrose.numIterations = 3;
        penrose.tileScale = 10f;
        penrose.GenerateTiles();
    }

    public void ChangeScale(float scale)
    {
        penrose.SetTileScale(scale);
    }

    public void ChangeColor(Color color)
    {
        penrose.SetTileColors(new Color[] { color });
    }

    public void RegenerateTiles()
    {
        penrose.GenerateTiles();
    }
}
