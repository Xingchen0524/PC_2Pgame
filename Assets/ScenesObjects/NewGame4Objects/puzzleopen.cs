using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
        }
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // 停止影片播放


        // 發送事件，告訴所有玩家場景即將切換
        PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

        // 延遲一小段時間後再進行場景切換，以確保所有玩家都收到切換信號
        StartCoroutine(DelayedSceneChange());

    }

    private IEnumerator DelayedSceneChange()
    {
        // 稍微延遲一下，確保所有玩家收到事件
        yield return new WaitForSeconds(0.1f); // 你可以調整延遲時間
        SceneManager.LoadScene("NewGame2-1");
    }

}
