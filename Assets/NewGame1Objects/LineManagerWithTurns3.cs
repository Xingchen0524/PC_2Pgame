using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // 用於顯示對話框
using UnityEngine.Video; // 用於播放影片
using UnityEngine.SceneManagement;

public class LineManagerWithTurns3 : MonoBehaviour
{
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
    public VideoPlayer videoPlayer; // 用於播放影片
    public CanvasGroup dialogCanvasGroup; // 用於控制對話框的漸隱效果
    private bool isDialogActive = false; // 判斷對話框是否正在顯示
    private bool hasPlayed = false; // 用來追蹤影片是否已經播放過
    private bool hasDrawnLine = false;
    private bool isFadingOut = false;

    private float timer = 5f;  // 用來追蹤剩餘時間


    void Start()
    {
        // 初始化 LineRenderer
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
        lineRenderer.numCornerVertices = 90;
        //dialogBox.SetActive(true);  // 初始顯示對話框
    }

    void Update()
    {

        // 鼠標按下，選擇起始物件
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // 檢測鼠標點擊的空物件
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
            Debug.Log("Hit: " + hit.collider.gameObject.name);
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

                    //points.Clear(); // 清除之前的点
                    //pointsObj.Clear();

                    visitedObjects.Clear(); // 清除之前經過的物件
                    visitedObjects.Add(hit.collider.gameObject); // 記錄起始物件

                    points.Add(hit.collider.transform.position); // 添加起始點
                    pointsObj.Add(hit.collider.gameObject);

                    pointsObj[0].GetComponent<LineRenderer>().positionCount = 1;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(0, hit.collider.transform.position);

                    // lineRenderer.positionCount = 1;
                    // lineRenderer.SetPosition(0, hit.collider.transform.position);
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

                            // lineRenderer.positionCount = points.Count;
                            // lineRenderer.SetPosition(points.Count - 1, turnPoint);
                            pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count;
                            pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count - 1, turnPoint);
                        }
                    }

                    // 添加新的空物件位置
                    points.Add(hitPosition);
                    pointsObj.Add(hit.collider.gameObject);

                    visitedObjects.Add(hit.collider.gameObject);  // 標記這個空物件已經經過
                                                                  // lineRenderer.positionCount = points.Count;
                                                                  //  lineRenderer.SetPosition(points.Count - 1, hitPosition);
                    pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count - 1, hitPosition);
                }
                else if (hit.collider == null)
                {
                    lastObjectUnderMouse = null; // 如果沒有碰到新的物件，重置
                }

                // 跟蹤鼠標當前位置
                if (points.Count > 0)
                {
                    //lineRenderer.positionCount = points.Count + 1;
                    //lineRenderer.SetPosition(points.Count, currentMousePosition);
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
                            lightBlueSavepointsObj[j].GetComponent<LineManagerWithTurns3>().enabled = false;
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
                            BlueSavepointsObj[j].GetComponent<LineManagerWithTurns3>().enabled = false;
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
                            YellowSavepointsObj[j].GetComponent<LineManagerWithTurns3>().enabled = false;
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
                            OrangeSavepointsObj[j].GetComponent<LineManagerWithTurns3>().enabled = false;


                        }
                        break;
                }
                pointsObj.Clear();
                points.Clear();
                FirstCircleName = "";
                SecondCircleName = "";
            }
            else
            {
                // 否則，重置線條
                for (int i = 0; i < pointsObj.Count; i++)
                {
                    pointsObj[i].GetComponent<LineRenderer>().positionCount = 0;
                }
                // lineRenderer.positionCount = 0;
                pointsObj.Clear();
                points.Clear();
                FirstCircleName = "";
                SecondCircleName = "";
                Debug.Log("連接失敗，重置線條");
            }

            isLineDrawing = false;
            startObject = null; // 重置起始物件
            visitedObjects.Clear();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (dialogBox.activeSelf) // 確保只有當dialogBox是活躍的時候才關閉它
            {
                Debug.Log("Dialog Box  Activated");
                dialogBox2.SetActive(true);   // 開啟對話框2                    
                dialogBox.SetActive(false); // 關閉對話框3                    
            }
        }

        if (hasDrawnLine) // 只有在畫過線的情況下才進行檢查
        {
            if (CheckLineOrder() && YellowSavePoints.Count > 0 && OrangeSavePoints.Count > 0 && lightBlueSavePoints.Count > 0 && BlueSavePoints.Count > 0)
            {
                dialogBox.SetActive(false);
                dialogBox3.SetActive(false);
                dialogBox2.SetActive(false);
                PlayVideo(); // 如果順序正確且所有線條完成，播放影片
            }
            else if (!CheckLineOrder() && dialogBox2.activeSelf)
            {
                dialogBox2.SetActive(false);
                dialogBox4.SetActive(true); // 如果順序錯誤，顯示對話框
                Debug.Log("Dialog Box 4 Activated");
                if (dialogBox4.activeSelf)
                {
                    dialogBox2.SetActive(false);
                }
            }
            else if (!CheckLineOrder() && dialogBox.activeSelf)
            {
                dialogBox.SetActive(false);
                dialogBox4.SetActive(true); // 如果順序錯誤，顯示對話框
                Debug.Log("Dialog Box 4 Activated");
                if (dialogBox4.activeSelf)
                {
                    dialogBox.SetActive(false);
                }
            }

        }
        if (CheckLineOrder() && Input.GetKeyDown(KeyCode.N)) // 按下N鍵
        {
            SceneManager.LoadScene("Menu");// 替換為你要切換的場景名稱
        }

        //if (dialogBox3.activeSelf && !isFadingOut) // 確保只觸發一次
        //{
        //isFadingOut = true;  // 設定淡出狀態
        //StartCoroutine(FadeOutAfterDelay(5f)); // 開始淡出協程
        //}
    }

    // 設定線條顏色
    private void SetLineColorBasedOnName(GameObject obj)
    {
        if (obj.name == "淺藍1" || obj.name == "淺藍2")
        {
            Color lightBlue;
            ColorUtility.TryParseHtmlString("#DE6118", out lightBlue);
            lineRenderer.startColor = lightBlue;
            lineRenderer.endColor = lightBlue;
        }
        else if (obj.name == "深藍1" || obj.name == "深藍2")
        {
            Color Blue;
            ColorUtility.TryParseHtmlString("#93341A", out Blue);
            lineRenderer.startColor = Blue;
            lineRenderer.endColor = Blue;
        }
        else if (obj.name == "黃色1" || obj.name == "黃色2")
        {
            Color yellow;
            ColorUtility.TryParseHtmlString("#F4D85A", out yellow);
            lineRenderer.startColor = yellow;
            lineRenderer.endColor = yellow;
        }
        else if (obj.name == "橘色1" || obj.name == "橘色2")
        {
            Color orange;
            ColorUtility.TryParseHtmlString("#DC8E16", out orange);
            lineRenderer.startColor = orange;
            lineRenderer.endColor = orange;
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


    // 撥放影片
    private void PlayVideo()
    {
        if (!hasPlayed) // 如果影片尚未播放過
        {
            videoPlayer.Play();
            hasPlayed = true; // 標記為已播放
            dialogBox.SetActive(false);
            dialogBox4.SetActive(false);
            dialogBox2.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊影片播放結束事件
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 影片播放結束後的處理
        vp.Stop(); // 停止影片播放

        dialogBox.SetActive(false);
        dialogBox4.SetActive(false);
        dialogBox2.SetActive(false);
        dialogBox3.SetActive(true);
        vp.loopPointReached -= OnVideoFinished; // 取消註冊事件
    }


}
