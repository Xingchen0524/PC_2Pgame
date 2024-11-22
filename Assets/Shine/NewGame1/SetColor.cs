using Photon.Pun;
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
        // �O�����󪺭�l�C��
        OrignalColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        // �ˬd Camera.main �O�_���ҥ� Volume �ե�
        Volume volume = Camera.main.GetComponent<Volume>();

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];

            // �p�G�O�f�f�A�����N�C��]���¥�
            if (role == "�f�f")
            {
                GetComponent<SpriteRenderer>().color = Color.black;
            }
            else
            {
                // �p�G�O�n�n�A��_��l�C��
                GetComponent<SpriteRenderer>().color = OrignalColor;
            }
        }
    }
}
