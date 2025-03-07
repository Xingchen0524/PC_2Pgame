using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class Game2other : MonoBehaviourPunCallbacks
{
    public VideoPlayer videoPlayer; // �Ω󼽩�v��
    public GameObject dialogBox; // �Ω���ܹ�ܮ�
    public GameObject dialogBox2;
    public CanvasGroup dialogCanvasGroup; // �Ω󱱨��ܮت������ĪG
    private bool isDialogActive = false; // �P�_��ܮجO�_���b���
    private bool hasPlayed = false; // �ΨӰl�ܼv���O�_�w�g����L
    private bool hasPlayed2 = false;
    private bool hasDrawnLine = false;
    private bool isFadingOut = false;

    public Volume volume;

    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {

        Debug.Log($"Cursor State: {Cursor.lockState}, Visible: {Cursor.visible}");

        if (dialogBox.activeSelf) // �T�O�u����dialogBox3�O���D���ɭԤ~������
            
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                dialogBox2.SetActive(true);   // �}�ҹ�ܮ�2                    
                dialogBox.SetActive(false); // ������ܮ�3
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                    {
                        { "dialogBox2", true },
                        { "dialogBox", false }
                    };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }

        }
    }
}
