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
        print("成功");
    }
    public override void OnConnectedToMaster()
    {
        print("連線成功");
        SceneManager.LoadScene("LobbyPun");
        print("真假");
    }
}

