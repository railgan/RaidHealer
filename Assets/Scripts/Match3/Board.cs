using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] tilePrefabs;
    public float tileSize = 1.0f;  // Size of each tile in Unity units

    public GameObject[,] allTiles;  // Change to public
    public bool isSwapping = false;

    void Start()
    {
        allTiles = new GameObject[width, height];

        // Move the board to the bottom left of the camera
        MoveBoardToBottomLeft();

        SetUp();
    }

    void SetUp()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int tileToUse = Random.Range(0, tilePrefabs.Length);
                GameObject tile = Instantiate(tilePrefabs[tileToUse], new Vector3(x, y, 0), Quaternion.identity);

                // Scale the tile
                ScaleTile(tile);

                tile.transform.parent = this.transform;
                tile.name = "( " + x + ", " + y + " )";
                allTiles[x, y] = tile;
                tile.GetComponent<Tile>().column = x;
                tile.GetComponent<Tile>().row = y;
            }
        }
    }

    private IEnumerator FillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allTiles[x, y] == null)
                {
                    int tileToUse = Random.Range(0, tilePrefabs.Length);
                    GameObject tile = Instantiate(tilePrefabs[tileToUse], new Vector3(x, y + height, 0), Quaternion.identity);
                    tile.transform.parent = this.transform;
                    tile.name = "( " + x + ", " + y + " )";
                    allTiles[x, y] = tile;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    void RefillBoard()
    {
        // Iterate through each column from top to bottom
        for (int x = 0; x < width; x++)
        {
            // Iterate through each row from bottom to top
            for (int y = 0; y < height; y++)
            {
                // Check if the current position is empty (null)
                if (allTiles[x, y] == null)
                {
                    // Instantiate a new tile prefab at this position
                    int tileToUse = Random.Range(0, tilePrefabs.Length);
                    GameObject newTile = Instantiate(tilePrefabs[tileToUse], new Vector3(x, y, 0), Quaternion.identity);

                    // Scale the new tile (if needed)
                    ScaleTile(newTile);

                    // Set parent and name for organization (optional)
                    newTile.transform.parent = transform; // Assuming this script is attached to the board
                    newTile.name = $"({x}, {y})";

                    // Update the allTiles array with the new tile
                    allTiles[x, y] = newTile;

                    // Update the Tile component's column and row (if necessary)
                    newTile.GetComponent<Tile>().column = x;
                    newTile.GetComponent<Tile>().row = y;
                }
            }
        }
    }

    void ScaleTile(GameObject tile)
    {
        // Get the sprite renderer component
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();

        // Calculate the scaling factor
        float scaleX = tileSize / spriteRenderer.bounds.size.x;
        float scaleY = tileSize / spriteRenderer.bounds.size.y;

        // Apply the scaling factor
        tile.transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }

    void MoveBoardToBottomLeft()
    {
        // Get the camera's bottom-left corner in world coordinates
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));

        // Adjust the board's position
        this.transform.position = new Vector3(bottomLeft.x + tileSize / 2, bottomLeft.y + tileSize / 2, 0);
    }

    public IEnumerator SwapTiles(Tile tile1, Tile tile2)
    {
        isSwapping = true;
        tile1.isSwapping = true;
        tile2.isSwapping = true;

        // Swap the tiles in the grid
        int tempColumn = tile1.column;
        int tempRow = tile1.row;
        tile1.column = tile2.column;
        tile1.row = tile2.row;
        tile2.column = tempColumn;
        tile2.row = tempRow;

        // Swap the tiles in the allTiles array
        allTiles[tile1.column, tile1.row] = tile1.gameObject;
        allTiles[tile2.column, tile2.row] = tile2.gameObject;

        // Wait until the tiles have moved
        while (tile1.isSwapping || tile2.isSwapping)
        {
            yield return null;
        }

        // Check for matches
        yield return new WaitForSeconds(0.1f);
        CheckForMatches();

        isSwapping = false;
    }

    void CheckForMatches()
    {
        List<GameObject> tilesToDestroy = new List<GameObject>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allTiles[x, y] != null)
                {
                    Tile currentTile = allTiles[x, y].GetComponent<Tile>();

                    // Horizontal match check
                    if (x > 1 && allTiles[x - 1, y] != null && allTiles[x - 2, y] != null)
                    {
                        Tile leftTile1 = allTiles[x - 1, y].GetComponent<Tile>();
                        Tile leftTile2 = allTiles[x - 2, y].GetComponent<Tile>();

                        if (AreTilesSameTag(currentTile.gameObject, leftTile1.gameObject, leftTile2.gameObject))
                        {
                            tilesToDestroy.Add(allTiles[x, y]);
                            tilesToDestroy.Add(allTiles[x - 1, y]);
                            tilesToDestroy.Add(allTiles[x - 2, y]);
                        }
                    }

                    // Vertical match check
                    if (y > 1 && allTiles[x, y - 1] != null && allTiles[x, y - 2] != null)
                    {
                        Tile downTile1 = allTiles[x, y - 1].GetComponent<Tile>();
                        Tile downTile2 = allTiles[x, y - 2].GetComponent<Tile>();
                        if (AreTilesSameTag(currentTile.gameObject, downTile1.gameObject, downTile2.gameObject))
                        {
                            tilesToDestroy.Add(allTiles[x, y]);
                            tilesToDestroy.Add(allTiles[x, y - 1]);
                            tilesToDestroy.Add(allTiles[x, y - 2]);
                        }
                    }
                }
            }
        }
        
        if (tilesToDestroy.Count > 0)
        {
            foreach (GameObject tile in tilesToDestroy)
            {
                int x = (int)tile.transform.position.x;
                int y = (int)tile.transform.position.y;

                allTiles[x, y] = null;
                Destroy(tile);
            }
        }
        RefillBoard();
    }

    bool AreTilesSameTag(GameObject tile1, GameObject tile2, GameObject tile3)
    {
        string tag1 = tile1.tag;
        string tag2 = tile2.tag;
        string tag3 = tile3.tag;

        return tag1 == tag2 && tag2 == tag3;
    }
}

