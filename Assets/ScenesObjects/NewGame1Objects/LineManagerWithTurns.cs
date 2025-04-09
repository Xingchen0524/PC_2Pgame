using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // 用於顯示對話框
using UnityEngine.Video; // 用於播放影片
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.Tilemaps;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class LineManagerWithTurns : MonoBehaviourPunCallbacks
{
    //1
    //2   
    //3

    public LineRenderer lineRenderer;
    public List<Vector3> points = new List<Vector3>();
    public HashSet<GameObject> visitedObjects = new HashSet<GameObject>();  // 存儲已經經過的空物件
    public bool isLineDrawing = false;
    public LayerMask clickableObjectsLayer;  // 可點擊的空物件層
    public GameObject lastObjectUnderMouse = null;
    public GameObject startObject = null;  // 用來追蹤起始物件

    public string FirstCircleName, SecondCircleName;
    public List<GameObject> pointsObj;
    public List<GameObject> BlueSavepointsObj;
    public List<GameObject> YellowSavepointsObj;
    public List<GameObject> lightBlueSavepointsObj;
    public List<GameObject> OrangeSavepointsObj;
    public List<Vector3> BlueSavePoints;
    public List<Vector3> YellowSavePoints;
    public List<Vector3> lightBlueSavePoints;
    public List<Vector3> OrangeSavePoints;

    public GameObject dialogBox; // 用於顯示對話框
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;
    public GameObject dialogBoxmistake;

    public VideoPlayer videoPlayer; // 用於播放影片

    public CanvasGroup dialogCanvasGroup; // 用於控制對話框的漸隱效果
    private bool isDialogActive = false; // 判斷對話框是否正在顯示
    private bool hasPlayed = false; // 用來追蹤影片是否已經播放過
    private bool hasDrawnLine = false;
    private bool isFadingOut = false;

    public GameObject bg;          // 名為 "bg" 的背景物件
    public GameObject bg2;
    private float timer = 5f;  // 用來追蹤剩餘時間
    public Volume volume;  // 預設的 Volume，包含黑白效果

    public GameObject NewScenes;

    void Start()
    {
        // 初始化 LineRenderer
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;//起始寬度
        lineRenderer.endWidth = 0.2f;//結尾寬度
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
        lineRenderer.numCornerVertices = 90; //轉角平滑
        Destroy(GameObject.Find("MusicManager"));//避免背景音樂重複。

        //雙人畫面判定
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {
                EnableBlackAndWhiteEffect();

            }
            else
            {
                DisableBlackAndWhiteEffect();

            }
        }

    }
    // 關閉輸入功能
    void DisableInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // 或可更進一步禁用 InputManager 綁定的輸入事件
    }
    void EnableInput()
    {
        Cursor.lockState = CursorLockMode.None; // 解鎖鼠標
        Cursor.visible = true; // 顯示鼠標
                               // 可以根據需求開啟其他的輸入操作
    }
    //雙人畫面判定
    void EnableBlackAndWhiteEffect()
    {
        if (volume != null)
        {
            volume.enabled = true;
        }
    }
    //雙人畫面判定
    void DisableBlackAndWhiteEffect()
    {
        if (volume != null)
        {
            volume.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // 按下N鍵
        {
            Debug.Log("嘗試按下N 鍵");     
        }
        if (Input.GetKeyDown(KeyCode.N)) // 按下N鍵
        {
            Debug.Log("N 鍵按下且 hasPlayed 為 true，嘗試切換場景");

            // 發送事件，告訴所有玩家場景即將切換
            PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

            // 延遲一小段時間後再進行場景切換，以確保所有玩家都收到切換信號
            StartCoroutine(DelayedSceneChange());
        }
        /*
        // 只允許角色為「妹妹」的玩家處理輸入
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString();
            if (role != "妹妹")
            {
                return;
            }
        }
        */
        // 鼠標按下，選擇起始物件
        if (Input.GetMouseButtonDown(0))
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
            {
                string role = PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString();
                if (role != "妹妹")
                {
                    return;
                }
            }
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // 檢測鼠標點擊的空物件
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickableObjectsLayer);

            if (hit.collider.tag == "Circle")
            {

                if (hit.collider != null && !visitedObjects.Contains(hit.collider.gameObject))
                {
                    // 設定起始物件
                    startObject = hit.collider.gameObject;
                    //抓取物件的顏色名稱
                    if (hit.collider.gameObject.name.Contains('1'))
                    {
                        FirstCircleName = hit.collider.gameObject.name.Split('1')[0];
                        SecondCircleName = FirstCircleName + "2";
                    }
                    if (hit.collider.gameObject.name.Contains('2'))
                    {
                        FirstCircleName = hit.collider.gameObject.name.Split('2')[0];
                        SecondCircleName = FirstCircleName + "1";
                    }
                    // 設定線條顏色
                    SetLineColorBasedOnName(hit.collider.gameObject);
                    visitedObjects.Clear(); // 清除之前經過的物件
                    visitedObjects.Add(hit.collider.gameObject); // 記錄起始物件

                    points.Add(hit.collider.transform.position); // 添加起始點
                    pointsObj.Add(hit.collider.gameObject);

                    pointsObj[0].GetComponent<LineRenderer>().positionCount = 1;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(0, hit.collider.transform.position);
                    isLineDrawing = true;
                }

                if (hit.collider != null)
                {
                    // 點擊檢測成功
                    // Debug.Log("Hit: " + hit.collider.gameObject.name);
                }
            }
        }

        // 鼠標按住時，線條跟隨鼠標
        if (isLineDrawing)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            // 檢測鼠標下是否有新的空物件
            RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag != "Circle" && hit.collider.tag != "Finished" || hit.collider.name == SecondCircleName)
            {
                if (hit.collider != null && hit.collider.gameObject != lastObjectUnderMouse && !visitedObjects.Contains(hit.collider.gameObject))
                {
                    lastObjectUnderMouse = hit.collider.gameObject;

                    // 如果經過了新的物件，添加拐點
                    Vector3 hitPosition = hit.collider.transform.position;
                    if (points.Count > 0)
                    {
                        // 檢查是否轉彎
                        Vector3 lastPoint = points[points.Count - 1];
                        if (lastPoint.x != hitPosition.x && lastPoint.y != hitPosition.y)
                        {
                            // 添加拐角点（水平或垂直移动）
                            Vector3 turnPoint = new Vector3(hitPosition.x, lastPoint.y, 0);
                            points.Add(turnPoint);
                            pointsObj.Add(hit.collider.gameObject);
                            pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count;
                            pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count - 1, turnPoint);                           
                        }
                    }

                    // 添加新的空物件位置
                    points.Add(hitPosition);
                    pointsObj.Add(hit.collider.gameObject);
                    visitedObjects.Add(hit.collider.gameObject);  // 標記這個空物件已經經過                                                                 
                    pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count - 1, hitPosition);


                    // 同步畫線過程到所有玩家
                    if (GetComponent<LineRenderer>().positionCount > 0)
                    {
                        if (GetComponent<RPCOrange>())
                        {
                            GetComponent<RPCOrange>().SendData();
                        }
                        if (GetComponent<RPCYellow>())
                        {
                            GetComponent<RPCYellow>().SendData();
                        }
                        if (GetComponent<RPCBlue>())
                        {
                            GetComponent<RPCBlue>().SendData();
                        }
                        if (GetComponent<RPClightBlue>())
                        {
                            GetComponent<RPClightBlue>().SendData();
                        }
                    }
                }
                else if (hit.collider == null)
                {
                    lastObjectUnderMouse = null; // 如果沒有碰到新的物件，重置
                }

                // 跟蹤鼠標當前位置
                if (points.Count > 0)
                {
                    pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count + 1;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count, currentMousePosition);
                }
            }
        }
        // 鼠標放開時，停止線條繪製
        if (Input.GetMouseButtonUp(0) && isLineDrawing)
        {
            // 檢查是否連接到相同顏色的方塊
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
            FirstCircleName = "";
            if (hit.collider != null && CheckColorMatch(startObject, hit.collider.gameObject))
            {
                // 如果成功連接，則保留線條
                lineRenderer.positionCount = points.Count; // 固定線條
                visitedObjects.Add(hit.collider.gameObject); // 添加終點物件到已訪問集合
                Debug.Log("成功連接：" + startObject.name + " 和 " + hit.collider.gameObject.name);
                hasDrawnLine = true;
                for (int i = 0; i < pointsObj.Count; i++)
                {
                    pointsObj[i].tag = "Finished";
                }

                switch (startObject.name.Split('1', '2')[0])
                {
                    case "淺藍":
                        for (int i = 0; i < points.Count; i++)
                        {
                            lightBlueSavePoints.Add(points[i]);
                            lightBlueSavepointsObj.Add(pointsObj[i]);
                            lightBlueSavepointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                        }
                        for (int j = 0; j < lightBlueSavepointsObj.Count; j++)
                        {
                            lightBlueSavepointsObj[j].GetComponent<LineRenderer>().positionCount = lightBlueSavePoints.Count;
                            for (int k = 0; k < lightBlueSavePoints.Count; k++)
                            {
                                lightBlueSavepointsObj[j].GetComponent<LineRenderer>().SetPosition(k, lightBlueSavePoints[k]);
                            }
                            lightBlueSavepointsObj[j].GetComponent<LineManagerWithTurns>().enabled = false;
                        }
                        break;
                    case "深藍":
                        for (int i = 0; i < points.Count; i++)
                        {
                            BlueSavePoints.Add(points[i]);
                            BlueSavepointsObj.Add(pointsObj[i]);
                            BlueSavepointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                        }
                        for (int j = 0; j < BlueSavepointsObj.Count; j++)
                        {
                            BlueSavepointsObj[j].GetComponent<LineRenderer>().positionCount = BlueSavePoints.Count;
                            for (int k = 0; k < BlueSavePoints.Count; k++)
                            {
                                BlueSavepointsObj[j].GetComponent<LineRenderer>().SetPosition(k, BlueSavePoints[k]);
                            }
                            BlueSavepointsObj[j].GetComponent<LineManagerWithTurns>().enabled = false;
                        }
                        break;
                    case "黃色":
                        for (int i = 0; i < points.Count; i++)
                        {
                            YellowSavePoints.Add(points[i]);
                            YellowSavepointsObj.Add(pointsObj[i]);
                            YellowSavepointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                        }
                        for (int j = 0; j < YellowSavepointsObj.Count; j++)
                        {
                            YellowSavepointsObj[j].GetComponent<LineRenderer>().positionCount = YellowSavePoints.Count;
                            for (int k = 0; k < YellowSavePoints.Count; k++)
                            {
                                YellowSavepointsObj[j].GetComponent<LineRenderer>().SetPosition(k, YellowSavePoints[k]);
                            }
                            YellowSavepointsObj[j].GetComponent<LineManagerWithTurns>().enabled = false;
                        }
                        break;
                    case "橘色":
                        for (int i = 0; i < points.Count; i++)
                        {
                            OrangeSavePoints.Add(points[i]);
                            OrangeSavepointsObj.Add(pointsObj[i]);
                            OrangeSavepointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                        }
                        for (int j = 0; j < OrangeSavepointsObj.Count; j++)
                        {
                            OrangeSavepointsObj[j].GetComponent<LineRenderer>().positionCount = OrangeSavePoints.Count;
                            for (int k = 0; k < OrangeSavePoints.Count; k++)
                            {
                                OrangeSavepointsObj[j].GetComponent<LineRenderer>().SetPosition(k, OrangeSavePoints[k]);
                            }
                           
                            OrangeSavepointsObj[j].GetComponent<LineManagerWithTurns>().enabled = false;
                        

                        }
                       
                        break;
                }
                ResetDrawingData();
                
            }
            else
            {
                // 否則，重置線條
                for (int i = 0; i < pointsObj.Count; i++)
                {
                    pointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                }
                ResetDrawingData();
                // 同步畫線過程到所有玩家
                
                    if (GetComponent<RPCOrange>())
                    {
                        GetComponent<RPCOrange>().SendClear();
                    }
                    if (GetComponent<RPCYellow>())
                    {
                        GetComponent<RPCYellow>().SendClear();
                    }
                    if (GetComponent<RPCBlue>())
                    {
                        GetComponent<RPCBlue>().SendClear();
                    }
                    if (GetComponent<RPClightBlue>())
                    {
                        GetComponent<RPClightBlue>().SendClear();
                    }
                
                Debug.Log("連接失敗，重置線條");
                
            }
            isLineDrawing = false;
            startObject = null; // 重置起始物件
            visitedObjects.Clear();
        }
        


        if (hasDrawnLine) // 只有在畫過線的情況下才進行檢查
        {
            if (CheckLineOrder() && YellowSavePoints.Count > 0 && OrangeSavePoints.Count > 0 && lightBlueSavePoints.Count > 0 && BlueSavePoints.Count > 0)
            {
                dialogBox.SetActive(false);
                dialogBox3.SetActive(false);
                dialogBox2.SetActive(false);
                PlayVideo(); // 如果順序正確且所有線條完成，播放影片

                // 同步狀態到另一位玩家
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayVideo", true },
                    { "dialogBox", false },
                    { "dialogBox3", false },
                    { "dialogBox2", false }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            else if (!CheckLineOrder() && dialogBox.activeSelf)
            {
                dialogBox.SetActive(false);
                dialogBox3.SetActive(true); // 如果順序錯誤，顯示對話框
                DisableInput();
                if (dialogBox2.activeSelf)
                {
                    dialogBox3.SetActive(false);
                }
                // 同步狀態到另一位玩家
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "dialogBox", false },
                    { "dialogBox3", true },
                    { "dialogBox2", dialogBox2.activeSelf }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (dialogBox3.activeSelf) // 確保只有當dialogBox3是活躍的時候才關閉它
                {
                    Debug.Log("Dialog Box  Activated");
                    dialogBox2.SetActive(true);   // 開啟對話框2                    
                    dialogBox3.SetActive(false); // 關閉對話框3                    
                    EnableInput();
                    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "dialogBox2", true },
                        { "dialogBox3", false }
                    };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
                }
            }

        }


    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {



        // 檢查是否同步播放影片
        if (changedProps.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)changedProps["PlayVideo"];
            // 只在尚未播放過影片時才呼叫 PlayVideo()
            if (shouldPlayVideo && !hasPlayed)
            {
                PlayVideo();
            }
        }

        // 檢查並同步 dialogBox 狀態
        if (changedProps.ContainsKey("dialogBox"))
        {
            dialogBox.SetActive((bool)changedProps["dialogBox"]);
        }

        if (changedProps.ContainsKey("dialogBox3"))
        {
            dialogBox3.SetActive((bool)changedProps["dialogBox3"]);
        }

        if (changedProps.ContainsKey("dialogBox2"))
        {
            dialogBox2.SetActive((bool)changedProps["dialogBox2"]);
        }

        if (changedProps.ContainsKey("NewScenes"))
        {
            NewScenes.SetActive((bool)changedProps["NewScenes"]);
        }
    }

    //------------------------線條相關開始-------------------------------------------------
    //清除邏輯
    private void ResetDrawingData()
    {
        pointsObj.Clear();
        points.Clear();
        FirstCircleName = "";
        SecondCircleName = "";
    }

    // 設定線條顏色   
    private void SetLineColorBasedOnName(GameObject obj)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        if (obj.name == "淺藍1" || obj.name == "淺藍2")
        {
            Color lightBlue;
            ColorUtility.TryParseHtmlString("#4F64B3", out lightBlue);
            lineRenderer.startColor = lightBlue;
            lineRenderer.endColor = lightBlue;     
        }
        else if (obj.name == "深藍1" || obj.name == "深藍2")
        {
            Color Blue;
            ColorUtility.TryParseHtmlString("#2F2A60", out Blue);
            lineRenderer.startColor = Blue;
            lineRenderer.endColor = Blue;
        }
        else if (obj.name == "黃色1" || obj.name == "黃色2")
        {
            Color yellow;
            ColorUtility.TryParseHtmlString("#F3D758", out yellow);
            lineRenderer.startColor = yellow;
            lineRenderer.endColor = yellow;    
        }
        else if (obj.name == "橘色1" || obj.name == "橘色2")
        {
            Color orange;
            ColorUtility.TryParseHtmlString("#DD8E15", out orange);
            lineRenderer.startColor = orange;
            lineRenderer.endColor = orange;      
        }
    }
    // 在 PhotonView 上執行同步線條繪製顏色的 RPC 方法
    [PunRPC]
    void SyncLineColor(string colorCode)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }


    // 檢查顏色是否匹配
    private bool CheckColorMatch(GameObject obj1, GameObject obj2)
    {
        // 檢查兩個物件的名稱是否屬於同一組
        if (obj1 == null || obj2 == null)
            return false;

        // 根據名稱匹配
        if ((obj1.name == "橘色1" && obj2.name == "橘色2") ||
            (obj1.name == "橘色2" && obj2.name == "橘色1") ||
            (obj1.name == "黃色1" && obj2.name == "黃色2") ||
            (obj1.name == "黃色2" && obj2.name == "黃色1") ||
            (obj1.name == "深藍1" && obj2.name == "深藍2") ||
            (obj1.name == "深藍2" && obj2.name == "深藍1") ||
            (obj1.name == "淺藍1" && obj2.name == "淺藍2") ||
            (obj1.name == "淺藍2" && obj2.name == "淺藍1"))
        {
            return true;
        }

        return false;
    }
    // 檢查四條線是否按照順序完成
    private bool CheckLineOrder()
    {
        // 清除任何可能影響的狀態
        int completedLines = 0;

        // 檢查每種顏色的線條
        bool yellowCompleted = YellowSavePoints.Count > 0;
        bool orangeCompleted = OrangeSavePoints.Count > 0;
        bool lightBlueCompleted = lightBlueSavePoints.Count > 0;
        bool blueCompleted = BlueSavePoints.Count > 0;

        // 根據顏色判斷已完成的線條
        if (yellowCompleted) completedLines++;
        if (orangeCompleted) completedLines++;
        if (lightBlueCompleted) completedLines++;
        if (blueCompleted) completedLines++;

        // 根據已完成的線條數量檢查顏色
        switch (completedLines)
        {
            case 0:
                return true; // 如果沒有線條，返回 true
            case 1:
                return yellowCompleted; // 如果是黃色，返回 true
            case 2:
                return yellowCompleted && orangeCompleted; // 如果是黃色和橘色，返回 true
            case 3:
                return yellowCompleted && orangeCompleted && lightBlueCompleted; // 如果是黃色、橘色和淺藍色，返回 true
            case 4:
                return yellowCompleted && orangeCompleted && lightBlueCompleted && blueCompleted; // 如果是所有顏色，返回 true
        }

        return false;
    }
    //------------------------線條相關結束-------------------------------------------------



    // 播放影片
    private void PlayVideo()
    {
        if (!hasPlayed) // 如果影片尚未播放過
        {
            videoPlayer.Play();
            hasPlayed = true; // 標記為已播放
            Debug.Log("影片開始播放，hasPlayed 設為 true");
            Debug.Log("Update 執行中, hasPlayed = " + hasPlayed);
            dialogBox.SetActive(false);
            dialogBox3.SetActive(false);
            dialogBox2.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊影片播放結束事件
            //NewScenes.SetActive(true);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 影片播放結束後的處理
        vp.Stop(); // 停止影片播放
        //NewScenes.SetActive(true);
        // 重設「PlayVideo」屬性為 false，避免再次觸發播放
        PhotonHashtable resetProperties = new PhotonHashtable
        {
             { "PlayVideo", false },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(resetProperties);
        dialogBox.SetActive(false);
        dialogBox3.SetActive(false);
        dialogBox2.SetActive(false);
        dialogBox4.SetActive(true);
        vp.loopPointReached -= OnVideoFinished; // 取消註冊事件
        bg.SetActive(false);
        bg2.SetActive(true);

    }

    private IEnumerator DelayedSceneChange()
    {
        // 稍微延遲一下，確保所有玩家收到事件
        yield return new WaitForSeconds(0.1f); // 你可以調整延遲時間
        SceneManager.LoadScene("NewGame1-2");
    }

}
