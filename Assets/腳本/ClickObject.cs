using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    [Header("被點擊物件")]
    public GameObject ClickObjcet;
    [Header("物件")]
    public GameObject ObjcetAn;
    [Header("提示")]
    public GameObject TipObj;

    //當玩家碰到客廳物品，就會有提示
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            TipObj.SetActive(true);
        }
    }
    //玩家離開，提示關閉
    private void OnTriggerExit2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            TipObj.SetActive(false);
        }
    }


    //點擊物件
    private void OnMouseDown()
    {
        ObjcetAn.SetActive(true);
        ClickObjcet.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonUp("E"))
        {
            ObjcetAn.SetActive(true);
            ClickObjcet.SetActive(false);
        }
    }

    
    

   
}

