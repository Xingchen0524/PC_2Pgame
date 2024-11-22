using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlicker : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI targetText; // 要閃爍的文字
    [SerializeField] 
    private float flickerSpeed = 1.0f; // 閃爍速度

    private bool isIncreasing = true;
    private Color originalColor;

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("未指定文字元件！");
            return;
        }

        originalColor = targetText.color; // 記錄初始顏色
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

        // 調整透明度
        if (isIncreasing)
        {
            color.a += Time.deltaTime * flickerSpeed;
            if (color.a >= 1.0f)
            {
                color.a = 1.0f;
                isIncreasing = false; // 切換為減少透明度
            }
        }
        else
        {
            color.a -= Time.deltaTime * flickerSpeed;
            if (color.a <= 0.0f)
            {
                color.a = 0.0f;
                isIncreasing = true; // 切換為增加透明度
            }
        }

        targetText.color = color; // 更新文字顏色
    }
}
