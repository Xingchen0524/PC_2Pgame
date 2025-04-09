using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FailureScreenEffect : MonoBehaviour
{
    [Header("�����л\�]�w")]
    public Image redOverlay;         // �����л\�� UI Image
    public float redFadeDuration = 0.5f; // ����H�J/�H�X�ɶ�
    public float redHoldDuration = 0.5f; // ����O���ɶ�
    public float redMaxAlpha = 0.8f;     // ����̤j�z����

    [Header("�¦�B�n�]�w")]
    public Image blackOverlay;       // �¦�B�n�� UI Image
    public float blackFadeDuration = 1f; // �¦�H�J�ɶ�
    public float blackTargetAlpha = 0.5f; // �¦�B�n�ؼгz����

    [Header("��ܮ�")]
    public GameObject dialogBox;     // ���ѫ�n��ܪ���ܮ�

    // Ĳ�o����ĪG�G����ĪG���槹����A����¦�B�n+��ܮ�
    public void TriggerFailureEffect()
    {
        StartCoroutine(FailureEffectSequence());
    }

    IEnumerator FailureEffectSequence()
    {
        // �������ĪG�G�H�J->�O��->�H�X
        yield return StartCoroutine(FadeRedOverlay());
        // ����ĪG������A����¦�B�n�H�J
        yield return StartCoroutine(FadeInBlackOverlay());
        // �¦�B�n�F��ؼгz���׫�A��ܹ�ܮ�
        if (dialogBox != null)
        {
            dialogBox.SetActive(true);
        }
    }

    IEnumerator FadeRedOverlay()
    {
        // �Ұʬ����л\�A��l�z���׳]�� 0
        redOverlay.gameObject.SetActive(true);
        Color color = redOverlay.color;
        color.a = 0f;
        redOverlay.color = color;

        // �H�J�G�q 0 �� redMaxAlpha
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

        // �O�� redHoldDuration
        yield return new WaitForSeconds(redHoldDuration);

        // �H�X�G�q redMaxAlpha �� 0
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
        // �Ұʶ¦�B�n�A��l�z���׳]�� 0
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

    // ���եΡG�� F ��Ĳ�o�ĪG
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TriggerFailureEffect();
        }
    }
}
