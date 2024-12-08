using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RPCBlue3 : MonoBehaviourPun
{
    public LineManagerWithTurns3 LineManagerWithTurnsObj;

    public int PositionCount;
    //public Vector3[] Points;
    public LineRenderer lineRenderer;
    void Start()
    {
    }
    // 定義一個 RPC 方法，傳送數值
    [PunRPC]
    public void SyncLineDrawing(int PositionCount, Vector3[] pointsArray)
    {

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            Color Blue;
            ColorUtility.TryParseHtmlString("#93341A", out Blue);
            lineRenderer.startColor = Blue;
            lineRenderer.endColor = Blue;
        }
        lineRenderer.positionCount = PositionCount;
        for (int k = 0; k < PositionCount; k++)
        {
            lineRenderer.SetPosition(k, pointsArray[k]);
        }
    }
    void Update()
    {

    }

    public void SendData()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        PositionCount = GetComponent<LineRenderer>().positionCount;

        Vector3[] Points = new Vector3[PositionCount];
        for (int i = 0; i < PositionCount; i++)
        {
            Points[i] = GetComponent<LineRenderer>().GetPosition(i);
        }

        photonView.RPC("SyncLineDrawing", RpcTarget.All, PositionCount, Points);
        Debug.Log("Send Data");
    }
}
