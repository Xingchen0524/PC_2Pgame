using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PuzzleGame4 : MonoBehaviourPunCallbacks
{
    public List<Image> slots;  // 存放左側六個拼圖片格
    public List<Sprite> puzzlePieces; // 可供選擇的拼圖碎片
    public List<Sprite> targetPieces; // 目標拼圖的各個碎片
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;
    public GameObject dialogBox5;
    public GameObject blackend;

    public GameObject No1, No2, No3, No4;
    private int selectedSlotIndex = 0;  // 當前選擇的拼圖片格索引
    private int selectedPieceIndex = 0; // 當前選擇的圖片索引

    private int currentStage = 4; // 當前關卡（1~4）
    private bool isCompleted1 = false, isCompleted2 = false; // 記錄兩名玩家是否完成拼圖

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {

            }
            else
            {
                No1.SetActive(false);
                No2.SetActive(false);
                No3.SetActive(false);
                No4.SetActive(true);
            }
        }
        ShufflePuzzlePieces(); // 再打亂拼圖片
        HighlightSelectedSlot(); // 顯示強調選擇框

        if (slots == null || slots.Count == 0)
        {
            Debug.LogError("slots 尚未初始化或為空！");
            return;
        }
        if (targetPieces == null || targetPieces.Count == 0)
        {
            Debug.LogError("targetPieces 尚未初始化或為空！");
            return;
        }
    }

    void Update()
    {

        HandleInput();


    }
    //--------------------------------------移動控制選項(已完成)------------------------------------
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

    //--------------------------------------上下移動+Call顯示強化(已完成)------------------------------------
    void MoveSelection(int direction)
    {
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + direction, 0, slots.Count - 1);
        HighlightSelectedSlot();
    }
    //--------------------------------------選項顯示強化(已完成)------------------------------------
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
    //--------------------------------------左右選項移動(已完成)------------------------------------

    void ChangePiece(int direction)
    {
        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("puzzlePieces 為 null 或數量為 0！");
            return;
        }

        // 找到 slots[selectedSlotIndex] 當前圖片在 puzzlePieces 中的索引
        selectedPieceIndex = puzzlePieces.IndexOf(slots[selectedSlotIndex].sprite);

        if (selectedPieceIndex == -1)
        {
            Debug.LogError($"找不到當前 slots[{selectedSlotIndex}] 的 sprite 在 puzzlePieces 內！");
            return;
        }

        // 根據方向變更索引
        selectedPieceIndex = (selectedPieceIndex + direction + puzzlePieces.Count) % puzzlePieces.Count;

        // 更新 UI Image
        slots[selectedSlotIndex].sprite = puzzlePieces[selectedPieceIndex];

        // 更新 SpriteRenderer
        SpriteRenderer spriteRenderer = slots[selectedSlotIndex].GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = puzzlePieces[selectedPieceIndex];
            spriteRenderer.color = Color.white; // 確保不被透明度影響
        }

        Debug.Log($"變更 slot[{selectedSlotIndex}]，新圖片: {puzzlePieces[selectedPieceIndex].name}");

        if (slots[selectedSlotIndex].sprite == null)
        {
            Debug.LogError($"slots[{selectedSlotIndex}] 的 sprite 為 null，請確認拼圖片塊是否正確分配！");
            return;
        }
    }

    //--------------------------------------------------



    // 隨機打亂拼圖片庫並分配給 slots
    void ShufflePuzzlePieces()
    {

        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("puzzlePieces 為 null 或數量為 0，無法隨機打亂！");
            return;
        }

        // 使用 Fisher-Yates Shuffle 來確保不重複
        List<Sprite> shuffledPieces = new List<Sprite>(puzzlePieces);
        for (int i = 0; i < shuffledPieces.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledPieces.Count);
            (shuffledPieces[i], shuffledPieces[randomIndex]) = (shuffledPieces[randomIndex], shuffledPieces[i]);
        }

        // 分配打亂後的拼圖片塊給 slots
        for (int i = 0; i < slots.Count && i < shuffledPieces.Count; i++)
        {
            Image img = slots[i].GetComponent<Image>();
            SpriteRenderer spriteRenderer = slots[i].GetComponentInParent<SpriteRenderer>();  // 確保找到正確的 SpriteRenderer
            if (slots[i] == null)
            {
                Debug.LogError($"slots[{i}] 未綁定！");
                continue;
            }
            if (img != null)
            {
                img.sprite = shuffledPieces[i];
                spriteRenderer.sprite = shuffledPieces[i];
                Debug.Log($"更新 {slots[i].name} 的 Image 為 {shuffledPieces[i].name}");
                Debug.Log($"更新 {slots[i].name} 的 SpriteRenderer 為 {shuffledPieces[i].name}");
            }
            else
            {
                Debug.LogError($"slots[{i}] 找不到 Image 組件！");
            }
        }

        Debug.Log("拼圖已隨機打亂並分配給 slots");
    }
    //---------------------------------------------------------------

    void CheckPuzzleCompletion()
    {
        if (slots == null || slots.Count == 0 || targetPieces == null || targetPieces.Count == 0)
        {
            Debug.LogError("slots 或 targetPieces 尚未正確初始化！");
            return;
        }

        bool isComplete = true;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null || slots[i].sprite == null)
            {
                Debug.LogError($"slots[{i}] 為 null 或沒有 sprite！");
                isComplete = false;
                break;
            }

            if (i >= targetPieces.Count || targetPieces[i] == null)
            {
                Debug.LogError($"targetPieces[{i}] 為 null！");
                isComplete = false;
                break;
            }

            if (slots[i].sprite != targetPieces[i])
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            Debug.Log("拼圖完成！");
            ShowDialogBox(currentStage);
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {

                    { "dialogBox4", true },

                };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            AdvanceStage();
        }
        else
        {
            Debug.Log("拼圖尚未完成！");
        }
    }



    void ShowDialogBox(int stage)
    {
        switch (stage)
        {
            case 1: dialogBox.SetActive(true); break;
            case 2: dialogBox2.SetActive(true); break;
            case 3: dialogBox3.SetActive(true); break;
            case 4: dialogBox4.SetActive(true); break;
        }

    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)

    {
        // 檢查是否同步播放影片
        if (changedProps.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)changedProps["PlayVideo"];
            if (shouldPlayVideo)
            {
                PlayVideo();
            }
        }

        // 檢查並同步 dialogBox 狀態
        if (changedProps.ContainsKey("dialogBox"))
        {
            dialogBox.SetActive((bool)changedProps["dialogBox"]);
        }
        if (changedProps.ContainsKey("dialogBox2"))
        {
            dialogBox2.SetActive((bool)changedProps["dialogBox2"]);
        }
        if (changedProps.ContainsKey("dialogBox3"))
        {
            dialogBox3.SetActive((bool)changedProps["dialogBox3"]);
        }
        if (changedProps.ContainsKey("dialogBox4"))
        {
            dialogBox4.SetActive((bool)changedProps["dialogBox4"]);
        }

    }


    void AdvanceStage()
    {
        if ( currentStage == 4)
        {
            No3.SetActive(false);
            No4.SetActive(true);
            dialogBox5.SetActive(true);
            blackend.SetActive(true);
        }
    }


    void PlayVideo()
    {
        // 播放影片的程式邏輯
        Debug.Log("播放影片中...");
    }
}
