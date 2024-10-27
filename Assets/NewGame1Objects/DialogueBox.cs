using System.Drawing;
using TMPro; // 导入TMP命名空间
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    // 引用TextMeshPro组件
    public TextMeshProUGUI myTextMeshPro;
    public TextMeshProUGUI myTextMeshPro1;
    public TextMeshProUGUI myTextMeshPro2;
    public TextMeshProUGUI myTextMeshPro3;



    void Start()
    {
        // 设置初始文字内容
        myTextMeshPro2.text = "「你折錯了！媽媽說折被子是有順序的，要從<color=#F3D758><u>淺色</u></color>邊開始阿！」";
        myTextMeshPro3.text = "-請按T鍵繼續對話-";
        //<color =#000000>淺色</color>
    }
    private void Update()
    {
        
    }
}