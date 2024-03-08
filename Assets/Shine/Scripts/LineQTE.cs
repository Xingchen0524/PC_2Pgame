using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineQTE : MonoBehaviour
{
    [Header("�u���ʵe")]
    public Animator Line;
    [Header("���ʪ��u")]
    public Transform MoveLine;
    bool AnimatorState;

    [Header("A���T�ȳ̤j��")]
    public float MaxNum_A;
    [Header("A���T�ȳ̤p��")]
    public float MinNum_A;

    [Header("B���T�ȳ̤j��")]
    public float MaxNum_B;
    [Header("B���T�ȳ̤p��")]
    public float MinNum_B;

    [Header("C���T�ȳ̤j��")]
    public float MaxNum_C;
    [Header("C���T�ȳ̤p��")]
    public float MinNum_C;

    [Header("D���T�ȳ̤j��")]
    public float MaxNum_D;
    [Header("D���T�ȳ̤p��")]
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
                Debug.Log("����");
            }
        }
        else
        {
            Line.speed = 1;
        }
    }
}
