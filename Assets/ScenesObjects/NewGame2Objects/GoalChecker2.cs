using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class GoalChecker2 : MonoBehaviourPunCallbacks
{
    public VideoPlayer videoPlayer; // �즲 VideoPlayer ����� Inspector
    private bool hasPlayed = false; // �ΨӰl�ܼv���O�_�w�g����L

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "NewGame2-2")
            {
                PlayVideo(); // ����v��
                Debug.Log("������2-2�A����v���I");
                ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {
                    { "PlayVideo2", true },
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            }
            else
            {
                Debug.Log("�C�������I");
            }
        }
    }
    // �o�Ӥ�k�O Photon ���ت��A�����B�~�q�\ EventReceived
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        // �ˬd�O�_�� "PlayVideo" �o���ݩ��ܧ�
        if (propertiesThatChanged.ContainsKey("PlayVideo"))
        {
            bool playVideo = (bool)propertiesThatChanged["PlayVideo"];

            // �p�G���켽��T���A�h�}�l����v��
            if (playVideo)
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
            videoPlayer.loopPointReached += OnVideoFinished; // ���U�v�����񵲧��ƥ�
        }
    }
    private void OnVideoFinished(VideoPlayer vp)
    {
        // �v�����񵲧��᪺�B�z
        vp.Stop(); // ����v������
        vp.loopPointReached -= OnVideoFinished; // �������U�ƥ�

        // �o�e�ƥ�A�i�D�Ҧ����a�����Y�N����
        PhotonNetwork.RaiseEvent(0, null, new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, new ExitGames.Client.Photon.SendOptions { Reliability = true });

        // ����@�p�q�ɶ���A�i����������A�H�T�O�Ҧ����a����������H��
        StartCoroutine(DelayedSceneChange());
    }
    private IEnumerator DelayedSceneChange()
    {
        // �y�L����@�U�A�T�O�Ҧ����a����ƥ�
        yield return new WaitForSeconds(0.1f); // �A�i�H�վ㩵��ɶ�
        SceneManager.LoadScene("NewGame2-3");
    }

}
