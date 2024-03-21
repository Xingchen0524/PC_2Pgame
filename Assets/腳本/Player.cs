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
    void Start()
    {
        ScriptSpeed = Speed;
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //控制玩家位移
       playerRB.velocity = new Vector2(Input.GetAxis(AxisHorizontal) * ScriptSpeed, playerRB.velocity.y);
        //控制玩家移動時的動畫
        //anim.SetFloat("Speed",Mathf.Abs(playerRB.velocity.x));
        if (Input.GetAxis(AxisHorizontal) != 0)
        {
            ScriptSpeed = Speed;
            anim.SetBool("Walk", true);
            //玩家移動翻轉x軸
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
