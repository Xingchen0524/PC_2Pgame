using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public bool isYoungerSister;

    [SerializeField] Button buttonSelectSister;
    [SerializeField] Button buttonSelectYoungerSister;

    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyPun");
        }
        else
        {
            textRoomName.text = "當前房間：" + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerlist();
        }
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "isYoungerSister", isYoungerSister } // true為妹妹，false為姊姊
        });
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
        if (!AllPlayersHaveRoles())
        {
            textNotSelected.text = "有玩家尚未選擇角色！";
            return;
        }

        if (PlayersSelectedSameRole(out string message))
        {
            textNotSelected.text = message; // 顯示提示訊息
            return;
        }

        SceneManager.LoadScene("NewGame1");
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
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", role } });
        Debug.Log($"{PhotonNetwork.NickName} 選擇了角色：{role}");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Role"))
        {
            UpdatePlayerlist();
        }
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
}
