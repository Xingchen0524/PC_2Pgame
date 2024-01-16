using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//使用UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("首頁")]
    public GameObject MenuPage;
    [Header("設定")]
    public GameObject SettingPage;
    // Start is called before the first frame update
    [Header("控制音量的Slider")]
    public Slider ControlVolumSlider;
    
    // Start is called before the first frame update
    [Header("選擇角色")]
    public GameObject RolePage;

    void Start()
    {
        AudioListener.volume = ControlVolumSlider.value;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToGame()
    {
        //切換場景
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
    public void SetMusicVolum()
    {
        Debug.Log(ControlVolumSlider.value);
        //整體聲音的音量=聲音控制Slider的數值(值介於0-1之間)
        AudioListener.volume = ControlVolumSlider.value;
    }
    public void SetRole(bool Set)
    {
        //控制首頁頁面關閉
        MenuPage.SetActive(Set);
        //控制選擇角色頁面關閉
        RolePage.SetActive(!Set);
    }
}
