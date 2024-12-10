using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RPCOrange2 : MonoBehaviourPun
{
    public LineManagerWithTurns2 LineManagerWithTurnsObj;

    public int PositionCount;
    //public Vector3[] Points;
    public LineRenderer lineRenderer;
    void Start()
    {
    }
    [PunRPC]
    public void ClearLineDraw()
    {

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();

        }
        lineRenderer.positionCount = 0;

    }
    // �w�q�@�� RPC ��k�A�ǰe�ƭ�
    [PunRPC]
    public void SyncLineDrawing(int PositionCount, Vector3[] pointsArray)
    {

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            
        }
        Color orange;
        ColorUtility.TryParseHtmlString("#E16620", out orange);
        lineRenderer.startColor = orange;
        lineRenderer.endColor = orange;
        lineRenderer.positionCount = PositionCount;
        for (int k = 0; k < PositionCount; k++)
        {
            lineRenderer.SetPosition(k, pointsArray[k]);
        }
    }
    void Update()
    {

    }
    public void SendClear()
    {
        photonView.RPC("ClearLineDraw", RpcTarget.All);

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
