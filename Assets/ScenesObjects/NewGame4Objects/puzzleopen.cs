using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class puzzleopen : MonoBehaviourPunCallbacks
{
    public GameObject No1, No2, No3, No4,No5;
    public VideoPlayer videoPlayer; // �Ω󼽩�v��
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
            if (role == "�f�f")
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
            PlayVideo(); // ����v��

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayVideo", true },
                };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)

    {
        // �ˬd�O�_�P�B����v��
        if (changedProps.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)changedProps["PlayVideo"];
            if (shouldPlayVideo)
            {
                PlayVideo();
            }
        }
        if (changedProps.ContainsKey("ScreenBlack"))
        {
            bool shouldBeBlack = (bool)changedProps["ScreenBlack"];
            if (shouldBeBlack)
            {
                StartCoroutine(FadeToBlackAndQuit());
            }
        }
    }
        // ����v��
        private void PlayVideo()
    {
        if (!hasPlayed) // �p�G�v���|������L
        {
            videoPlayer.Play();
            hasPlayed = true; // �аO���w����
            No1.SetActive(false);
            No2.SetActive(false);
            No3.SetActive(false);
            No4.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished; // ���U�v�����񵲧��ƥ�
            StartCoroutine(WaitForVideoEnd());
        }
    }
    IEnumerator WaitForVideoEnd()
    {
        yield return new WaitForSeconds(30f); // ���]�v�����׬� 30 ��A�ھڹ�ڱ��p�ק�

        // �q���Ҧ����a�e���ܶ�
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
    {
        { "ScreenBlack", true }
    };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
    IEnumerator FadeToBlackAndQuit()
    {
        GameObject blackScreen = new GameObject("BlackScreen");
        blackScreen.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        Image blackImage = blackScreen.AddComponent<Image>();
        blackImage.color = new Color(0, 0, 0, 0); // ��l�z��

        // �L��ʵe���e���ܶ�
        float duration = 2f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            blackImage.color = new Color(0, 0, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackImage.color = Color.black;

        yield return new WaitForSeconds(2f); // ���� 2 ��

        Application.Quit(); // �����C�� (�A�Ω� PC)
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // ����v������

        // �q���Ҧ����a�e���ܶ�
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "ScreenBlack", true }
        };



    }


}
