using UnityEngine;
using UnityEngine.InputSystem; // 新しいインプットシステムを使うために必要

public class StoneMove : MonoBehaviour
{
    private GameTile gameTileCS;
    public int column;
    public int row;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    private Vector2 distance;

    private GameObject neighborStone;
    public bool isMatching;
    public Vector2 mypreviousPos;

    // ドラッグ中かを管理するフラグ
    private bool isDragging = false;
    public bool isMoving;

    void Start()
    {
        gameTileCS = FindObjectOfType<GameTile>();
        if (gameTileCS == null)
        {
            Debug.LogError("GameTileが見つかりません！");
            return;
        }

        column = Mathf.RoundToInt(transform.position.x);
        row = Mathf.RoundToInt(transform.position.y);
        mypreviousPos = new Vector2(column, row);
        SetStoneToArray();
    }

    void Update()
    {
        if (gameTileCS == null) return;

        // 新しいインプットシステムのポインタ（マウスまたはタッチ）を取得
        if (Pointer.current != null)
        {
            // 1. タッチ/クリックし始めた瞬間
            if (Pointer.current.press.wasPressedThisFrame)
            {
                Vector2 screenPos = Pointer.current.position.ReadValue();
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                // レイキャストで「自分自身の石」がタップされたか判定
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    fingerDown = worldPos;
                    isDragging = true;
                    Debug.Log("ダウン (New Input System)");
                }
            }

            // 2. タッチ/クリックを離した瞬間
            if (Pointer.current.press.wasReleasedThisFrame && isDragging)
            {
                Vector2 screenPos = Pointer.current.position.ReadValue();
                fingerUp = Camera.main.ScreenToWorldPoint(screenPos);
                distance = fingerUp - fingerDown;
                isDragging = false;
                Debug.Log("fingerUp (New Input System)");
                moveStone();
            }
        }

        // 石の移動・落下アニメーション処理
        if (transform.position.x != column || transform.position.y != row)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(column, row), 0.3f);

            Vector2 dif = (Vector2)transform.position - new Vector2(column, row);
            if (Mathf.Abs(dif.magnitude) < 0.01f)
            {
                transform.position = new Vector2(column, row);
                SetStoneToArray();
            }
        }
        else if (row > 0 && gameTileCS.tileStone[column, row - 1] == null)
        {
            FallStone();
        }
    }

    void moveStone()
    {
        int targetColumn = column;
        int targetRow = row;

        // スワイプ方向の判定
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (distance.x >= 0) targetColumn = column + 1; // 右
            else targetColumn = column - 1;                // 左
        }
        else
        {
            if (distance.y >= 0) targetRow = row + 1;       // 上
            else targetRow = row - 1;                      // 下
        }

        // 移動先の範囲チェック
        if (targetColumn >= 0 && targetColumn <= 4 && targetRow >= 0 && targetRow <= 6)
        {
            neighborStone = gameTileCS.tileStone[targetColumn, targetRow];

            if (neighborStone != null)
            {
                StoneMove neighborScript = neighborStone.GetComponent<StoneMove>();

                // 配列と座標の入れ替え
                gameTileCS.tileStone[column, row] = neighborStone;
                gameTileCS.tileStone[targetColumn, targetRow] = gameObject;

                int tempCol = column;
                int tempRow = row;

                column = targetColumn;
                row = targetRow;

                neighborScript.column = tempCol;
                neighborScript.row = tempRow;
            }
        }
        Invoke("DoCheckMatching",0.5f);
    }
    void DoCheckMatching()
    {
        gameTileCS.CheckMatching();
    }

    public void SetStoneToArray()
    {
        if (gameTileCS != null)
        {
            gameTileCS.tileStone[column, row] = gameObject;
        }
    }

    void FallStone()
    {
        gameTileCS.tileStone[column, row] = null;
        row -= 1;
    }
    public void BackToPreviouspos()
    {
        column = (int)mypreviousPos.x;
        row = (int)mypreviousPos.y;
    }
    private void OnMouseUp()
    {
        if (isMoving)
        {
            return;
        }
    }

}