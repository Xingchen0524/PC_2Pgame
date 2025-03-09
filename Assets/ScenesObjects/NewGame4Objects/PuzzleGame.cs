using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PuzzleGame : MonoBehaviourPunCallbacks
{
    public List<Image> slots;  // 存放左側六個拼圖片格
    public List<Image> slots2;
    public List<Image> slots3;
    public List<Image> slots4;
    public List<Sprite> puzzlePieces; // 可供選擇的拼圖碎片
    public List<Sprite> puzzlePieces2;
    public List<Sprite> puzzlePieces3;
    public List<Sprite> puzzlePieces4;
    public List<Sprite> targetPieces; // 目標拼圖的各個碎片
    public List<Sprite> targetPieces2;
    public List<Sprite> targetPieces3;
    public List<Sprite> targetPieces4;
    public Sprite targetImage;  // 目標完整圖片
    public Sprite targetImage2;
    public Sprite targetImage3;
    public Sprite targetImage4;
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;

    private int selectedSlotIndex = 0;  // 當前選擇的拼圖片格索引
    private int selectedPieceIndex = 0; // 當前選擇的圖片索引

    private int currentStage = 1; // 當前關卡（1~4）
    private string playerRole;
    private bool isCompleted1 = false, isCompleted2 = false; // 記錄兩名玩家是否完成拼圖

    void Start()
    {
        //playerRole = PhotonNetwork.NickName; // 取得玩家名稱作為角色判斷
        playerRole = "妹妹";  // 測試用，實際上應該用 PhotonNetwork.NickName 來決定

        
        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("Start(): puzzlePieces 仍為 null 或空，請確認 LoadPuzzleSet() 是否正確執行！");
            return;
        }
        LoadPuzzleSet();  // 先加載對應的拼圖組合
        ShufflePuzzlePieces(); // 再打亂拼圖片
        HighlightSelectedSlot(); // 顯示強調選擇框


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
        // 確保 puzzlePieces 不為空
        puzzlePieces = GetCurrentPuzzlePieces();
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

            if (img != null)
            {
                img.sprite = shuffledPieces[i];               
                spriteRenderer.sprite = shuffledPieces[i];
                Debug.Log($"更新 {slots[i].name} 的 Image 為 {shuffledPieces[i].name}");
                Debug.Log($"更新 {slots[i].name} 的 SpriteRenderer 為 {shuffledPieces[i].name}");
            }
        }

        Debug.Log("拼圖已隨機打亂並分配給 slots");
    }

    void CheckPuzzleCompletion()
    {
        targetPieces = GetCurrentTargetPieces();
        Debug.Log("開始檢查拼圖是否完成...");

        // 檢查 slots 是否為空
        if (slots == null || slots.Count == 0)
        {
            Debug.LogError("slots 為 null 或數量為 0！");
            return;
        }
        
        // 檢查 targetPieces 是否為空
        if (targetPieces == null || targetPieces.Count == 0)
        {
            Debug.LogError("targetPieces 為 null 或數量為 0！");
            return;
        }

        bool isComplete = true;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].sprite != targetPieces[i])
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            Debug.Log("拼圖完成！");
            //photonView.RPC("NotifyCompletion", RpcTarget.All, playerRole, currentStage);
            //LoadNextPuzzleSet();  // 確保這個方法存在並正確執行
            ShowDialogBox(currentStage);
            if (dialogBox.activeSelf) // 確保只有當dialogBox3是活躍的時候才關閉它
            {
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "dialogBox", true },
            };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            if (dialogBox2.activeSelf) // 確保只有當dialogBox3是活躍的時候才關閉它
            {
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "dialogBox2", true },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            if (dialogBox3.activeSelf)
            {
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "dialogBox3", true },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            if (dialogBox4.activeSelf)
            {
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "dialogBox4", true },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
        }
        else
        {
            Debug.Log("拼圖尚未完成！");
        }

    }
    void LoadNextPuzzleSet()
    {
        currentStage++;  // 更新關卡

        if (currentStage > 4)  // 假設有 4 組拼圖
        {
            Debug.Log("所有拼圖都完成了！");
            return;
        }

        Debug.Log($"加載新的拼圖組合: Stage {currentStage}");

        // 根據新關卡加載對應的拼圖組合
        switch (currentStage)
        {
            case 2:
                puzzlePieces = puzzlePieces2;
                targetPieces = targetPieces2;
                targetImage = targetImage2;
                break;
            case 3:
                puzzlePieces = puzzlePieces3;
                targetPieces = targetPieces3;
                targetImage = targetImage3;
                break;
            case 4:
                puzzlePieces = puzzlePieces4;
                targetPieces = targetPieces4;
                targetImage = targetImage4;
                break;
        }


        if (targetPieces == null || puzzlePieces == null)
        {
            Debug.LogError("新的 targetPieces 或 puzzlePieces 加載失敗！");
            return;
        }

        // 更新 bg(1) 的 SpriteRenderer
        GameObject bg = GameObject.Find("bg(1)");
        if (bg != null)
        {
            SpriteRenderer bgSprite = bg.GetComponent<SpriteRenderer>();
            if (bgSprite != null) bgSprite.sprite = targetImage;
        }

        // 更新 slots 的拼圖片
        for (int i = 0; i < slots.Count && i < puzzlePieces.Count; i++)
        {
            slots[i].sprite = puzzlePieces[i];
            SpriteRenderer spriteRenderer = slots[i].GetComponentInParent<SpriteRenderer>();
            if (spriteRenderer != null) spriteRenderer.sprite = puzzlePieces[i]; // **確保 SpriteRenderer 也更新**
        }


        Debug.Log("新的拼圖組合加載完成！");
    }

    void ShowDialogBox(int stage)
    {


        // 根據關卡顯示對應的對話框
        switch (stage)
        {
            case 1:
                dialogBox.SetActive(true);
                photonView.RPC("ShowDialogBoxForBoth", RpcTarget.Others, stage);
                break;
            case 2:
                dialogBox2.SetActive(true);
                photonView.RPC("ShowDialogBoxForBoth", RpcTarget.Others, stage);
                break;
            case 3:
                dialogBox3.SetActive(true);
                photonView.RPC("ShowDialogBoxForBoth", RpcTarget.Others, stage);
                break;
            case 4:
                dialogBox4.SetActive(true);
                photonView.RPC("ShowDialogBoxForBoth", RpcTarget.Others, stage);
                break;
            default:
                Debug.LogWarning("未知的關卡，沒有對應的對話框！");
                break;
        }
    }
    [PunRPC]
    void ShowDialogBoxForBoth(int stage)
    {
        // 確保另一邊的玩家也顯示對話框
        ShowDialogBox(stage);
    }

    [PunRPC]
    void NotifyCompletion(string role, int stage)
    {
        Debug.Log(role + " 完成第 " + stage + " 組拼圖！");

        if (role == "妹妹") isCompleted1 = true;
        else if (role == "姐姐") isCompleted2 = true;

        // 根據當前關卡顯示對應的對話框
        if (playerRole == "妹妹" || playerRole == "姐姐")
        {
            ShowDialogBox(stage);  // 顯示對話框
        }

        // 如果雙方都完成，則播放影片
        if (isCompleted1 && isCompleted2)
        {
            Debug.Log("所有拼圖都完成，開始播放影片...");
            //PlayEndingVideo();
        }
    }

    void AdvanceStage()
    {

            if (playerRole == "妹妹")
            {
                if (currentStage == 1)
                    currentStage = 3;
            }
            else if (playerRole == "姐姐")
            {
                if (currentStage == 2)
                    currentStage = 4;
            }

            Debug.Log($"關卡切換至: {currentStage}");

            LoadPuzzleSet();  // 加載新的拼圖組合
            ShufflePuzzlePieces();  // 重新打亂拼圖並更新 slots
        
    }

    void LoadPuzzleSet()
    {
        Sprite newTargetImage = null;

        if (playerRole == "妹妹")
            newTargetImage = (currentStage == 1) ? targetImage : targetImage3;
        else if (playerRole == "姐姐")
            newTargetImage = (currentStage == 2) ? targetImage2 : targetImage4;

        if (newTargetImage == null)
        {
            Debug.LogError($"LoadPuzzleSet(): 無法找到對應的 newTargetImage！playerRole: {playerRole}, currentStage: {currentStage}");
            return;
        }

        GameObject bgObject = GameObject.Find("bg(1)");
        if (bgObject == null)
        {
            Debug.LogError("LoadPuzzleSet(): 找不到 bg (1) 物件！");
            return;
        }

        SpriteRenderer bgSpriteRenderer = bgObject.GetComponent<SpriteRenderer>();
        if (bgSpriteRenderer == null)
        {
            Debug.LogError("LoadPuzzleSet(): bg (1) 沒有 SpriteRenderer 組件！");
            return;
        }

        bgSpriteRenderer.sprite = newTargetImage;
        Debug.Log($"bg (1) 的圖片已更新為: {newTargetImage.name}");
    }

    List<Sprite> GetCurrentPuzzlePieces()
    {
        if (playerRole == "妹妹")
        {
            return (currentStage == 1) ? puzzlePieces :
                   (currentStage == 3) ? puzzlePieces3 : null;
        }
        else // "姐姐"
        {
            return (currentStage == 2) ? puzzlePieces2 :
                   (currentStage == 4) ? puzzlePieces4 : null;
        }
    }

    List<Sprite> GetCurrentTargetPieces()
    {
        if (playerRole == "妹妹") return (currentStage == 1) ? targetPieces : targetPieces3;
        else return (currentStage == 2) ? targetPieces2 : targetPieces4;
    }

    void PlayVideo()
    {
        // 播放影片的程式邏輯
        Debug.Log("播放影片中...");
    }
}

