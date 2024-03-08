using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleQTE : MonoBehaviour
{

    [Header("���u")]
    public Animator CircleLine;
    bool AnimatorState;

    [Header("���T�ȳ̤j��")]
    public float MaxNum;
    [Header("���T�ȳ̤p��")]
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
                Debug.Log("���\");
            }
            else {
                Debug.Log("����");
            }
        }
        else
        {
            CircleLine.speed = 1;
        }
    }
}
