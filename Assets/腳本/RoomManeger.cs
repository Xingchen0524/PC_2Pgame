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
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
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



        // 讓開始按鈕預設為不能點選
        buttonStartGame.interactable = false;

        // 初始化提示訊息
        textNotSelected.text = "請選擇角色！";
    }
    void Update()
    {
        // 檢查是否按下 Z 鍵來切斷動畫並轉場
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 停止影片播放
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
            }

            // 發送訊息給其他玩家，告訴他們需要切換場景
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "PlayVideo", false }  // 停止播放影片的標誌
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

            // 同步場景切換到所有玩家
            PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

            // 直接切換場景
            SceneManager.LoadScene("NewGame1");
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        CheckStartButton();
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
        CheckStartButton();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerlist();
        CheckStartButton();
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
        photonView.RPC("PlayVideoRPC", RpcTarget.All);
        PlayVideo();
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
            CheckStartButton();
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
        // 檢查玩家數量是否為 2
        if (PhotonNetwork.CurrentRoom.PlayerCount != 2)
        {
            UpdateRoomMessage("等待另一位玩家加入！");
        }
        else if (!AllPlayersHaveRoles())
        {
            UpdateRoomMessage("有玩家尚未選擇角色！");
        }
        else if (PlayersSelectedSameRole(out string message))
        {
            UpdateRoomMessage(message);
        }
        else
        {
            // 當玩家數為 2 且所有玩家選擇角色且角色選擇正確時
            UpdateRoomMessage("角色選擇完畢，可以開始遊戲！");
        }
    }
    private void CheckStartButton()
    {
        bool canStart = PhotonNetwork.IsMasterClient &&
                        PhotonNetwork.CurrentRoom.PlayerCount == 2 &&
                        AllPlayersHaveRoles() &&
                        !PlayersSelectedSameRole(out _);

        buttonStartGame.interactable = canStart;
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
            Destroy(GameObject.Find("MusicManager"));//避免背景音樂重複。
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

        // 發送事件，告訴所有玩家場景即將切換
        PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

        // 延遲一小段時間後再進行場景切換，以確保所有玩家都收到切換信號
        StartCoroutine(DelayedSceneChange());
    }
    private void OnEventReceived(ExitGames.Client.Photon.EventData photonEvent)
    {
        if (photonEvent.Code == 0) // 當收到場景切換的事件
        {
            SceneManager.LoadScene("NewGame1");
        }
    }
    private IEnumerator DelayedSceneChange()
    {
        // 稍微延遲一下，確保所有玩家收到事件
        yield return new WaitForSeconds(0.1f); // 你可以調整延遲時間
        SceneManager.LoadScene("NewGame1");
    }
    void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

}
