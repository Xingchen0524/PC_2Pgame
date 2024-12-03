using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("按下 R，準備重載場景。");

            // 設定屬性，通知其他玩家
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "ReloadScene", SceneManager.GetActiveScene().name } // 將當前場景名稱同步給其他玩家
            };
            Debug.Log("設定屬性: " + SceneManager.GetActiveScene().name);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties); // 設定房間屬性
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps)
    {
        // 檢查是否有更新 "ReloadScene"
        if (changedProps.ContainsKey("ReloadScene"))
        {
            string sceneToLoad = (string)changedProps["ReloadScene"]; // 取得場景名稱
            Debug.Log("屬性更新: 需要重載的場景: " + sceneToLoad);

            // 重載場景
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}