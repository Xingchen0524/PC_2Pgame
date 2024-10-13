using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManagerWithTurns : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private HashSet<GameObject> visitedObjects = new HashSet<GameObject>();  // 存储已经经过的空物件
    private bool isLineDrawing = false;
    public LayerMask clickableObjectsLayer;  // 可点击的空物件层
    private GameObject lastObjectUnderMouse = null;
    private GameObject startObject = null;  // 用來追蹤起始物件

    void Start()
    {
        // 初始化 LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
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
            if (hit.collider != null && !visitedObjects.Contains(hit.collider.gameObject))
            {
                // 設定起始物件
                startObject = hit.collider.gameObject;

                // 設定線條顏色
                SetLineColorBasedOnName(hit.collider.gameObject);

                points.Clear(); // 清除之前的点
                visitedObjects.Clear(); // 清除之前经过的物件
                visitedObjects.Add(hit.collider.gameObject); // 记录起始物件
                points.Add(hit.collider.transform.position); // 添加起始点
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, hit.collider.transform.position);
                isLineDrawing = true;
            }

            if (hit.collider != null)
            {
                // 點擊檢測成功
                Debug.Log("Hit: " + hit.collider.gameObject.name);
            }
        }

        // 鼠标按住时，线条跟随鼠标
        if (isLineDrawing)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePosition.z = 0;

            // 检测鼠标下是否有新的空物件
            RaycastHit2D hit = Physics2D.Raycast(currentMousePosition, Vector2.zero, Mathf.Infinity, clickableObjectsLayer);
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
                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPosition(points.Count - 1, turnPoint);
                    }
                }

                // 添加新的空物件位置
                points.Add(hitPosition);
                visitedObjects.Add(hit.collider.gameObject);  // 标记这个空物件已经经过
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPosition(points.Count - 1, hitPosition);
            }
            else if (hit.collider == null)
            {
                lastObjectUnderMouse = null; // 如果没有碰到新的物件，重置
            }

            // 跟踪鼠标当前位置
            if (points.Count > 0)
            {
                lineRenderer.positionCount = points.Count + 1;
                lineRenderer.SetPosition(points.Count, currentMousePosition);
            }
        }

        // 鼠标放开时，停止线条绘制
        if (Input.GetMouseButtonUp(0) && isLineDrawing)
        {
            // 检查是否连接到相同颜色的方块
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableObjectsLayer);

            if (hit.collider != null && CheckColorMatch(startObject, hit.collider.gameObject))
            {
                // 如果成功连接，则保留线条
                lineRenderer.positionCount = points.Count; // 固定线条
                visitedObjects.Add(hit.collider.gameObject); // 添加終點物件到已訪問集合
                Debug.Log("成功連接：" + startObject.name + " 和 " + hit.collider.gameObject.name);
            }
            else
            {
                // 否则，重置线条
                lineRenderer.positionCount = 0;
                points.Clear();
                Debug.Log("連接失敗，重置線條");
            }

            isLineDrawing = false;
            startObject = null; // 重置起始物件
        }
    }

    // 設定線條顏色
    private void SetLineColorBasedOnName(GameObject obj)
    {
        if (obj.name == "Square (5)" || obj.name == "Square (22)")
        {
            Color lightBlue;
            ColorUtility.TryParseHtmlString("#4F64B3", out lightBlue);
            lineRenderer.startColor = lightBlue;
            lineRenderer.endColor = lightBlue;
        }
        else if (obj.name == "Square (6)" || obj.name == "Square (12)")
        {
            Color Blue;
            ColorUtility.TryParseHtmlString("#2F2A60", out Blue);
            lineRenderer.startColor = Blue;
            lineRenderer.endColor = Blue;
        }
        else if (obj.name == "Square (7)" || obj.name == "Square (16)")
        {
            Color yellow;
            ColorUtility.TryParseHtmlString("#F3D758", out yellow);
            lineRenderer.startColor = yellow;
            lineRenderer.endColor = yellow;
        }
        else if (obj.name == "Square (9)" || obj.name == "Square (24)")
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
        if ((obj1.name == "Square (5)" && obj2.name == "Square (22)") ||
            (obj1.name == "Square (22)" && obj2.name == "Square (5)") ||
            (obj1.name == "Square (6)" && obj2.name == "Square (12)") ||
            (obj1.name == "Square (12)" && obj2.name == "Square (6)") ||
            (obj1.name == "Square (7)" && obj2.name == "Square (16)") ||
            (obj1.name == "Square (16)" && obj2.name == "Square (7)") ||
            (obj1.name == "Square (9)" && obj2.name == "Square (24)") ||
            (obj1.name == "Square (24)" && obj2.name == "Square (9)"))
        {
            return true;
        }

        return false;
    }
}