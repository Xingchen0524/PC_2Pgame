using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartManeger : MonoBehaviourPunCallbacks
{
    public void onclickstart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        print("���\");
    }
    public override void OnConnectedToMaster()
    {
        print("�s�u���\");
        SceneManager.LoadScene("LobbyPun");
        print("�u��");
    }
}

