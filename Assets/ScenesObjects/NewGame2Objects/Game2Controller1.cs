using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game2Controller1 : MonoBehaviourPun
{
    public GameObject Player;  // ���� A�]�i�ѩj�j�P�f�f����^
    public float moveSpeed = 5f;
    private Vector3 Player_startPosition;  // ���� A ���_�l��m
    private PhotonView pv;

    private string role; // ���a����
    private Vector3 currentPosition;

    void Start()
    {
        role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        Debug.Log("Player role: " + role);  // �ˬd����O�_���T
        Player_startPosition = Player.transform.position;
        if (pv.IsMine)
        {
            //Destroy(regidbody) �����z�d�b���a
        }
        
    }

    void Update()
    {
        // �u�� `IsMine` �����a�i�H����󲾰�
        if (pv.IsMine)
        {
            if (role == "�j�j")
            {
                // �j�j����W�U����
                float verticalMove = 0f;
                if (Input.GetKey(KeyCode.W)) // ����V�W
                {
                    verticalMove = 1;
                }
                if (Input.GetKey(KeyCode.S)) // ����V�U
                {
                    verticalMove = -1;
                }
                
                Vector3 newPosition = Player.transform.position + new Vector3(0, verticalMove * moveSpeed * Time.deltaTime, 0);
                Player.transform.position = newPosition;
                

                // �P�B��m���L���a
                photonView.RPC("SyncPosition", RpcTarget.Others, newPosition);
            }
            else if (role == "�f�f")
            {
                // �f�f����k����
                float horizontalMove = 0f;
                if (Input.GetKey(KeyCode.A)) // ����V��
                {
                    horizontalMove = -1;
                }
                if (Input.GetKey(KeyCode.D)) // ����V�k
                {
                    horizontalMove = 1;
                }
                Vector3 newPosition = Player.transform.position + new Vector3(horizontalMove * moveSpeed * Time.deltaTime, 0, 0);
                Player.transform.position = newPosition;

                // �P�B��m���L���a
                photonView.RPC("SyncPosition", RpcTarget.Others, newPosition);
            }
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        // �P�B�����m���L���a
        Player.transform.position = newPosition;
    }
}
