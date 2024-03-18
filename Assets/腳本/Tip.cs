using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [Header("提示")]
    public GameObject TipObj;
    //當玩家碰到客廳物品，就會有提示
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if(Hit.tag == "Player")
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
}
