using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGame : MonoBehaviour
{
    public List<Image> slots;  // 存放左側六個拼圖片格
    public List<Sprite> puzzlePieces; // 可供選擇的拼圖碎片
    public List<Sprite> targetPieces; // 目標拼圖的各個碎片
    public Sprite targetImage;  // 目標完整圖片

    private int selectedSlotIndex = 0;  // 當前選擇的拼圖片格索引
    private int selectedPieceIndex = 0; // 當前選擇的圖片索引

    void Start()
    {
        ShufflePuzzlePieces();// 隨機打亂拼圖片
        HighlightSelectedSlot();
    }

    void Update()
    {
        HandleInput();


    }

    void HandleInput()
    {
        // 移動選擇的拼圖片格
        if (Input.GetKeyDown(KeyCode.W))
            MoveSelection(-1);
        if (Input.GetKeyDown(KeyCode.S))
            MoveSelection(1);

        // 切換拼圖片
        if (Input.GetKeyDown(KeyCode.A))
            ChangePiece(-1);
        if (Input.GetKeyDown(KeyCode.D))
            ChangePiece(1);

        // 確認是否拼圖正確
        if (Input.GetKeyDown(KeyCode.Return))
            CheckPuzzleCompletion();
    }

    void MoveSelection(int direction)
    {
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + direction, 0, slots.Count - 1);
        HighlightSelectedSlot();
    }

    void ChangePiece(int direction)
    {
        selectedPieceIndex = (selectedPieceIndex + direction + puzzlePieces.Count) % puzzlePieces.Count;

        // 更新 UI Image
        slots[selectedSlotIndex].sprite = puzzlePieces[selectedPieceIndex];

        // 嘗試找到對應的 SpriteRenderer 並同步修改
        SpriteRenderer spriteRenderer = slots[selectedSlotIndex].GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = puzzlePieces[selectedPieceIndex];  // 更新 SpriteRenderer
            spriteRenderer.color = Color.white; // 確保不被透明度影響
        }

        // Debug 確認更改
        Debug.Log($"已更改 slot[{selectedSlotIndex}]，Sprite: {puzzlePieces[selectedPieceIndex].name}");
    }


    void HighlightSelectedSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Transform outline = slots[i].transform.Find("Highlight"); // 找到子物件 Outline
            if (outline != null)
                outline.gameObject.SetActive(i == selectedSlotIndex);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            Transform outline = slots[i].transform.Find("Highlight1"); // 找到子物件 Outline1
            if (outline != null)
                outline.gameObject.SetActive(i == selectedSlotIndex);
        }
    }

    // 隨機打亂拼圖片庫並分配給 slots
    void ShufflePuzzlePieces()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image img = slots[i].GetComponent<Image>();
            SpriteRenderer spriteRenderer = slots[i].GetComponent<SpriteRenderer>();

            // 隨機選擇一張圖片
            Sprite randomSprite = puzzlePieces[Random.Range(0, puzzlePieces.Count)];

            if (img != null)
                img.sprite = randomSprite;

            if (spriteRenderer != null)
                spriteRenderer.sprite = randomSprite;
        }

    }

        void CheckPuzzleCompletion()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].sprite != targetPieces[i])
            {
                Debug.Log("拼圖還沒完成！");
                return;
            }
        }
        Debug.Log("拼圖完成！");
    }
}

