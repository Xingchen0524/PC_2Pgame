using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetColor : MonoBehaviour
{
    public Color OrignalColor;

    // Start is called before the first frame update
    void Start()
    {
        // 記錄物件的原始顏色
        OrignalColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        // 檢查 Camera.main 是否有啟用 Volume 組件
        Volume volume = Camera.main.GetComponent<Volume>();

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];

            // 如果是妹妹，直接將顏色設為黑白
            if (role == "妹妹")
            {
                GetComponent<SpriteRenderer>().color = Color.black;
            }
            else
            {
                // 如果是姊姊，恢復原始顏色
                GetComponent<SpriteRenderer>().color = OrignalColor;
            }
        }
    }
}
