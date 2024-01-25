using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //遊戲場經的音量=SaveData內儲存的音量值
        AudioListener.volume = SaveData.SaveVolume;
        //Test=SaveData.SaveVolume
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
