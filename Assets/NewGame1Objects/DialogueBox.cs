using System.Drawing;
using TMPro; // 导入TMP命名空间
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    // 引用TextMeshPro组件
    public TextMeshProUGUI myTextMeshPro;

    void Start()
    {
        // 设置初始文字内容
        myTextMeshPro.text = "「你折錯了！媽媽說折被子是有順序的，要從<color=#FFFFFF><i><u>淺色</i></u></color>邊開始阿！」";
        //<color =#000000>淺色</color>
    }
}