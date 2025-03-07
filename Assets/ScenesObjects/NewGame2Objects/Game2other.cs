using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class Game2other : MonoBehaviourPunCallbacks
{
    public VideoPlayer videoPlayer; // 用於播放影片
    public GameObject dialogBox; // 用於顯示對話框
    public GameObject dialogBox2;
    public CanvasGroup dialogCanvasGroup; // 用於控制對話框的漸隱效果
    private bool isDialogActive = false; // 判斷對話框是否正在顯示
    private bool hasPlayed = false; // 用來追蹤影片是否已經播放過
    private bool hasPlayed2 = false;
    private bool hasDrawnLine = false;
    private bool isFadingOut = false;

    public Volume volume;

    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {

        Debug.Log($"Cursor State: {Cursor.lockState}, Visible: {Cursor.visible}");

        if (dialogBox.activeSelf) // 確保只有當dialogBox3是活躍的時候才關閉它
            
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                dialogBox2.SetActive(true);   // 開啟對話框2                    
                dialogBox.SetActive(false); // 關閉對話框3
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "dialogBox2", true },
                        { "dialogBox", false }
                    };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }

        }
    }
}
