using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class DrawingSync : MonoBehaviourPunCallbacks
{
    public GameObject drawingObject;  // ø�s������A�Ҧp�u��

    void Start()
    {
        // �T�O�u���u�f�f�v���a�i�Hø�s
        if (PhotonNetwork.IsMasterClient)  // �o�̥i�H�ھڻݭn�վ�O Master Client �٬O��L
        {
            return;
        }

        // �f�f�}�lø�s�ɷ|�P�Bø�s�i�׵��n�n
        SyncDrawingProgress();
    }

    // �P�Bø�϶i��
    void SyncDrawingProgress()
    {
        // ���]�A���@�Ӥ�k�Ӻ�ť�f�f��ø�Ϧ欰
        // ��f�fø�s�ɡA�Nø�s�i�׳z�L Photon �o�e���n�n
        if (drawingObject != null)
        {
            photonView.RPC("UpdateDrawing", RpcTarget.Others, drawingObject.transform.position);
        }
    }

    // �Ψӱ�����L���a��ø�ϧ�s
    [PunRPC]
    void UpdateDrawing(Vector3 newPosition)
    {
        if (!PhotonNetwork.IsMasterClient)  // �T�O�n�n�౵���ç�sø�s
        {
            // ��s�n�n���e���A�o�̧A�i�H�ھڹ�ڱ��p�ӧ�s�u����m
            drawingObject.transform.position = newPosition;
        }
    }
}
