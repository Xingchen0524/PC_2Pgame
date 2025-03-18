using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ImageSwitcher : MonoBehaviourPunCallbacks
{
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    private int currentIndex = 1; // �w�] dialogBox2�]����1�^

    void Start()
    {
        dialogBox.SetActive(false);
        dialogBox2.SetActive(true);
        dialogBox3.SetActive(false);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "�j�j")
            {
                UpdateBackground();
            }
            else
            {
                Debug.Log("�f�f�ݶ}�l�P�B�I��...");
                SyncWithMaster(); // �f�f�P�B
            }
        }

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

        if (currentIndex == 0)
        {
            properties["dialogBox"] = true;
            properties["dialogBox2"] = false;
            properties["dialogBox3"] = false;
        }
        else if (currentIndex == 1)
        {
            properties["dialogBox"] = false;
            properties["dialogBox2"] = true;
            properties["dialogBox3"] = false;
        }
        else if (currentIndex == 2)
        {
            properties["dialogBox"] = false;
            properties["dialogBox2"] = false;
            properties["dialogBox3"] = true;
        }

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

        if (propertiesThatChanged.ContainsKey("dialogBox"))
        {
            dialogBox.SetActive((bool)propertiesThatChanged["dialogBox"]);
        }
        if (propertiesThatChanged.ContainsKey("dialogBox2"))
        {
            dialogBox2.SetActive((bool)propertiesThatChanged["dialogBox2"]);
        }
        if (propertiesThatChanged.ContainsKey("dialogBox3"))
        {
            dialogBox3.SetActive((bool)propertiesThatChanged["dialogBox3"]);
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

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("dialogBox", out object box1))
        {
            dialogBox.SetActive((bool)box1);
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("dialogBox2", out object box2))
        {
            dialogBox2.SetActive((bool)box2);
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("dialogBox3", out object box3))
        {
            dialogBox3.SetActive((bool)box3);
        }

        UpdateBackground();
    }

    void UpdateBackground()
    {
        dialogBox.SetActive(false);
        dialogBox2.SetActive(false);
        dialogBox3.SetActive(false);

        if (currentIndex == 0)
        {
            dialogBox.SetActive(true);
        }
        else if (currentIndex == 1)
        {
            dialogBox2.SetActive(true);
        }
        else if (currentIndex == 2)
        {
            dialogBox3.SetActive(true);
        }

        Debug.Log($"�ثe��ܭI������: {currentIndex} (�b {PhotonNetwork.LocalPlayer.NickName} ��)");
    }
}
