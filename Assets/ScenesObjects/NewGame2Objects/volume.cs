using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class volume : MonoBehaviourPunCallbacks
{
    public Volume volume1;
    // Start is called before the first frame update
    void Start()
    {
        //雙人畫面判定
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {
                EnableBlackAndWhiteEffect();
            }
            else
            {
                DisableBlackAndWhiteEffect();
                EnableInput(); // 關啟姊姊的輸入功能

            }
        }
    }
    // 關閉輸入功能
    void DisableInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        // 或可更進一步禁用 InputManager 綁定的輸入事件
    }
    void EnableInput()
    {
        Debug.Log("執行 EnableInput");
        Cursor.lockState = CursorLockMode.None; // 解除鎖定
        Cursor.visible = true; // 顯示滑鼠

        Debug.Log($"After EnableInput -> Cursor State: {Cursor.lockState}, Visible: {Cursor.visible}");
    }
    //雙人畫面判定
    void EnableBlackAndWhiteEffect()
    {
        if (volume1 != null)
        {
            volume1.enabled = true;
        }
    }
    //雙人畫面判定
    void DisableBlackAndWhiteEffect()
    {
        if (volume1 != null)
        {
            volume1.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
