using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("W��������")]
    public GameObject TipObj;
    //����I��I�����|�u�XW�����ܮ�
    private void OnCollisionEnter2D(Collision2D Hit)
    {
        if(Hit.collider.name == "Player")
        {
            TipObj.SetActive(true);
        }
    }
    //�������}�I�������ܮط|���_��
    private void OnCollisionExit2D(Collision2D Hit)
    {
        if (Hit.collider.name == "Player")
        {
            TipObj.SetActive(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Header("�k��ж�")]
    public GameObject Daughterroom;
    [Header("���Y")]
    public GameObject Cooridor;

    public void ToGame()
    {
        //��������
        Application.LoadLevel("Game");
    }

    //Update is called once per frame
    //����U��L�WW�|���}�ж��쨫�Y�W
    void Update()
     {
       if (Input.GetKeyDown(KeyCode.W))
        {
            //����k��ж���������
            //(�����D���a��)Daughterroom.SetActive(Set);
            //����Y��������
            //(�����D���a��)Cooridor.SetActive(!Set);
        }
    }   
}
