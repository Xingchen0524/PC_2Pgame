using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class DrawingSync : MonoBehaviourPunCallbacks
{
    public GameObject drawingObject;  // 繪製的物件，例如線條

    void Start()
    {
        // 確保只有「妹妹」玩家可以繪製
        if (PhotonNetwork.IsMasterClient)  // 這裡可以根據需要調整是 Master Client 還是其他
        {
            return;
        }

        // 妹妹開始繪製時會同步繪製進度給姊姊
        SyncDrawingProgress();
    }

    // 同步繪圖進度
    void SyncDrawingProgress()
    {
        // 假設你有一個方法來監聽妹妹的繪圖行為
        // 當妹妹繪製時，將繪製進度透過 Photon 發送給姊姊
        if (drawingObject != null)
        {
            photonView.RPC("UpdateDrawing", RpcTarget.Others, drawingObject.transform.position);
        }
    }

    // 用來接收其他玩家的繪圖更新
    [PunRPC]
    void UpdateDrawing(Vector3 newPosition)
    {
        if (!PhotonNetwork.IsMasterClient)  // 確保姊姊能接收並更新繪製
        {
            // 更新姊姊的畫面，這裡你可以根據實際情況來更新線條位置
            drawingObject.transform.position = newPosition;
        }
    }
}
