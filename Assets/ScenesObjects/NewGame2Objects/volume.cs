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
        //���H�e���P�w
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "�f�f")
            {
                EnableBlackAndWhiteEffect();
            }
            else
            {
                DisableBlackAndWhiteEffect();
                EnableInput(); // ���ҩn�n����J�\��

            }
        }
    }
    // ������J�\��
    void DisableInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        // �Υi��i�@�B�T�� InputManager �j�w����J�ƥ�
    }
    void EnableInput()
    {
        Debug.Log("���� EnableInput");
        Cursor.lockState = CursorLockMode.None; // �Ѱ���w
        Cursor.visible = true; // ��ܷƹ�

        Debug.Log($"After EnableInput -> Cursor State: {Cursor.lockState}, Visible: {Cursor.visible}");
    }
    //���H�e���P�w
    void EnableBlackAndWhiteEffect()
    {
        if (volume1 != null)
        {
            volume1.enabled = true;
        }
    }
    //���H�e���P�w
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
