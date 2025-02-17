using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSync : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 當前玩家擁有此物件
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // 接收來自其他玩家的數據
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}