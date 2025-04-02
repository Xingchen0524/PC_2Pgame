using UnityEngine;

public class CloseUIOnClick : MonoBehaviour
{
    // ���w�n�������¦�B�n����A�Цb Inspector �����w
    public GameObject blackImage;

    // �p�G�A�Q�������O�o�� UI ����]�]�t�o�ӫ��s��������^�A�i�H������ gameObject
    // �Ϊ̧A�i�H���w�t�@�Ӫ���
    public GameObject uiObjectToClose;

    // ����k�i�b UI ���s OnClick �ƥ󤤩I�s
    public void OnClickClose()
    {
        if (blackImage != null)
        {
            blackImage.SetActive(false);
        }

        if (uiObjectToClose != null)
        {
            uiObjectToClose.SetActive(false);
        }
        else
        {
            // �p�G�����w uiObjectToClose�A�N������e�������}��������
            gameObject.SetActive(false);
        }
    }
}
