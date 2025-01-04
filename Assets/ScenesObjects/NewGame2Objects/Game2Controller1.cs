using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game2Controller1 : MonoBehaviourPun
{
    public float speed = 5f; // 移動速度
    public GameObject plate; // 盤子圖片（需要在 Inspector 指定）
    private Vector2 movement;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private PolygonCollider2D plateCollider; // 盤子的 Polygon Collider
    private Collider2D playerCollider;       // 玩家物件的 Collider

    void Start()
    {
        // 記錄起始位置
        startPosition = transform.position;

        // 獲取 Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // 獲取盤子的 Polygon Collider2D
        if (plate != null)
        {
            plateCollider = plate.GetComponent<PolygonCollider2D>();
            if (plateCollider == null)
            {
                Debug.LogError("盤子圖片必須有 Polygon Collider2D！");
            }
        }
        else
        {
            Debug.LogError("未指定盤子圖片！");
        }

        // 獲取玩家的 Collider2D
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("玩家圖片必須有 Collider2D！");
        }
    }

    void Update()
    {
        // 取得輸入
        movement.x = Input.GetAxis("Horizontal"); // A/D 或 ←/→
        movement.y = Input.GetAxis("Vertical");   // W/S 或 ↑/↓

        // 計算新的位置
        Vector2 newPosition = rb.position + movement * speed * Time.deltaTime;

        // 檢查是否超出盤子範圍
        if (IsWithinPlate(newPosition))
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.Log("移動被限制，玩家圖片將超出盤子範圍！");
            transform.position = startPosition;
        }
    }

    private bool IsWithinPlate(Vector2 position)
    {
        if (plateCollider == null || playerCollider == null)
            return false;

        // 計算玩家圖片的所有邊界點
        Bounds playerBounds = playerCollider.bounds;
        Vector2[] playerCorners = new Vector2[4]
        {
            new Vector2(playerBounds.min.x, playerBounds.min.y), // 左下角
            new Vector2(playerBounds.min.x, playerBounds.max.y), // 左上角
            new Vector2(playerBounds.max.x, playerBounds.min.y), // 右下角
            new Vector2(playerBounds.max.x, playerBounds.max.y)  // 右上角
        };

        // Debug 訊息：顯示玩家邊界點位置
        //Debug.Log("玩家邊界點：");
        //foreach (Vector2 corner in playerCorners)
        //{
            //Debug.Log(corner);
        //}

        // 檢查每個邊界點是否都在盤子的 Collider 範圍內
        foreach (Vector2 corner in playerCorners)
        {
            if (!plateCollider.OverlapPoint(corner))
            {
                Debug.Log($"邊界點 {corner} 超出盤子範圍！");
                return false; // 任一角超出盤子範圍，則返回 false
            }
        }

        return true; // 所有角都在盤子範圍內
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            // 碰撞到邊界時重置位置
            Debug.Log("碰到邊界！");
            transform.position = startPosition;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("遊戲完成！");
            // 在此添加完成遊戲的邏輯
        }
    }

}
