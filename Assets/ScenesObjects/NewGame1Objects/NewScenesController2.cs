using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NewScenesController2 : MonoBehaviourPunCallbacks
{
    // ����Q�ҥήɡA�o�Ӥ�k�|�Q�I�s
    void OnEnable()
    {
        Debug.Log("NewScenesController �w�ҥΡA�}�l��ť N ��");
    }

    void Update()
    {
        // �ˬd�O�_���U N ��
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("���U N ��A���դ�������");

            // �z�L Photon �o�e�ƥ�A�i���Ҧ����a�n��������
            PhotonNetwork.RaiseEvent(0, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });

            // ����@�p�q�ɶ�����������A�T�O�Ҧ����a������T��
            StartCoroutine(DelayedSceneChange());
        }
    }

    // ������������� Coroutine
    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("NewGame2-1");
    }
}