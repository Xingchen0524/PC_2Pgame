using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleQTE : MonoBehaviour
{

    [Header("圈圈線")]
    public Animator CircleLine;
    bool AnimatorState;

    [Header("正確值最大值")]
    public float MaxNum;
    [Header("正確值最小值")]
    public float MinNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AnimatorState = !AnimatorState;
           
        }
        if (AnimatorState)
        {
            CircleLine.speed = 0;
            if (CircleLine.transform.localScale.x < MaxNum && CircleLine.transform.localScale.x > MinNum)
            {
                Debug.Log("成功");
            }
            else {
                Debug.Log("失敗");
            }
        }
        else
        {
            CircleLine.speed = 1;
        }
    }
}
