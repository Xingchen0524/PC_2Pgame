using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWithBorder : MonoBehaviour
{
    public SpriteRenderer borderRenderer; // 邊框的SpriteRenderer
    public Color borderColor = Color.black; // 邊框顏色（默認為黑色）

    void Start()
    {
        // 如果邊框Renderer存在，設置顏色
        if (borderRenderer != null)
        {
            borderRenderer.color = borderColor; // 設置邊框顏色
        }
        else
        {
            // 如果沒有設置邊框Renderer，則創建一個
            CreateBorder();
        }
    }

    // 動態創建邊框
    private void CreateBorder()
    {
        // 創建一個新的 GameObject，並添加 SpriteRenderer 用來顯示邊框
        GameObject border = new GameObject("Border");
        border.transform.SetParent(transform); // 把邊框設為當前格子的子物件
        border.transform.localPosition = Vector3.zero; // 邊框在格子中心顯示

        // 為邊框添加 SpriteRenderer 組件
        borderRenderer = border.AddComponent<SpriteRenderer>();

        // 設置邊框顏色
        borderRenderer.color = borderColor;

        // 使用您創建的邊框圖像或編輯器中的預設圖像
        // 例如，使用一個白色的矩形邊框（您可以根據需求替換成實際的邊框圖片）
        borderRenderer.sprite = Resources.Load<Sprite>("BorderSprite"); // 假設您已經將邊框圖像放在Resources文件夾中

        // 設置邊框的排序層，防止被其他物體遮擋
        borderRenderer.sortingOrder = 1;
    }

    // 設置邊框顏色的方法
    public void SetBorderColor(Color color)
    {
        if (borderRenderer != null)
        {
            borderRenderer.color = color; // 動態改變邊框顏色
        }
    }
}

