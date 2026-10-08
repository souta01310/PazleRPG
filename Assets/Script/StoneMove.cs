using System.Threading;
using UnityEngine;


public class StoneMove : MonoBehaviour
{
    private GameTile gameTileCS;
    public int column;
    public int row;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    private Vector2 distance;
    //ó◊ÇÃêŒÇå©ÇÈ
    private GameObject neighborStone;
    //3Ç¬à»è„ï¿ÇÒÇ≈Ç¢ÇÈÇ∆Ç´Ç…ímÇÁÇπÇÈóp
    public bool isMacching;
    public Vector2 mypreviousPos;

    void Start()
    {
        gameTileCS = FindObjectOfType<GameTile>();
        column = (int)transform.position.x;
        row = (int)transform.position.y;
        mypreviousPos = new Vector2(column, row);
    }

    private void OnMouseDown()
    {
        fingerDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        fingerUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        distance = fingerUp - fingerDown;
        moveStone();
    }

    void moveStone()
    {
        if (distance.x >= 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (column < 4)
            {
                neighborStone = gameTileCS.tileStone[column + 1, row];
                neighborStone.GetComponent<StoneMove>().column -= 1;
                column += 1;
            }
        }
        if(distance.x < 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.y)){
            if(column > 0)
            {
                neighborStone = gameTileCS.tileStone[column - 1, row];

                neighborStone.GetComponent<StoneMove>().column += 1;
                column -= 1;
            }
        }
        if (distance.y >= 0 && Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            if (row < 6)
            {
                neighborStone = gameTileCS.tileStone[column, row + 1];
                neighborStone.GetComponent<StoneMove>().row -= 1;
                row += 1;
            }
        }
        if(distance.y < 0&& Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            if(row > 0)
            {
                neighborStone = gameTileCS.tileStone[column, row - 1];
                neighborStone.GetComponent<StoneMove>().row += 1;
                row -= 1;
            }
        }
    }
    public void SetStoneToArray()
    {
        gameTileCS.tileStone[column, row] = gameObject;
    }
    private void Update()
    {
        if(transform.position.x!=column || transform.position.y != row)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(column, row), 0.3f);
            Vector2 dif = (Vector2)transform.position - new Vector2(column, row);
            if (Mathf.Abs(dif.magnitude) < 0.1f)
            {
                transform.position = new Vector2(column, row);
                SetStoneToArray();
            }
            else if (row > 0 && gameTileCS.tileStone[column,row-1]==null)
            {
                FallStone();
            }
        }
    }
    void FallStone()
    {
        gameTileCS.tileStone[column, row]= null;
        row -= 1;
    }
}