using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movie : MonoBehaviour
{
    [Header("�Ĥ@���@���v��")]
    public VideoPlayer StoryMovie1;
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
        if (!StoryMovie1.isPlaying && NowTime > SetTime)
        {
            ToGame();
        }
    }
    public void ToGame()
    {
        //��������
        Application.LoadLevel("GameBoth");
    }
}
