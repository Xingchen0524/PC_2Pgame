using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FailureScreenEffect : MonoBehaviour
{
    [Header("紅色覆蓋設定")]
    public Image redOverlay;         // 紅色覆蓋的 UI Image
    public float redFadeDuration = 0.5f; // 紅色淡入/淡出時間
    public float redHoldDuration = 0.5f; // 紅色保持時間
    public float redMaxAlpha = 0.8f;     // 紅色最大透明度

    [Header("黑色遮罩設定")]
    public Image blackOverlay;       // 黑色遮罩的 UI Image
    public float blackFadeDuration = 1f; // 黑色淡入時間
    public float blackTargetAlpha = 0.5f; // 黑色遮罩目標透明度

    [Header("對話框")]
    public GameObject dialogBox;     // 失敗後要顯示的對話框

    // 觸發整體效果：紅色效果執行完畢後再執行黑色遮罩+對話框
    public void TriggerFailureEffect()
    {
        StartCoroutine(FailureEffectSequence());
    }

    IEnumerator FailureEffectSequence()
    {
        // 執行紅色效果：淡入->保持->淡出
        yield return StartCoroutine(FadeRedOverlay());
        // 紅色效果完成後，執行黑色遮罩淡入
        yield return StartCoroutine(FadeInBlackOverlay());
        // 黑色遮罩達到目標透明度後，顯示對話框
        if (dialogBox != null)
        {
            dialogBox.SetActive(true);
        }
    }

    IEnumerator FadeRedOverlay()
    {
        // 啟動紅色覆蓋，初始透明度設為 0
        redOverlay.gameObject.SetActive(true);
        Color color = redOverlay.color;
        color.a = 0f;
        redOverlay.color = color;

        // 淡入：從 0 到 redMaxAlpha
        float t = 0f;
        while (t < redFadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, redMaxAlpha, t / redFadeDuration);
            redOverlay.color = color;
            yield return null;
        }
        color.a = redMaxAlpha;
        redOverlay.color = color;

        // 保持 redHoldDuration
        yield return new WaitForSeconds(redHoldDuration);

        // 淡出：從 redMaxAlpha 到 0
        t = 0f;
        while (t < redFadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(redMaxAlpha, 0f, t / redFadeDuration);
            redOverlay.color = color;
            yield return null;
        }
        color.a = 0f;
        redOverlay.color = color;
        redOverlay.gameObject.SetActive(false);
    }

    IEnumerator FadeInBlackOverlay()
    {
        // 啟動黑色遮罩，初始透明度設為 0
        blackOverlay.gameObject.SetActive(true);
        Color color = blackOverlay.color;
        color.a = 0f;
        blackOverlay.color = color;

        float t = 0f;
        while (t < blackFadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, blackTargetAlpha, t / blackFadeDuration);
            blackOverlay.color = color;
            yield return null;
        }
        color.a = blackTargetAlpha;
        blackOverlay.color = color;
    }

    // 測試用：按 F 鍵觸發效果
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerFailureEffect();
        }
    }
}
