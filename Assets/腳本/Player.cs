using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("���a���ʳt��")]
    public float Speed;
    public Rigidbody2D playerRB;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //����a�첾
       playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed,playerRB.velocity.y);
       
    }
}
