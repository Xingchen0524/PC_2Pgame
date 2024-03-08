using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineQTE : MonoBehaviour
{
    [Header("絬笆礶")]
    public Animator Line;
    [Header("簿笆絬")]
    public Transform MoveLine;
    bool AnimatorState;

    [Header("Aタ絋程")]
    public float MaxNum_A;
    [Header("Aタ絋程")]
    public float MinNum_A;

    [Header("Bタ絋程")]
    public float MaxNum_B;
    [Header("Bタ絋程")]
    public float MinNum_B;

    [Header("Cタ絋程")]
    public float MaxNum_C;
    [Header("Cタ絋程")]
    public float MinNum_C;

    [Header("Dタ絋程")]
    public float MaxNum_D;
    [Header("Dタ絋程")]
    public float MinNum_D;
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
            else if (MoveLine.localPosition.x < MaxNum_D && MoveLine.localPosition.x > MinNum_D)
            {
                Debug.Log("D");
            }
            else
            {
                Debug.Log("ア毖");
            }
        }
        else
        {
            Line.speed = 1;
        }
    }
}
