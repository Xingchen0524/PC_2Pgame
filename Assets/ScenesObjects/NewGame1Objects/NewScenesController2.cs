using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NewScenesController2 : MonoBehaviourPunCallbacks
{
    // 當物件被啟用時，這個方法會被呼叫
    void OnEnable()
    {
        Debug.Log("NewScenesController 已啟用，開始監聽 N 鍵");
    }

    void Update()
    {
        // 檢查是否按下 N 鍵
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("按下 N 鍵，嘗試切換場景");

            // 透過 Photon 發送事件，告知所有玩家要切換場景
            PhotonNetwork.RaiseEvent(0, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });

            // 延遲一小段時間後切換場景，確保所有玩家都收到訊息
            StartCoroutine(DelayedSceneChange());
        }
    }

    // 延遲切換場景的 Coroutine
    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("NewGame2-1");
    }
}