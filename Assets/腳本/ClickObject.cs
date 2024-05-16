using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    // [Header("被點擊物件")]
    //public GameObject ClickObjcet;
    [Header("放大物件")]
    public GameObject ObjcetAn;
    [Header("提示")]
    public GameObject TipObj;
    [Header("放大的背景")]
    public GameObject Shady;
    [Header("對話")]
    public GameObject Dialogue;

    public bool isTouchObj;
    //當玩家碰到客廳物品，就會有提示
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            TipObj.SetActive(true);
            isTouchObj = true;
        }
    }
    //玩家離開，提示關閉
    private void OnTriggerExit2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            TipObj.SetActive(false);
            isTouchObj = false;

        }
    }


   
    //按E開啟物件
    void Update()
    {
        if (isTouchObj)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //提示E關閉，被典籍物件關閉，放大物件打開，對話打開，黑幕打開
                TipObj.SetActive(false);
                //ClickObjcet.SetActive(false);
                ObjcetAn.SetActive(true);
                Shady.SetActive(true);
                Dialogue.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //提示E打開，被典籍物件打開，放大物件關閉，對話關閉，黑幕關閉
                TipObj.SetActive(true);
               // ClickObjcet.SetActive(true);
                ObjcetAn.SetActive(false);
                Dialogue.SetActive(false);
                Shady.SetActive(false);
            }
        }
    }

     //點擊物件開啟物件
    //private void OnMouseDown()
    //{
        //ObjcetAn.SetActive(true);
        //ClickObjcet.SetActive(false);
    //}

  
}

