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
    [SerializeField] TMP_Text textNotSelected; // �Ω���ܴ��ܰT��
    [SerializeField] Button buttonStartGame;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] Button buttonSelectSister;
    [SerializeField] Button buttonSelectYoungerSister;

    public VideoPlayer videoPlayer; // �Ω󼽩�ʵe
    public string nextSceneName;    // �ʵe������n�����������W��
    private bool hasPlayed = false;

    void Start()
    {
        // �]�w�ж��W�٨ç�s���a�C��
        if (PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyPun");
        }
        else
        {
            textRoomName.text = "��e�ж��G" + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerlist();
        }

        // �u���ХD�i�H���U�}�l�C�����s
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;

        // ��l�ƴ��ܰT��
        textNotSelected.text = "�п�ܨ���I";
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
        // �T�{�Ҧ����a�w��ܨ���
        if (!AllPlayersHaveRoles())
        {
            UpdateRoomMessage("�����a�|����ܨ���I");
            return;
        }

        // �T�{������t�O�_���T
        if (PlayersSelectedSameRole(out string message))
        {
            UpdateRoomMessage(message);
            return;
        }

        // �Ҧ����󺡨���A����v���öi��C��
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
        Debug.Log($"{PhotonNetwork.NickName} ��ܤF����G{role}");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Role"))
        {
            UpdatePlayerlist();
        }

        // �ˬd�Ҧ����a�������ܪ��A�A��s���ܤ�r
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

    private void CheckRoomMessage()
    {
        if (!AllPlayersHaveRoles())
        {
            UpdateRoomMessage("�����a�|����ܨ���I");
        }
        else if (PlayersSelectedSameRole(out string message))
        {
            UpdateRoomMessage(message);
        }
        else
        {
            UpdateRoomMessage("�����ܧ����A�i�H�}�l�C���I");
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

        if (!hasPlayed) // �p�G�v���|������L
        {
            uiCanvas.gameObject.SetActive(false);
            videoPlayer.Play();
            hasPlayed = true; // �аO���w����
            videoPlayer.loopPointReached += OnVideoFinished; // ���U�v�����񵲧��ƥ�
        }
        else
        {
            Debug.Log("Video already played.");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // ����v������
        SetRoomProperty("PlayVideo", false);
        SceneManager.LoadScene("NewGame1");
    }
}
