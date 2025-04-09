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
    private int currentIndex = 1; // 預設 dialogBox2（索引1）
    private bool canShowDialogs = false; // 新增變數，收到ImageGo後才顯示對話框

    void Start()
    {
        HideAllDialogs(); // 遊戲開始時不顯示任何對話框
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
        if (propertiesThatChanged.ContainsKey("ImageGo"))
        {
            canShowDialogs = (bool)propertiesThatChanged["ImageGo"];
            UpdateBackground();
            Debug.Log("收到 ImageGo 指令，開始顯示對話框。");
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
    }

    void UpdateBackground()
    {
        if (!canShowDialogs) return; // 只有收到 ImageGo 指令後才會顯示對話框

        HideAllDialogs();

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {
                if (currentIndex == 0) dialogBox.SetActive(true);
                else if (currentIndex == 1) dialogBox2.SetActive(true);
                else if (currentIndex == 2) dialogBox3.SetActive(true);
            }
            else if (role == "姐姐")
            {
                if (currentIndex == 0) dialogBox4.SetActive(true);
                else if (currentIndex == 1) dialogBox5.SetActive(true);
                else if (currentIndex == 2) dialogBox6.SetActive(true);
            }
        }
        Debug.Log($"目前顯示背景索引: {currentIndex} (在 {PhotonNetwork.LocalPlayer.NickName} 端)");
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
        Debug.Log("收到 ImageGo 指令，開始顯示對話框。");
    }
}
