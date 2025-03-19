using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LineQTE : MonoBehaviourPunCallbacks
{
    [Header("線的動畫")]
    public Animator Line;
    [Header("移動的線")]
    public Transform MoveLine;
    bool AnimatorState;

    [Header("A正確值最大值")]
    public float MaxNum_A;
    [Header("A正確值最小值")]
    public float MinNum_A;

    public float Test;

    public GameObject dialogBox1; // 失敗時顯示
    public GameObject[] LineQTEs; // 16 個 QTE 物件
    private int currentQTEIndex = 0; // 當前判定的 QTE

    private bool isControllable = false; // 只有妹妹能操作



    void Start()
    {
        currentQTEIndex = 0; // **確保場景開始時數值重置**

        // 確認是否為「妹妹」，只有妹妹能操作
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = PhotonNetwork.LocalPlayer.CustomProperties["Role"].ToString();
            if (role == "妹妹")
            {
                isControllable = true;
            }

            // 嘗試從房間變數讀取當前 QTE 進度，確保同步
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("CurrentQTE"))
            {
                currentQTEIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentQTE"];
                ActivateQTE(currentQTEIndex);
            }
        }
    }


    void Update()
    {


        //Debug.Log(MoveLine.localPosition.x);
        if (!isControllable) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimatorState = !AnimatorState;
            Debug.Log($"AnimatorState 變更為: {AnimatorState}");
        }

        if (AnimatorState)
        {

            Line.speed = 0;
            Test = MoveLine.localPosition.x;

            Debug.Log($"currentQTEIndex: {currentQTEIndex}, LineQTEs.Length: {LineQTEs.Length}");
            if (currentQTEIndex < LineQTEs.Length)
            {

                if (MoveLine.localPosition.x < MaxNum_A && MoveLine.localPosition.x > MinNum_A)
                {

                    Debug.Log($"QTE {currentQTEIndex + 1} 成功！");
                    SetQTEStatus(currentQTEIndex, true);
                    // **如果成功的是 QTE 16，則播放影片**
                    if (currentQTEIndex == 16)
                    {
                        Debug.Log("PlayVideo傳值");
                        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                        {
                            { "PlayVideo", true }
                        };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
                    }
                }
                else
                {
                    Debug.Log("失敗");
                    SetQTEStatus(currentQTEIndex, false);
                }


            }
            else
            {
                Line.speed = 1;
            }
        }

    }

    void SetQTEStatus(int qteIndex, bool isSuccess)
    {


            if (isSuccess)
            {
                //AnimatorState = !AnimatorState;//讓值回歸初始值
                AnimatorState = false;
                LineQTEs[qteIndex].SetActive(false);// **先關閉當前 QTE**
                currentQTEIndex = qteIndex + 1;// **更新 QTE 進度**



                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "CurrentQTE", currentQTEIndex }
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                // **手動觸發 OnRoomPropertiesUpdate 確保立即同步**
                OnRoomPropertiesUpdate(properties);
            }
            else
            {
                // **重置 QTE 進度**
                currentQTEIndex = 0;
                // **確保失敗狀態也立即同步**
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "QTEFailed", true },
                { "CurrentQTE", currentQTEIndex } // 確保 QTE 重新開始
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(properties);

                // **手動觸發 OnRoomPropertiesUpdate 確保失敗立即生效**
                OnRoomPropertiesUpdate(properties);
            }
        
    
        

    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        

        // **同步 QTE 進度**
        if (propertiesThatChanged.ContainsKey("CurrentQTE"))
        {
            int newQTEIndex = (int)propertiesThatChanged["CurrentQTE"];
            ActivateQTE(newQTEIndex);
        }

        // **同步 QTE 失敗**
        if (propertiesThatChanged.ContainsKey("QTEFailed"))
        {
            bool isFailed = (bool)propertiesThatChanged["QTEFailed"];
            if (isFailed)
            {
                dialogBox1.SetActive(true);
                DisableQTE();
                currentQTEIndex = 0;
                //Time.timeScale = 0; // 暫停遊戲
            }
        }


        
    }
    void ActivateQTE(int index)
    {
        for (int i = 0; i < LineQTEs.Length; i++)
        {
            LineQTEs[i].SetActive(i == index);
        }
        currentQTEIndex = index;
    }

    void DisableQTE()
    {
        isControllable = false; // 禁用 QTE 操作
        Line.speed = 0; // 停止 QTE 動畫
    }

    
}




/*
[Header("B正確值最大值")]
public float MaxNum_B;
[Header("B正確值最小值")]
public float MinNum_B;

[Header("C正確值最大值")]
public float MaxNum_C;
[Header("C正確值最小值")]
public float MinNum_C;

[Header("D正確值最大值")]
public float MaxNum_D;
[Header("D正確值最小值")]    
public float MinNum_D;
*/

/*
else if (MoveLine.localPosition.x < MaxNum_B && MoveLine.localPosition.x > MinNum_B)
{
    Debug.Log("B");
}
else if (MoveLine.localPosition.x < MaxNum_C && MoveLine.localPosition.x > MinNum_C)
{
    Debug.Log("C");
}
else if (MoveLine.localPosition.x < MaxNum_D && MoveLine.localPosition.x > MinNum_D)
{
    Debug.Log("D");
}
*/
