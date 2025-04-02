using UnityEngine;

public class CloseUIOnClick : MonoBehaviour
{
    // 指定要關閉的黑色遮罩物件，請在 Inspector 中指定
    public GameObject blackImage;

    // 如果你想關閉的是這個 UI 物件（包含這個按鈕的父物件），可以直接用 gameObject
    // 或者你可以指定另一個物件
    public GameObject uiObjectToClose;

    // 此方法可在 UI 按鈕 OnClick 事件中呼叫
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
            // 如果未指定 uiObjectToClose，就關閉當前掛載此腳本的物件
            gameObject.SetActive(false);
        }
    }
}
