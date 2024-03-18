using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Wswitch : MonoBehaviour
{
    [Header("W��������")]
    public GameObject TipObj;
    [Header("�I��������H")]
    public GameObject User;
    [Header("�U�@�Ӯy�Ц�m")]
    public Vector3 Pos;
    [Header("�]�wCM vcam��Bounding Shape 2D")]
    public CinemachineConfiner Obj;
    [Header("�U�@��mapRange")]
    public PolygonCollider2D mapRange;
    //����I��I�����|�u�XW�����ܮ�
    private void OnCollisionEnter2D(Collision2D Hit)
    {
        if(Hit.collider.tag == "Player")
        {
            User = Hit.collider.gameObject;
            TipObj.SetActive(true);
        }
    }
    //�������}�I�������ܮط|���_��
    private void OnCollisionExit2D(Collision2D Hit)
    {
        if (Hit.collider.tag == "Player")
        {
            User = null;
            TipObj.SetActive(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ToGame()
    {
        //��������
        Application.LoadLevel("Game");
    }

    //Update is called once per frame
    //����U��L�WW�|���}�ж��쨫�Y�W
    void Update()
     {
       if (Input.GetKeyDown(KeyCode.W)&& User!=null)
        {
            User.transform.position = Pos;
            Obj.m_BoundingShape2D = mapRange;
        }
    }   
}
