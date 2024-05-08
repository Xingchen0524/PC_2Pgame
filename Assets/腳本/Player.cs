using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("玩家移動速度")]
    public float Speed;
    float ScriptSpeed;
    public Rigidbody2D playerRB;

    private Animator anim;
    private SpriteRenderer theSR;

    public string AxisHorizontal;
    //控制姊妹的isTrigger Bool
    static public bool ColliderIsTrigger;
    [Header("姊姊妹妹物件")]
    public Transform SisterObj, ElderSisterObj;
    [Header("兩姊妹之間的距離")]
    public float Dis;
    public float D;

    void Start()
    {
        ScriptSpeed = Speed;
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        D = Vector3.Distance(SisterObj.position, ElderSisterObj.position);
        if (Vector3.Distance(SisterObj.position, ElderSisterObj.position) < Dis) 
        {
          ColliderIsTrigger=true;
            GetComponent<Rigidbody2D>().gravityScale = 0;

        }
        else
        {
            ColliderIsTrigger = false;
            GetComponent<Rigidbody2D>().gravityScale = 10;
            
        }
        GetComponent<Collider2D>().isTrigger = ColliderIsTrigger;

        //控制玩家為一
        playerRB.velocity = new Vector2(Input.GetAxis(AxisHorizontal) * ScriptSpeed, playerRB.velocity.y);
        //控制玩家移動時的動畫
        //anim.SetFloat("Speed",Mathf.Abs(playerRB.velocity.x));
        if (Input.GetAxis(AxisHorizontal) != 0)
        {
            ScriptSpeed = Speed;
            anim.SetBool("Walk", true);
            //玩家移動翻轉X軸
            if (playerRB.velocity.x > 0)
            {
                theSR.flipX = true;
            }
            else if (playerRB.velocity.x < 0)
            {
                theSR.flipX = false;
            }
        }
        else
        {
            ScriptSpeed = 0;
            anim.SetBool("Walk", false);
        }
    
    }
}
