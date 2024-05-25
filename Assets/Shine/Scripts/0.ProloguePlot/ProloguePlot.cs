using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProloguePlot : MonoBehaviour
{
    [Header("擺放前導影片物件")]
    public VideoPlayer StartMovie;
    public VideoPlayer QTEMovie;
    public VideoPlayer EndMovie;
    public VideoPlayer ProloguePlotMovie;

    [Header("設定幾秒後才可以偵測影片是否撥放完畢")]
    public float SetTime;
    public float NowTime;

    public GameObject[] VideoGameObject;
    public GameObject DialogueObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //NowTime = NowTime + Time.deltaTime;
        if (VideoGameObject[0].active || VideoGameObject[2].active)
        {
            NowTime += Time.deltaTime;
        }
        //如果影片撥放完畢，並且目前的計時時間，就可以切換到遊戲場景
        if (NowTime > SetTime)
        {
            if (!StartMovie.isPlaying&& VideoGameObject[0].active) {
                //關閉前導動畫
                StartMovie.gameObject.SetActive(false);
                VideoGameObject[0].SetActive(false);
                //開啟QTE背景動畫
                QTEMovie.Play();
                VideoGameObject[1].SetActive(true);
                NowTime = 0;
            }
            if (!EndMovie.isPlaying&& VideoGameObject[2].active)
            {
                //關閉end動畫
                EndMovie.gameObject.SetActive(false);
                VideoGameObject[2].SetActive(false);
                NowTime = 0;

                //開啟下個劇情
                //StartCoroutine(PlayVideo2());
                DialogueObj.SetActive(true);

            }
          /*  if (!ProloguePlotMovie.isPlaying && VideoGameObject[3].active)
            {
                //關閉end動畫
                ProloguePlotMovie.gameObject.SetActive(false);
                VideoGameObject[3].SetActive(false);
                DialogueObj.SetActive(true);
                NowTime = 0;
            }*/
        }
    }

    IEnumerator PlayVideo2() {
        yield return new WaitForEndOfFrame();
        ProloguePlotMovie.Play();
        VideoGameObject[3].SetActive(true);
    }
}
