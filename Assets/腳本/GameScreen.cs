using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameScreen : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LineRenderer lineRenderer;
    private PhotonView photonView;

    void Start()
    {
        string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        SetupScreen(role);
        photonView = GetComponent<PhotonView>();
    }

    void SetupScreen(string role)
    {
        if (role == "Sister")
        {
            // 彩色畫面，禁用操作
            mainCamera.backgroundColor = Color.white;
            lineRenderer.enabled = false;
        }
        else if (role == "YoungerSister")
        {
            // 黑白畫面，啟用操作
            mainCamera.backgroundColor = Color.black;
            lineRenderer.enabled = true;
        }
    }

    [PunRPC]
    public void DrawLine(Vector3[] positions)
    {
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void OnDrawLine(Vector3[] positions)
    {
        if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Role"] == "YoungerSister")
        {
            photonView.RPC("DrawLine", RpcTarget.All, positions);
        }
    }
}
