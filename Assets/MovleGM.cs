using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovleGM : MonoBehaviour
{
    [Header("�\��e�ɼv������")]
    public VideoPlayer Movie;
    [Header("�]�w�X���~�i�H�����v���O�_���񧹲�")]
    public float SetTime;
    float NowTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //NowTime = NowTime + Time.deltaTime;
        NowTime += Time.deltaTime;
        //�p�G�v�����񧹲��A�åB�ثe���p�ɮɶ��A�N�i�H������C������
        if(!Movie.isPlaying&& NowTime> SetTime)
        {
            ToGame();
        }
    }
    public void ToGame()
    {
        //��������
        Application.LoadLevel("Game");
    }
}
