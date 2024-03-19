using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    [Header("被點擊物件")]
    public GameObject ClickObjcet;
    [Header("物件動畫")]
    public GameObject ObjcetAn;

    //滑鼠點一下
    private void OnMouseDown()
    {
        ObjcetAn.SetActive(true);
        ClickObjcet.SetActive(false);
    }
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

