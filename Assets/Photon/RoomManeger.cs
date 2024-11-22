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
    [SerializeField] TMP_Text textNotSelected; // �Ω���ܴ��ܰT��
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
            textRoomName.text = "��e�ж��G" + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerlist();
        }
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;

        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "isYoungerSister", isYoungerSister } // true���f�f�Afalse���n�n
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
            string role = player.CustomProperties.ContainsKey("Role") ? (string)player.CustomProperties["Role"] : "�����";
            sb.AppendLine($"�� {player.NickName} ({role})");
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
            textNotSelected.text = "�����a�|����ܨ���I";
            return;
        }

        if (PlayersSelectedSameRole(out string message))
        {
            textNotSelected.text = message; // ��ܴ��ܰT��
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
        Debug.Log($"{PhotonNetwork.NickName} ��ܤF����G{role}");
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
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "Role", "�j�j" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        Debug.Log($"{PhotonNetwork.NickName} ��ܤF�j�j");
    }

    public void OnSelectYoungerSister()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable { { "Role", "�f�f" } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        Debug.Log($"{PhotonNetwork.NickName} ��ܤF�f�f");
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
                if (role == "�j�j") sisterCount++;
                if (role == "�f�f") youngerSisterCount++;
            }
        }

        if (sisterCount > 1)
        {
            message = "�Ф@�쪱�a��ܩf�f�I";
            return true;
        }

        if (youngerSisterCount > 1)
        {
            message = "�Ф@�쪱�a��ܩj�j�I";
            return true;
        }

        message = string.Empty;
        return false;
    }
}
