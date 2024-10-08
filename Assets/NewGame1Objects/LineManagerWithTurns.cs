using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class LineManagerWithTurns : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private HashSet<GameObject> visitedObjects = new HashSet<GameObject>();  // 存储已经经过的空物件
    private bool isLineDrawing = false;
    public LayerMask clickableObjectsLayer;  // 可点击的空物件层
    private GameObject lastObjectUnderMouse = null;

    void Start()
    {
        // 初始化 LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
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
            }
            Debug.Log("Hit: " + hit.collider.gameObject.name);
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
            isLineDrawing = false;
            lineRenderer.positionCount = points.Count; // 固定线条
        }
    }
}
