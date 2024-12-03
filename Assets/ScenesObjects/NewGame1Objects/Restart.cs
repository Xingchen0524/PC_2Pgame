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
            Debug.Log("���U R�A�ǳƭ��������C");

            // �]�w�ݩʡA�q����L���a
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
            {
                { "ReloadScene", SceneManager.GetActiveScene().name } // �N��e�����W�٦P�B����L���a
            };
            Debug.Log("�]�w�ݩ�: " + SceneManager.GetActiveScene().name);
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties); // �]�w�ж��ݩ�
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps)
    {
        // �ˬd�O�_����s "ReloadScene"
        if (changedProps.ContainsKey("ReloadScene"))
        {
            string sceneToLoad = (string)changedProps["ReloadScene"]; // ���o�����W��
            Debug.Log("�ݩʧ�s: �ݭn����������: " + sceneToLoad);

            // ��������
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}