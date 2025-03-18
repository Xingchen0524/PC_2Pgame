using UnityEngine;
using UnityEngine.EventSystems; // 引入事件系統
using UnityEngine.UI; // 引入 UI 相關的

public class TextPopupTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject dialogBox;  // 你的對話框 GameObject
    public GameObject dialogBox2;

    private void Start()
    {
        // 預設對話框隱藏
        dialogBox.SetActive(false);
        dialogBox2.SetActive(false);
    }

    // 當滑鼠進入 UI 元素時顯示對話框
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 顯示對話框
        dialogBox.SetActive(true);
        dialogBox2.SetActive(true);

    }

    // 當滑鼠離開 UI 元素時隱藏對話框
    public void OnPointerExit(PointerEventData eventData)
    {
        // 隱藏對話框
        dialogBox.SetActive(false);
        dialogBox2.SetActive(false);
    }
}
