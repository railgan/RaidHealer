using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    public float swipeAngle = 0;
    public int column;
    public int row;

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            // Right swipe
            tempPosition = transform.position;
            transform.position = new Vector2(column + 1, row);
            board.allTiles[column + 1, row].transform.position = tempPosition;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // Up swipe
            tempPosition = transform.position;
            transform.position = new Vector2(column, row + 1);
            board.allTiles[column, row + 1].transform.position = tempPosition;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Left swipe
            tempPosition = transform.position;
            transform.position = new Vector2(column - 1, row);
            board.allTiles[column - 1, row].transform.position = tempPosition;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down swipe
            tempPosition = transform.position;
            transform.position = new Vector2(column, row - 1);
            board.allTiles[column, row - 1].transform.position = tempPosition;
            row -= 1;
        }
    }

}
