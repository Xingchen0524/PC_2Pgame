using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ϥ�UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    [Header("����")]
    public GameObject MenuPage;
    [Header("�]�w")]
    public GameObjeet SettingPage;
    [Header("����q��Slider")]
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
        //������ĵ
        Application.LoadLevel("Game");
    }
    public void Quit()
    {
        //�����C��
        Application.Quit();
    }

public void SetSetting(bool Set)
{
    //�������������
    MenuPage.SetActive(Set);
    //����]�w��������
    SettingPage.SetActive(!Set);
}



        
        
}
