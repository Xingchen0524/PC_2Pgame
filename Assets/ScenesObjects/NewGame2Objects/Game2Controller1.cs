using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game2Controller1 : MonoBehaviourPun
{
    public GameObject Player;  // 物件 A（可由姐姐與妹妹控制）
    public float moveSpeed = 5f;
    private Vector3 Player_startPosition;  // 物件 A 的起始位置
    private PhotonView pv;

    private string role; // 玩家角色
    private Vector3 currentPosition;

    void Start()
    {
        role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        Debug.Log("Player role: " + role);  // 檢查角色是否正確
        Player_startPosition = Player.transform.position;
        if (pv.IsMine)
        {
            //Destroy(regidbody) 讓物理留在本地
        }
        
    }

    void Update()
    {
        // 只有 `IsMine` 的玩家可以控制物件移動
        if (pv.IsMine)
        {
            if (role == "姐姐")
            {
                // 姐姐控制上下移動
                float verticalMove = 0f;
                if (Input.GetKey(KeyCode.W)) // 物件向上
                {
                    verticalMove = 1;
                }
                if (Input.GetKey(KeyCode.S)) // 物件向下
                {
                    verticalMove = -1;
                }
                
                Vector3 newPosition = Player.transform.position + new Vector3(0, verticalMove * moveSpeed * Time.deltaTime, 0);
                Player.transform.position = newPosition;
                

                // 同步位置到其他玩家
                photonView.RPC("SyncPosition", RpcTarget.Others, newPosition);
            }
            else if (role == "妹妹")
            {
                // 妹妹控制左右移動
                float horizontalMove = 0f;
                if (Input.GetKey(KeyCode.A)) // 物件向左
                {
                    horizontalMove = -1;
                }
                if (Input.GetKey(KeyCode.D)) // 物件向右
                {
                    horizontalMove = 1;
                }
                Vector3 newPosition = Player.transform.position + new Vector3(horizontalMove * moveSpeed * Time.deltaTime, 0, 0);
                Player.transform.position = newPosition;

                // 同步位置到其他玩家
                photonView.RPC("SyncPosition", RpcTarget.Others, newPosition);
            }
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        // 同步物件位置到其他玩家
        Player.transform.position = newPosition;
    }
}
