using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Text;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class RoomManeger : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text textRoomName;
    [SerializeField] TMP_Text textPlayerList;
    [SerializeField] TMP_Text textNotSelected; // 用於顯示提示訊息
    [SerializeField] Button buttonStartGame;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] Button buttonSelectSister;
    [SerializeField] Button buttonSelectYoungerSister;

    public VideoPlayer videoPlayer; // 用於播放動畫
    public string nextSceneName;    // 動畫結束後要切換的場景名稱
    private bool hasPlayed = false;

    void Start()
    {
        // 設定房間名稱並更新玩家列表
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyPun");
        }
        else
        {
            textRoomName.text = "當前房間：" + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerlist();
        }

        // 只有房主可以按下開始遊戲按鈕
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;

        // 初始化提示訊息
        textNotSelected.text = "請選擇角色！";
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    public void UpdatePlayerlist()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            var player = kvp.Value;
            string role = player.CustomProperties.ContainsKey("Role") ? (string)player.CustomProperties["Role"] : "未選擇";
            sb.AppendLine($"→ {player.NickName} ({role})");
        }
        textPlayerList.text = sb.ToString();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerlist();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerlist();
    }

    public void OnClickStartGame()
    {
        // 確認所有玩家已選擇角色
        if (!AllPlayersHaveRoles())
        {
            UpdateRoomMessage("有玩家尚未選擇角色！");
            return;
        }

        // 確認角色分配是否正確
        if (PlayersSelectedSameRole(out string message))
        {
            UpdateRoomMessage(message);
            return;
        }

        // 所有條件滿足後，播放影片並進行遊戲
        SetRoomProperty("PlayVideo", true);
        //photonView.RPC("PlayVideoRPC", RpcTarget.All);
        //PlayVideo();
    }
    private void SetRoomProperty(string key, object value)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { key, value } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        Debug.Log($"SetRoomProperty: {key} = {value}");
    }
    public void OnSelectSister()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "Role", "姐姐" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        Debug.Log($"{PhotonNetwork.NickName} 選擇了姐姐");
    }

    public void OnSelectYoungerSister()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "Role", "妹妹" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        Debug.Log($"{PhotonNetwork.NickName} 選擇了妹妹");
    }


    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyPun");
    }

    public void SelectRole(string role)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "Role", role } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        Debug.Log($"{PhotonNetwork.NickName} 選擇了角色：{role}");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Role"))
        {
            UpdatePlayerlist();
        }

        // 檢查所有玩家的角色選擇狀態，更新提示文字
        CheckRoomMessage();
    }

    private bool AllPlayersHaveRoles()
    {
        foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            var player = kvp.Value;
            if (!player.CustomProperties.ContainsKey("Role"))
                return false;
        }
        return true;
    }

    private bool PlayersSelectedSameRole(out string message)
    {
        int sisterCount = 0;
        int youngerSisterCount = 0;

        foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            var player = kvp.Value;
            if (player.CustomProperties.ContainsKey("Role"))
            {
                string role = (string)player.CustomProperties["Role"];
                if (role == "姐姐") sisterCount++;
                if (role == "妹妹") youngerSisterCount++;
            }
        }

        if (sisterCount > 1)
        {
            message = "請一位玩家選擇妹妹！";
            return true;
        }

        if (youngerSisterCount > 1)
        {
            message = "請一位玩家選擇姐姐！";
            return true;
        }

        message = string.Empty;
        return false;
    }

    private void CheckRoomMessage()
    {
        if (!AllPlayersHaveRoles())
        {
            UpdateRoomMessage("有玩家尚未選擇角色！");
        }
        else if (PlayersSelectedSameRole(out string message))
        {
            UpdateRoomMessage(message);
        }
        else
        {
            UpdateRoomMessage("角色選擇完畢，可以開始遊戲！");
        }
    }

    private void UpdateRoomMessage(string message)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "RoomMessage", message } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("RoomMessage"))
        {
            textNotSelected.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomMessage"];
        }

        if (propertiesThatChanged.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)PhotonNetwork.CurrentRoom.CustomProperties["PlayVideo"];
            if (shouldPlayVideo && !hasPlayed)
            {
                PlayVideo();
            }
        }
    }

    private void PlayVideo()
    {

        if (!hasPlayed) // 如果影片尚未播放過
        {
            uiCanvas.gameObject.SetActive(false);
            videoPlayer.Play();
            hasPlayed = true; // 標記為已播放
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊影片播放結束事件
        }
        else
        {
            Debug.Log("Video already played.");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // 停止影片播放
        SetRoomProperty("PlayVideo", false);
        SceneManager.LoadScene("NewGame1");
    }
}
