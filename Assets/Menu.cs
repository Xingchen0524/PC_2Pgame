using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//使用UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    [Header("首頁")]
    public GameObject MenuPage;
    [Header("設定")]
    public GameObjeet SettingPage;
    [Header("控制音量的Slider")]
    public slider ControlVolumSlider
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToGame()
    {
        //切換場警
        Application.LoadLevel("Game");
    }
    public void Quit()
    {
        //關閉遊戲
        Application.Quit();
    }

public void SetSetting(bool Set)
{
    //控制首頁頁面關閉
    MenuPage.SetActive(Set);
    //控制設定頁面關閉
    SettingPage.SetActive(!Set);
}



        
        
}
