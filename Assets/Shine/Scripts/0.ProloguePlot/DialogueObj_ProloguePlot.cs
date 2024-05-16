using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueObj_ProloguePlot : MonoBehaviour
{
    public GameObject UICanvas;
    // Start is called before the first frame update
    void Start()
    {

    }
    //��ܤ@�}��
    public void OnConversationStart(Transform actor)
    {
        //Debug.Log(string.Format("Starting conversation with {0}", actor.name));
    }
    //��ܤ�
    public void OnConversationLine(Subtitle subtitle)
    {
        //Debug.Log(string.Format("{0}: {1}", subtitle.speakerInfo.transform.name, subtitle.formattedText.text));
    }
    //��ܵ���
    public void OnConversationEnd(Transform actor)
    {
        //Debug.Log(string.Format("Ending conversation with {0}", actor.name));
        UICanvas.SetActive(true);
    }
}
