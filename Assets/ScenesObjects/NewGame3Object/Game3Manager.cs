using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro; // 用來顯示倒數
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class Game3Manager : MonoBehaviourPunCallbacks
{
    public GameObject dialogBox1, dialogBox2; // 妹妹 & 姐姐的對話框
    public TextMeshProUGUI CountdownText; // 倒數顯示

    private bool isSisterReady = false;
    private bool isYoungerSisterReady = false;
    private float startTime;
    private bool isCountingDown = false;
    private bool gameStarted = false;

    public GameObject LineQTE1, LineQTE2, LineQTE3, LineQTE4, LineQTE5, LineQTE6, LineQTE7, LineQTE8, LineQTE9, LineQTE10, LineQTE11, LineQTE12, LineQTE13, LineQTE14, LineQTE15, LineQTE16; // QTE 物件

    private void Start()
    {
        Debug.Log("場景重載，變數初始化！");
        isSisterReady = false;
        isYoungerSisterReady = false;
        isCountingDown = false;
        gameStarted = false;

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
            {
                string role = PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString();

                if (role == "姐姐")
                {
                    isSisterReady = true;
                    dialogBox2.SetActive(true);
                    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "SisterReady", true },
                        { "dialogBox2", true }
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                }
                else if (role == "妹妹")
                {
                    isYoungerSisterReady = true;
                    dialogBox1.SetActive(true);
                    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "YoungerSisterReady", true },
                        { "dialogBox1", true }
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                }

                // 設定玩家的準備狀態
                ExitGames.Client.Photon.Hashtable readyProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "Ready", true }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(readyProperties);

                // 檢查是否所有玩家都準備好，並更新房間狀態
                if (AllPlayersReady())
                {
                    Debug.Log("所有玩家都準備好了！");
                    ExitGames.Client.Photon.Hashtable gameReadyProperties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "GameReady", true }
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(gameReadyProperties);
                }
            }
        }
        // 檢查兩人是否都準備好了
        if (isSisterReady && isYoungerSisterReady && !isCountingDown)
        {
            Debug.Log("開始倒數！");
            StartCoroutine(StartCountdown());
            isCountingDown = true;
        }

       
    }

    bool AllPlayersReady()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("Ready") || !(bool)player.CustomProperties["Ready"])
            {
                return false;
            }
        }
        return true;
    }

    void StartGame()
    {
        startTime = (float)PhotonNetwork.Time; // 紀錄開始時間
        gameStarted = true;

        // 啟動協程來管理 QTE 出現時間
        StartCoroutine(QTEManager());
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // 更新其他玩家的準備狀態
        if (changedProps.ContainsKey("SisterReady"))
        {
            isSisterReady = (bool)changedProps["SisterReady"];
        }
        if (changedProps.ContainsKey("YoungerSisterReady"))
        {
            isYoungerSisterReady = (bool)changedProps["YoungerSisterReady"];
        }

        // 更新 dialogBox 狀態
        if (changedProps.ContainsKey("dialogBox1"))
        {
            dialogBox1.SetActive((bool)changedProps["dialogBox1"]);
        }
        if (changedProps.ContainsKey("dialogBox2"))
        {
            dialogBox2.SetActive((bool)changedProps["dialogBox2"]);
        }


        // 檢查是否所有玩家都準備好了
        if (isSisterReady && isYoungerSisterReady && !isCountingDown)
        {
            StartCoroutine(StartCountdown());
            isCountingDown = true;
        }

        // 如果遊戲已經準備好，則開始倒數
        if (changedProps.ContainsKey("GameReady") && (bool)changedProps["GameReady"] && !isCountingDown)
        {
            StartCoroutine(StartCountdown());
            isCountingDown = true;
        }

        
    }

    IEnumerator StartCountdown()
    {
        float countdown = 3;
        while (countdown > 0)
        {
            CountdownText.text = countdown.ToString();
            Debug.Log("倒數時間：" + countdown + " 秒");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        CountdownText.text = "開始！";
        yield return new WaitForSeconds(1f);
        CountdownText.gameObject.SetActive(false);

        Debug.Log("倒數結束，開始遊戲");
        StartGame();
        dialogBox1.SetActive(false);
        dialogBox2.SetActive(false);
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "dialogBox1", false },
                        { "dialogBox2", false },
                        {"ImageGo",true}
                    };
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

    IEnumerator QTEManager()
    {
        float elapsedTime = 0f;
        Debug.Log($"QTE 計時開始: {elapsedTime} 秒");

        yield return new WaitForSeconds(4); // 3 秒後顯示第一個 QTE
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第一個 QTE");
        LineQTE1.SetActive(true);
        /*
        yield return new WaitForSeconds(4); // 7 秒時顯示第二個 QTE
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第二個 QTE");
        LineQTE2.SetActive(true);

        yield return new WaitForSeconds(4); // 11 秒時顯示第三個 QTE
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第三個 QTE");
        LineQTE3.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第四個 QTE");
        LineQTE4.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第五個 QTE");
        LineQTE5.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第六個 QTE");
        LineQTE6.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第七個 QTE");
        LineQTE7.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第八個 QTE");
        LineQTE8.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第九個 QTE");
        LineQTE9.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十個 QTE");
        LineQTE10.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十一個 QTE");
        LineQTE11.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十二個 QTE");
        LineQTE12.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十三個 QTE");
        LineQTE13.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十四個 QTE");
        LineQTE14.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十五個 QTE");
        LineQTE15.SetActive(true);

        yield return new WaitForSeconds(4);
        elapsedTime += 4;
        Debug.Log($"[{elapsedTime}s] 顯示第十六個 QTE");
        LineQTE16.SetActive(true);
        */
    }
}
