using UnityEngine;

public class Tile : MonoBehaviour
{
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isSwapping = false;
    public float swipeAngle = 0;
    public float swipeResistance = 1.0f;

    public GameObject[,] allTiles;  // Change to public

    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        column = targetX;
        row = targetY;
    }

    void Update()
    {
        if (isSwapping)
        {
            MoveTile();
        }
    }

    private void OnMouseDown()
    {
        if (!board.isSwapping)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"Tile clicked: ({column}, {row})");
        }
    }

    private void OnMouseUp()
    {
        if (!board.isSwapping)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log($"Swipe angle: {swipeAngle}");
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            // Right swipe
            Debug.Log($"Swiping right from ({column}, {row}) to ({column + 1}, {row})");
            StartCoroutine(board.SwapTiles(this, board.allTiles[column + 1, row].GetComponent<Tile>()));
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // Up swipe
            Debug.Log($"Swiping up from ({column}, {row}) to ({column}, {row + 1})");
            StartCoroutine(board.SwapTiles(this, board.allTiles[column, row + 1].GetComponent<Tile>()));
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Left swipe
            Debug.Log($"Swiping left from ({column}, {row}) to ({column - 1}, {row})");
            StartCoroutine(board.SwapTiles(this, board.allTiles[column - 1, row].GetComponent<Tile>()));
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down swipe
            Debug.Log($"Swiping down from ({column}, {row}) to ({column}, {row - 1})");
            StartCoroutine(board.SwapTiles(this, board.allTiles[column, row - 1].GetComponent<Tile>()));
        }
    }

    void MoveTile()
    {
        targetX = column;
        targetY = row;
        Vector2 tempPosition = new Vector2(targetX, targetY);
        transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);

        if (Vector2.Distance(transform.position, tempPosition) < 0.1f)
        {
            transform.position = tempPosition;
            board.isSwapping = false;
            isSwapping = false;
        }
    }

    
}
