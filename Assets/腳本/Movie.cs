using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movie : MonoBehaviour
{
    [Header("第一關劇情影片")]
    public VideoPlayer StoryMovie1;
    [Header("設定幾秒後才可以偵測影片是否撥放完畢")]
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
        //如果影片撥放完畢，並且目前的計時時間，就可以切換到遊戲場景
        if (!StoryMovie1.isPlaying && NowTime > SetTime)
        {
            ToGame();
        }
    }
    public void ToGame()
    {
        //切換場景
        Application.LoadLevel("GameBoth");
    }
}
