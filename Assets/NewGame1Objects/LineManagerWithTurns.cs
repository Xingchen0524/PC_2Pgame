using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManagerWithTurns : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> points = new List<Vector3>();
    public HashSet<GameObject> visitedObjects = new HashSet<GameObject>();  // 存储已经经过的空物件
    public bool isLineDrawing = false;
    public LayerMask clickableObjectsLayer;  // 可点击的空物件层
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
    void Start()
    {
        // 初始化 LineRenderer
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
      
            // 鼠标按下，选择起始物件
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                // 检测鼠标点击的空物件
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

                        //points.Clear(); // 清除之前的点
                        //pointsObj.Clear();

                        visitedObjects.Clear(); // 清除之前经过的物件
                        visitedObjects.Add(hit.collider.gameObject); // 记录起始物件

                        points.Add(hit.collider.transform.position); // 添加起始点
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

            // 鼠标按住时，线条跟随鼠标
            if (isLineDrawing)
            {
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.z = 0;

                // 检测鼠标下是否有新的空物件
                RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
           // Debug.Log(hit.collider.name);
                if (hit.collider.tag != "Circle"&& hit.collider.tag != "Finished" || hit.collider.name == SecondCircleName)
                {
                    if (hit.collider != null && hit.collider.gameObject != lastObjectUnderMouse && !visitedObjects.Contains(hit.collider.gameObject))
                    {
                        lastObjectUnderMouse = hit.collider.gameObject;

                        // 如果经过了新的物件，添加拐点
                        Vector3 hitPosition = hit.collider.transform.position;
                        if (points.Count > 0)
                        {
                            // 检查是否转弯
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

                        visitedObjects.Add(hit.collider.gameObject);  // 标记这个空物件已经经过
                       // lineRenderer.positionCount = points.Count;
                      //  lineRenderer.SetPosition(points.Count - 1, hitPosition);
                    pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count - 1, hitPosition);
                }
                    else if (hit.collider == null)
                    {
                        lastObjectUnderMouse = null; // 如果没有碰到新的物件，重置
                    }

                    // 跟踪鼠标当前位置
                    if (points.Count > 0)
                    {
                    //lineRenderer.positionCount = points.Count + 1;
                    //lineRenderer.SetPosition(points.Count, currentMousePosition);
                    pointsObj[0].GetComponent<LineRenderer>().positionCount = points.Count + 1;
                    pointsObj[0].GetComponent<LineRenderer>().SetPosition(points.Count, currentMousePosition);
                }
                }
            }
            // 鼠标放开时，停止线条绘制
            if (Input.GetMouseButtonUp(0) && isLineDrawing)
            {
                // 检查是否连接到相同颜色的方块
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
                FirstCircleName = "";
                if (hit.collider != null && CheckColorMatch(startObject, hit.collider.gameObject))
                {
                    // 如果成功连接，则保留线条
                    lineRenderer.positionCount = points.Count; // 固定线条
                    visitedObjects.Add(hit.collider.gameObject); // 添加終點物件到已訪問集合
                    Debug.Log("成功連接：" + startObject.name + " 和 " + hit.collider.gameObject.name);
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
                    pointsObj.Clear();
                    points.Clear();
                    FirstCircleName = "";
                    SecondCircleName = "";
                }
                else
                {
                // 否则，重置线条
                for (int i = 0; i < pointsObj.Count; i++) {
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
        
    }

    // 設定線條顏色
    private void SetLineColorBasedOnName(GameObject obj)
    {
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
}