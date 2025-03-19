using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoControl : MonoBehaviourPunCallbacks
{

    public VideoPlayer videoPlayer; // 用於播放影片
    private bool hasPlayed = false; // 用來追蹤影片是否已經播放過

    void Start()
    {
        // 確保 VideoPlayer 被正確綁定
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    // 這個方法是 Photon 內建的，不用額外訂閱 EventReceived
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // 檢查是否有 "PlayVideo" 這個屬性變更
        if (propertiesThatChanged.ContainsKey("PlayVideo"))
        {
            bool playVideo = (bool)propertiesThatChanged["PlayVideo"];

            // 如果收到播放訊號，則開始播放影片
            if (playVideo)
            {
                PlayVideo();
            }
        }


    }

    // 撥放影片
    private void PlayVideo()
    {
        if (!hasPlayed) // 如果影片尚未播放過
        {
            videoPlayer.Play();
            hasPlayed = true; // 標記為已播放
            videoPlayer.loopPointReached += OnVideoFinished; // 註冊影片播放結束事件
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // 影片播放結束後的處理
        vp.Stop(); // 停止影片播放
        vp.loopPointReached -= OnVideoFinished; // 取消註冊事件

    }


}
