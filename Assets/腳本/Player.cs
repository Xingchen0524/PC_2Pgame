using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("���a���ʳt��")]
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
        //����a�첾
       playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed,playerRB.velocity.y);
        //����a���ʮɪ��ʵe
        //anim.SetFloat("Speed",Mathf.Abs(playerRB.velocity.x));
        if(Input.GetAxis("Horizontal")!=0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);

        //���a����½��x�b
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
