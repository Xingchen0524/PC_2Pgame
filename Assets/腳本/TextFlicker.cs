using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlicker : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI targetText; // �n�{�{����r
    [SerializeField] 
    private float flickerSpeed = 1.0f; // �{�{�t��

    private bool isIncreasing = true;
    private Color originalColor;

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("�����w��r����I");
            return;
        }

        originalColor = targetText.color; // �O����l�C��
    }

    void Update()
    {
        if (targetText != null)
        {
            FlickerText();
        }
    }

    private void FlickerText()
    {
        Color color = targetText.color;

        // �վ�z����
        if (isIncreasing)
        {
            color.a += Time.deltaTime * flickerSpeed;
            if (color.a >= 1.0f)
            {
                color.a = 1.0f;
                isIncreasing = false; // ��������ֳz����
            }
        }
        else
        {
            color.a -= Time.deltaTime * flickerSpeed;
            if (color.a <= 0.0f)
            {
                color.a = 0.0f;
                isIncreasing = true; // �������W�[�z����
            }
        }

        targetText.color = color; // ��s��r�C��
    }
}
