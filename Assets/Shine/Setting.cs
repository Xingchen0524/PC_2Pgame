using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Setting : MonoBehaviour
{
    [Header("控制音量的Slider")]
    public Slider ControlVolumSlider;
    bool SettingState;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        AudioListener.volume = ControlVolumSlider.value;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SettingState = !SettingState;
            transform.GetChild(0).gameObject.SetActive(SettingState);
        }
        if (GameObject.Find("VideoPlayerObject"))
        {
            GameObject.Find("VideoPlayerObject").GetComponent<UnityEngine.Video.VideoPlayer>().SetDirectAudioVolume(0, ControlVolumSlider.value);
        }
    }
    public void SetMusicVolum()
    {
        Debug.Log(ControlVolumSlider.value);
        //整體聲音的音量=聲音控制Slider的數值(值介於0-1之間)
        AudioListener.volume = ControlVolumSlider.value;
       
        //將聲音的音量储存在SaveData腳本中的SaveVolume變數內
        SaveData.SaveVolume = AudioListener.volume;
    }
    public void Quit()
    {
        //關閉遊戲
        Application.Quit();
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #endif
    }
}
