using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Wswitch : MonoBehaviour
{
    [Header("W切換場景")]
    public GameObject TipObj;
    [Header("碰到牆壁的人")]
    public GameObject User;
    [Header("下一個座標位置")]
    public Vector3 Pos;
    [Header("設定CM vcam的Bounding Shape 2D")]
    public CinemachineConfiner Obj;
    [Header("下一個mapRange")]
    public PolygonCollider2D mapRange;
    //角色碰到碰撞器會彈出W的提示框
    private void OnCollisionEnter2D(Collision2D Hit)
    {
        if(Hit.collider.tag == "Player")
        {
            User = Hit.collider.gameObject;
            TipObj.SetActive(true);
        }
    }
    //角色離開碰撞器提示框會收起來
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
        //切換場景
        Application.LoadLevel("Game");
    }

    //Update is called once per frame
    //當按下鍵盤上W會離開房間到走廊上
    void Update()
     {
       if (Input.GetKeyDown(KeyCode.W)&& User!=null)
        {
            User.transform.position = Pos;
            Obj.m_BoundingShape2D = mapRange;
        }
    }   
}
