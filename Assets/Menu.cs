using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToGame()
    {
        //������ĵ
        Application.LoadLevel("Game");
    }
    public void Quit()
    {
        //�x���C��
        Application.Quit();
    }

        
        
}
