using UnityEngine;
using System.Collections.Generic;

public class testPenrose : MonoBehaviour {
    public GameObject kitePrefab;
    public GameObject dartPrefab;
    public int iterations = 3;

    void Start() {
        GenerateTiling();
    }

    void GenerateTiling() {
        // Define constants for the angles and lengths used in the rhombus method
        float phi = (1 + Mathf.Sqrt(5)) / 2;
        float angleA = Mathf.PI / 5;
        float angleB = 2 * Mathf.PI / 5;
        float lengthA = 1;
        float lengthB = lengthA * phi;

        // Create a list to store the game objects for the kites and darts
        List<GameObject> tiles = new List<GameObject>();

        // Create an initial rhombus and add it to the list
        Vector2 position = Vector2.zero;
        GameObject rhombus = Instantiate(kitePrefab, position, Quaternion.identity);
        tiles.Add(rhombus);

        // Perform the rhombus method for the specified number of iterations
        for (int i = 0; i < iterations; i++) {
            // Create a new list to store the next level of tiles
            List<GameObject> nextTiles = new List<GameObject>();

            // Iterate over the current level of tiles
            foreach (GameObject tile in tiles) {
                // Check if the tile is a kite or a dart
                bool isKite = tile.CompareTag("Kite");

                // Calculate the positions and rotations of the new tiles
                Vector2[] positions = new Vector2[isKite ? 2 : 5];
                Quaternion[] rotations = new Quaternion[isKite ? 2 : 5];
                for (int j = 0; j < (isKite ? 2 : 5); j++) {
                    float angle = j * angleB + (isKite ? angleA : 0);
                    float length = j % 2 == 0 ? lengthA : lengthB;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    Vector2 newPosition = (Vector2)tile.transform.position + length * direction;
                    Quaternion newRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                    positions[j] = newPosition;
                    rotations[j] = newRotation;
                }

                // Create the new tiles and add them to the list
                GameObject[] newTiles = new GameObject[isKite ? 2 : 5];
                for (int j = 0; j < (isKite ? 2 : 5); j++) {
                    GameObject newTile = Instantiate(isKite ? dartPrefab : kitePrefab, positions[j], rotations[j]);
                    nextTiles.Add(newTile);
                    newTiles[j] = newTile;
                }

                // Connect the new tiles together
                if (isKite) {
                    newTiles[0].GetComponent<ConnectTiles>().ConnectTo(tile, ConnectTiles.Side.Right);
                    newTiles[1].GetComponent<ConnectTiles>().ConnectTo(tile, ConnectTiles.Side.Left);
                } 
                else {
                    newTiles[0].GetComponent<ConnectTiles>().ConnectTo(newTiles[2], ConnectTiles.Side.Top);
                    newTiles[1].GetComponent<ConnectTiles>().ConnectTo(newTiles[3], ConnectTiles.Side.Top);
                    newTiles[2].GetComponent<ConnectTiles>().ConnectTo(tile, ConnectTiles.Side.Right);
                    newTiles[3].GetComponent<ConnectTiles>().ConnectTo(tile, ConnectTiles.Side.Left);
                    newTiles[4].GetComponent<ConnectTiles>().ConnectTo(newTiles[1], ConnectTiles.Side.Right);
                    }
                }
             // Replace the old list of tiles with the new list
        tiles = nextTiles;
        
        
    }

}

}


