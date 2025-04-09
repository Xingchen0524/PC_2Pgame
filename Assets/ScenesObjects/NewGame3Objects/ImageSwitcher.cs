using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ImageSwitcher : MonoBehaviourPunCallbacks
{
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;
    public GameObject dialogBox5;
    public GameObject dialogBox6;
    private int currentIndex = 1; // �w�] dialogBox2�]����1�^
    private bool canShowDialogs = false; // �s�W�ܼơA����ImageGo��~��ܹ�ܮ�

    void Start()
    {
        HideAllDialogs(); // �C���}�l�ɤ���ܥ����ܮ�
    }

    void Update()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role")) return;

        string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        if (role != "�j�j") return; // �u���u�j�j�v��ާ@

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex = (currentIndex - 1 + 3) % 3; // �T�ӭI��
            SetImageProperty();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex = (currentIndex + 1) % 3;
            SetImageProperty();
        }
    }

    void SetImageProperty()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties["bgIndex"] = currentIndex;

        bool success = PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        Debug.Log($"�]�w�ж��ݩ� bgIndex = {currentIndex}�A��s���\: {success}");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("bgIndex"))
        {
            currentIndex = (int)propertiesThatChanged["bgIndex"];
            Debug.Log($"�ж��ݩʧ�s�A�s�� bgIndex = {currentIndex}");
            UpdateBackground();
        }
        if (propertiesThatChanged.ContainsKey("ImageGo"))
        {
            canShowDialogs = (bool)propertiesThatChanged["ImageGo"];
            UpdateBackground();
            Debug.Log("���� ImageGo ���O�A�}�l��ܹ�ܮءC");
        }

    }

    void SyncWithMaster()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("bgIndex", out object index))
        {
            currentIndex = (int)index;
            Debug.Log($"�f�f�ݱq MasterClient �P�B�� bgIndex = {currentIndex}");
        }
        else
        {
            Debug.LogWarning("�f�f�ݵL�k�P�B bgIndex�A�i�� Master �٨S�]�m");
        }
    }

    void UpdateBackground()
    {
        if (!canShowDialogs) return; // �u������ ImageGo ���O��~�|��ܹ�ܮ�

        HideAllDialogs();

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "�f�f")
            {
                if (currentIndex == 0) dialogBox.SetActive(true);
                else if (currentIndex == 1) dialogBox2.SetActive(true);
                else if (currentIndex == 2) dialogBox3.SetActive(true);
            }
            else if (role == "�j�j")
            {
                if (currentIndex == 0) dialogBox4.SetActive(true);
                else if (currentIndex == 1) dialogBox5.SetActive(true);
                else if (currentIndex == 2) dialogBox6.SetActive(true);
            }
        }
        Debug.Log($"�ثe��ܭI������: {currentIndex} (�b {PhotonNetwork.LocalPlayer.NickName} ��)");
    }

    void HideAllDialogs()
    {
        dialogBox.SetActive(false);
        dialogBox2.SetActive(false);
        dialogBox3.SetActive(false);
        dialogBox4.SetActive(false);
        dialogBox5.SetActive(false);
        dialogBox6.SetActive(false);
    }

    public void OnReceiveImageGo()
    {
        canShowDialogs = true;
        UpdateBackground();
        Debug.Log("���� ImageGo ���O�A�}�l��ܹ�ܮءC");
    }
}
