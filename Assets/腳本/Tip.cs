using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tip : MonoBehaviour
{
    [Header("����")]
    public GameObject TipObj;
    //���a�I����U���~�A�N�|������
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        if(Hit.name == "Player")
        {
            TipObj.SetActive(true);
        }
    }
    //���a���}�A��������
    private void OnTriggerExit2D(Collider2D Hit)
    {
        if (Hit.name == "Player")
        {
            TipObj.SetActive(false);
        }
    }
}
