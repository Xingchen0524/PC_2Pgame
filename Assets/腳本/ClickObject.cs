using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    [Header("�Q�I������")]
    public GameObject ClickObjcet;
    [Header("����ʵe")]
    public GameObject ObjcetAn;

    //�ƹ��I�@�U
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

