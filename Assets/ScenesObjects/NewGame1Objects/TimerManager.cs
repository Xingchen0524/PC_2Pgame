using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    // 用來顯示訊息的UI文字
    private bool isTimerActive = false;
    public GameObject dialogBox;

    void Start()
    {
        Debug.Log("Timer Started");
        StartCoroutine(ShowMessageAfterDelay(20f)); // 設定20秒後顯示文字
    }

    IEnumerator ShowMessageAfterDelay(float delay)
    {
        float remainingTime = delay;

        while (remainingTime > 0)
        {
            Debug.Log("倒數時間：" + remainingTime + " 秒"); // 每秒顯示倒數時間
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }
        dialogBox.SetActive(true); // 確保文字是可見的
    }
}
