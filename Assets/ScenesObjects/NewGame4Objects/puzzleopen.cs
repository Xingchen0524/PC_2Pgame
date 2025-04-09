using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class puzzleopen : MonoBehaviourPunCallbacks
{
    public GameObject No1, No2, No3, No4,No5;
    public VideoPlayer videoPlayer; // 用於播放影片
    private bool hasPlayed = false;
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {

                No1.SetActive(true);
                No2.SetActive(false);
                No3.SetActive(false);
                No4.SetActive(false);
            }
            else 
            {
                No1.SetActive(false);
                No2.SetActive(false);
                No3.SetActive(true);
                No4.SetActive(false);
            }
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeSelf && dialogBox2.activeSelf && dialogBox3.activeSelf && dialogBox4.activeSelf)
        {
            PlayVideo(); // 播放影片

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayVideo", true },
                };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)

    {
        // 檢查是否同步播放影片
        if (changedProps.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)changedProps["PlayVideo"];
            if (shouldPlayVideo)
            {
                PlayVideo();
            }
        }
        if (changedProps.ContainsKey("ScreenBlack"))
        {
            bool shouldBeBlack = (bool)changedProps["ScreenBlack"];
            if (shouldBeBlack)
            {
                StartCoroutine(FadeToBlackAndQuit());
            }
        }
    }
        // 播放影片
        private void PlayVideo()
    {
        if (!hasPlayed) // 如果影片尚未播放過
        {
            videoPlayer.Play();
            hasPlayed = true; // 標記為已播放
            No1.SetActive(false);
            No2.SetActive(false);
            No3.SetActive(false);
            No4.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊影片播放結束事件
            StartCoroutine(WaitForVideoEnd());
        }
    }
    IEnumerator WaitForVideoEnd()
    {
        yield return new WaitForSeconds(30f); // 假設影片長度為 30 秒，根據實際情況修改

        // 通知所有玩家畫面變黑
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
    {
        { "ScreenBlack", true }
    };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
    IEnumerator FadeToBlackAndQuit()
    {
        GameObject blackScreen = new GameObject("BlackScreen");
        blackScreen.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        Image blackImage = blackScreen.AddComponent<Image>();
        blackImage.color = new Color(0, 0, 0, 0); // 初始透明

        // 過渡動畫讓畫面變黑
        float duration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            blackImage.color = new Color(0, 0, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackImage.color = Color.black;

        yield return new WaitForSeconds(2f); // 等待 2 秒

        Application.Quit(); // 關閉遊戲 (適用於 PC)
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // 停止影片播放

        // 通知所有玩家畫面變黑
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "ScreenBlack", true }
        };



    }


}
