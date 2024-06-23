using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] tilePrefabs;
    public float tileSize = 1.0f;  // Size of each tile in Unity units

    public GameObject[,] allTiles;
    public bool isSwapping = false;

    void Start()
    {
        allTiles = new GameObject[width, height];
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

    public void FindMatches()
    {
        // Horizontal matches
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject currentTile = allTiles[x, y];
                if (currentTile != null)
                {
                    if (x > 1 && allTiles[x - 1, y] != null && allTiles[x - 2, y] != null)
                    {
                        if (allTiles[x - 1, y].tag == currentTile.tag && allTiles[x - 2, y].tag == currentTile.tag)
                        {
                            // Match found
                            Destroy(allTiles[x, y]);
                            Destroy(allTiles[x - 1, y]);
                            Destroy(allTiles[x - 2, y]);
                        }
                    }
                    if (y > 1 && allTiles[x, y - 1] != null && allTiles[x, y - 2] != null)
                    {
                        if (allTiles[x, y - 1].tag == currentTile.tag && allTiles[x, y - 2].tag == currentTile.tag)
                        {
                            // Match found
                            Destroy(allTiles[x, y]);
                            Destroy(allTiles[x, y - 1]);
                            Destroy(allTiles[x, y - 2]);
                        }
                    }
                }
            }
        }
    }

    public void RefillBoard()
    {
        StartCoroutine(FillBoard());
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

        // Check for matches and revert if no match is found
        // (Add your match checking logic here)

        isSwapping = false;
    }


}
