using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("玩家移動速度")]
    public float Speed;
    public Rigidbody2D playerRB;

    private Animator anim;
    private SpriteRenderer theSR;

    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //控制玩家位移
       playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed,playerRB.velocity.y);
        //控制玩家移動時的動畫
        //anim.SetFloat("Speed",Mathf.Abs(playerRB.velocity.x));
        if(Input.GetAxis("Horizontal")!=0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        //玩家移動翻轉x軸
        if (playerRB.velocity.x > 0)
        {
            theSR.flipX = true;
        }
        else if(playerRB.velocity.x < 0)
        {
            theSR.flipX = false;
        }
    }
}
