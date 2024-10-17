using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetColor : MonoBehaviour
{
    public Color OrignalColor;
    // Start is called before the first frame update
    void Start()
    {
        OrignalColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.GetComponent<Volume>().enabled)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else {
            GetComponent<SpriteRenderer>().color = OrignalColor;

        }
    }
}
