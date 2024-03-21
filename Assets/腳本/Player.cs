using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("���a���ʳt��")]
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
        //����a�첾
       playerRB.velocity = new Vector2(Input.GetAxis(AxisHorizontal) * ScriptSpeed, playerRB.velocity.y);
        //����a���ʮɪ��ʵe
        //anim.SetFloat("Speed",Mathf.Abs(playerRB.velocity.x));
        if (Input.GetAxis(AxisHorizontal) != 0)
        {
            ScriptSpeed = Speed;
            anim.SetBool("Walk", true);
            //���a����½��x�b
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
