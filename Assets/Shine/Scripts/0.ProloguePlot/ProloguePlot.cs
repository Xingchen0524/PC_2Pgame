using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProloguePlot : MonoBehaviour
{
    [Header("�\��e�ɼv������")]
    public VideoPlayer StartMovie;
    public VideoPlayer QTEMovie;
    public VideoPlayer EndMovie;
    public VideoPlayer ProloguePlotMovie;

    [Header("�]�w�X���~�i�H�����v���O�_���񧹲�")]
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
        //�p�G�v�����񧹲��A�åB�ثe���p�ɮɶ��A�N�i�H������C������
        if (NowTime > SetTime)
        {
            if (!StartMovie.isPlaying&& VideoGameObject[0].active) {
                //�����e�ɰʵe
                StartMovie.gameObject.SetActive(false);
                VideoGameObject[0].SetActive(false);
                //�}��QTE�I���ʵe
                QTEMovie.Play();
                VideoGameObject[1].SetActive(true);
                NowTime = 0;
            }
            if (!EndMovie.isPlaying&& VideoGameObject[2].active)
            {
                //����end�ʵe
                EndMovie.gameObject.SetActive(false);
                VideoGameObject[2].SetActive(false);
                NowTime = 0;

                //�}�ҤU�Ӽ@��
                //StartCoroutine(PlayVideo2());
                DialogueObj.SetActive(true);

            }
          /*  if (!ProloguePlotMovie.isPlaying && VideoGameObject[3].active)
            {
                //����end�ʵe
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
