using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�C�����g�����q=SaveData���x�s�����q��
        AudioListener.volume = SaveData.SaveVolume;
        //Test=SaveData.SaveVolume
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
