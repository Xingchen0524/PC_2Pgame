using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System.Text;

public class LobbyManeger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_InputField inputRoomName;
    [SerializeField]
    TMP_InputField inputPlayerName;
    [SerializeField]
    TMP_Text textRoomList;
    void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            if (PhotonNetwork.CurrentLobby == null)
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("join");
    }
    public string GetRoomName() 
    { 
        string roomName=inputRoomName.text;
        return roomName.Trim();
    }
    public string GenPlayerName()
    {
        string PlayerName = inputPlayerName.text;
        return PlayerName.Trim(); 
    }
    public void OnClickCreateRoom() 
    {
        string roomName = GetRoomName();
        string playerName = GenPlayerName();
        if (roomName.Length > 0 && playerName.Length>0)
        {
            PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else 
        {
            print("無效房間名稱或玩家名字");
        }
    }
    public void OnClickJoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GenPlayerName();
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            print("無效房間名稱或玩家名字");
        }
    }
    public override void OnJoinedRoom()
    {
        print("成功加入");
        SceneManager.LoadScene("RoomScene");
    }
    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("update");
        StringBuilder sb = new StringBuilder();
        foreach (RoomInfo roomInfo in roomList)
        {
            if(roomInfo.PlayerCount>0)
            {
                sb.AppendLine("→" + roomInfo.Name);
            }            
        }
        textRoomList.text = sb.ToString();
    }
    
}
