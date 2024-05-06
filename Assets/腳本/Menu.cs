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
    [Header("調整遊戲解析度")]
    public Dropdown GameSizeDropdown;
    [Header("Server")]
    public GameObject ServerPage;
    [Header("Client")]
    public GameObject ClientPage;
    //----------------------------------
    [Header("選擇角色")]
    public GameObject RolePage;
    // [Header("GameBS")]
    //public GameObject GameBS;
    //[Header("GameLS")]
    // public GameObject GameLS;

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
        //將聲音的音量储存在SaveData腳本中的SaveVolume變數內
        SaveData.SaveVolume = AudioListener.volume;
    }
    public void SetRole(bool Set)
    {
        //控制首頁頁面關閉
        MenuPage.SetActive(Set);
        //控制選擇角色頁面關閉
        RolePage.SetActive(!Set);
    }
    public void SetGameSize()
    {
        if (GameSizeDropdown.value == 0)
        {
            Screen.SetResolution(1920,1080,false);
        }
        if (GameSizeDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, false);
        }
        if (GameSizeDropdown.value == 2)
        {
            Screen.SetResolution(800, 480, false);
        }
    }
    //Bool=true為server,如果為false則開啟Client
    public void SetServerOrClient(bool State) {
        if (State) {
            ServerPage.SetActive(true);
        }
        else
        {
            ClientPage.SetActive(true);
        }
    }
}
