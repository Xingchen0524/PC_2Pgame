using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptBox : MonoBehaviour
{
    [Header("提示框")]
    public GameObject Prompt;
    [Header("叉叉")]
    public GameObject Chacha;

    private void OnMouseDown()
    {
        Prompt.SetActive(false);
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
