using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{
    [Header("����")]
    public GameObject TipObj;
    //���a�I����U���~�A�N�|������
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if(Hit.tag == "Player")
        {
            TipObj.SetActive(true);
        }
    }
    //���a���}�A��������
    private void OnTriggerExit2D(Collider2D Hit)
    {
        if (Hit.tag == "Player")
        {
            TipObj.SetActive(false);
        }
    }
}
