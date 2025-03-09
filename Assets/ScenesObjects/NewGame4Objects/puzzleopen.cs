using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
        }
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Stop(); // ����v������


        // �o�e�ƥ�A�i�D�Ҧ����a�����Y�N����
        PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

        // ����@�p�q�ɶ���A�i����������A�H�T�O�Ҧ����a����������H��
        StartCoroutine(DelayedSceneChange());

    }

    private IEnumerator DelayedSceneChange()
    {
        // �y�L����@�U�A�T�O�Ҧ����a����ƥ�
        yield return new WaitForSeconds(0.1f); // �A�i�H�վ㩵��ɶ�
        SceneManager.LoadScene("NewGame2-1");
    }

}
