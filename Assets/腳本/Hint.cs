using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [Header("提示或說明框")]
    public GameObject Illustrate;
    [Header("打叉")]
    public GameObject cross;

    private void OnMouseDown(){
        Illustrate.SetActive(false);
        cross.SetActive(false);
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
