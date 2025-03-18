using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ImageSwitcher : MonoBehaviourPunCallbacks
{
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    private int currentIndex = 1; // 預設 dialogBox2（索引1）

    void Start()
    {
        dialogBox.SetActive(false);
        dialogBox2.SetActive(true);
        dialogBox3.SetActive(false);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "姐姐")
            {
                UpdateBackground();
            }
            else
            {
                Debug.Log("妹妹端開始同步背景...");
                SyncWithMaster(); // 妹妹同步
            }
        }

    }

    void Update()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role")) return;

        string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
        if (role != "姐姐") return; // 只有「姐姐」能操作

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex = (currentIndex - 1 + 3) % 3; // 三個背景
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
        Debug.Log($"設定房間屬性 bgIndex = {currentIndex}，更新成功: {success}");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("bgIndex"))
        {
            currentIndex = (int)propertiesThatChanged["bgIndex"];
            Debug.Log($"房間屬性更新，新的 bgIndex = {currentIndex}");
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
            Debug.Log($"妹妹端從 MasterClient 同步到 bgIndex = {currentIndex}");
        }
        else
        {
            Debug.LogWarning("妹妹端無法同步 bgIndex，可能 Master 還沒設置");
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

        Debug.Log($"目前顯示背景索引: {currentIndex} (在 {PhotonNetwork.LocalPlayer.NickName} 端)");
    }
}
