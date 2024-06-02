using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieGM : MonoBehaviour
{
    [Header("擺放影片物件")]
    public VideoPlayer Movie;
    [Header("設定幾秒後才可以偵測影片是否撥放完畢")]
    public float SetTime;
    float NowTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            Movie.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //NowTime = NowTime + Time.deltaTime;
        NowTime += Time.deltaTime;
        //如果影片撥放完畢，並且目前的計時時間，就可以切換到遊戲場景
        if(!Movie.isPlaying&& NowTime> SetTime)
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
