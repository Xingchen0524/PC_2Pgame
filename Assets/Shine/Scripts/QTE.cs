using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    [Header("線的動畫")]
    public Animator Line;
    [Header("移動的線")]
    public Transform MoveLine;
    bool AnimatorState;

    [Header("A正確值最大值")]
    public float MaxNum_A;
    [Header("A正確值最小值")]
    public float MinNum_A;

    [Header("B正確值最大值")]
    public float MaxNum_B;
    [Header("B正確值最小值")]
    public float MinNum_B;

    [Header("C正確值最大值")]
    public float MaxNum_C;
    [Header("C正確值最小值")]
    public float MinNum_C;
    
    
    public float Test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimatorState = !AnimatorState;

        }
        if (AnimatorState)
        {
            Line.speed = 0;
            Test = MoveLine.localPosition.x;
            if (MoveLine.localPosition.x < MaxNum_A && MoveLine.localPosition.x >MinNum_A)
            {
                Debug.Log("A");
            }
            else if (MoveLine.localPosition.x < MaxNum_B && MoveLine.localPosition.x > MinNum_B)
            {
                Debug.Log("B");
            }
            else if (MoveLine.localPosition.x < MaxNum_C && MoveLine.localPosition.x > MinNum_C)
            {
                Debug.Log("C");
            }
            else
            {
                Debug.Log("失敗");
            }
        }
        else
        {
            Line.speed = 1;
        }
    }
 
}
