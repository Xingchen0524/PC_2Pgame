using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    // �Ψ���ܰT����UI��r
    private bool isTimerActive = false;
    public GameObject dialogBox;

    void Start()
    {
        Debug.Log("Timer Started");
        StartCoroutine(ShowMessageAfterDelay(20f)); // �]�w20�����ܤ�r
    }

    IEnumerator ShowMessageAfterDelay(float delay)
    {
        float remainingTime = delay;

        while (remainingTime > 0)
        {
            Debug.Log("�˼Ʈɶ��G" + remainingTime + " ��"); // �C����ܭ˼Ʈɶ�
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }
        dialogBox.SetActive(true); // �T�O��r�O�i����
    }
}
