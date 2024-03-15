using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("W切換場景")]
    public GameObject TipObj;
    //角色碰到碰撞器會彈出W的提示框
    private void OnCollisionEnter2D(Collision2D Hit)
    {
        if(Hit.collider.name == "Player")
        {
            TipObj.SetActive(true);
        }
    }
    //角色離開碰撞器提示框會收起來
    private void OnCollisionExit2D(Collision2D Hit)
    {
        if (Hit.collider.name == "Player")
        {
            TipObj.SetActive(false);
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [Header("女兒房間")]
    public GameObject MenuPage;
    [Header("走廊")]
    public GameObject SettingPage;

    public void ToGame()
    {
        //切換場景
        Application.LoadLevel("Game");
    }

    public void SetSetting(bool Set)
    {
        //控制首頁頁面關閉
        MenuPage.SetActive(Set);
        //控制設定頁面關閉
        SettingPage.SetActive(!Set);
    }

    //Update is called once per frame
    //當按下鍵盤上W會離開房間到走廊上
    //void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.W))
    //}
}
