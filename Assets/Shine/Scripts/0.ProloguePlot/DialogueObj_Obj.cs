using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class DialogueObj_Obj : MonoBehaviour
{
    public GameObject OpenObj,CloseObj;
    public SpriteRenderer CloseObjSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }
    //對話一開啟
    public void OnConversationStart(Transform actor)
    {
       // Debug.Log(string.Format("Starting conversation with {0}", actor.name));
    }
    //對話中
    public void OnConversationLine(Subtitle subtitle)
    {
       // Debug.Log(string.Format("{0}: {1}", subtitle.speakerInfo.transform.name, subtitle.formattedText.text));
    }
    //對話結束
    public void OnConversationEnd(Transform actor)
    {
       // Debug.Log(string.Format("Ending conversation with {0}", actor.name));
        CloseObj.SetActive(false);
        if (CloseObjSpriteRenderer != null) {
            CloseObjSpriteRenderer.enabled = false;
        }
        OpenObj.SetActive(true);

//        StartCoroutine(Open_Obj());
    }
    IEnumerator Open_Obj() {
        yield return new WaitForSeconds(0.5f);

    }
}
