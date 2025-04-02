using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class GoalChecker2 : MonoBehaviourPunCallbacks
{
    public VideoPlayer videoPlayer; // 拖曳 VideoPlayer 物件到 Inspector
    private bool hasPlayed = false; // 用來追蹤影片是否已經播放過

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "NewGame2-2")
            {
                PlayVideo(); // 播放影片
                Debug.Log("偵測到2-2，播放影片！");
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayVideo2", true },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            else
            {
                Debug.Log("遊戲完成！");
            }
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

        // 發送事件，告訴所有玩家場景即將切換
        PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

        // 延遲一小段時間後再進行場景切換，以確保所有玩家都收到切換信號
        StartCoroutine(DelayedSceneChange());
    }
    private IEnumerator DelayedSceneChange()
    {
        // 稍微延遲一下，確保所有玩家收到事件
        yield return new WaitForSeconds(0.1f); // 你可以調整延遲時間
        SceneManager.LoadScene("NewGame2-3");
    }

}
