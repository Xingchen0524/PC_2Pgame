using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Game2Controller1 : MonoBehaviourPun
{
    public float speed = 5f; // ���ʳt��
    public GameObject plate; // �L�l�Ϥ��]�ݭn�b Inspector ���w�^
    private Vector2 movement;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private PolygonCollider2D plateCollider; // �L�l�� Polygon Collider
    private Collider2D playerCollider;       // ���a���� Collider

    void Start()
    {
        // �O���_�l��m
        startPosition = transform.position;

        // ��� Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // ����L�l�� Polygon Collider2D
        if (plate != null)
        {
            plateCollider = plate.GetComponent<PolygonCollider2D>();
            if (plateCollider == null)
            {
                Debug.LogError("�L�l�Ϥ������� Polygon Collider2D�I");
            }
        }
        else
        {
            Debug.LogError("�����w�L�l�Ϥ��I");
        }

        // ������a�� Collider2D
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("���a�Ϥ������� Collider2D�I");
        }
    }

    void Update()
    {
        // ���o��J
        movement.x = Input.GetAxis("Horizontal"); // A/D �� ��/��
        movement.y = Input.GetAxis("Vertical");   // W/S �� ��/��

        // �p��s����m
        Vector2 newPosition = rb.position + movement * speed * Time.deltaTime;

        // �ˬd�O�_�W�X�L�l�d��
        if (IsWithinPlate(newPosition))
        {
            rb.MovePosition(newPosition);
        }
        else
        {
            Debug.Log("���ʳQ����A���a�Ϥ��N�W�X�L�l�d��I");
            transform.position = startPosition;
        }
    }

    private bool IsWithinPlate(Vector2 position)
    {
        if (plateCollider == null || playerCollider == null)
            return false;

        // �p�⪱�a�Ϥ����Ҧ�����I
        Bounds playerBounds = playerCollider.bounds;
        Vector2[] playerCorners = new Vector2[4]
        {
            new Vector2(playerBounds.min.x, playerBounds.min.y), // ���U��
            new Vector2(playerBounds.min.x, playerBounds.max.y), // ���W��
            new Vector2(playerBounds.max.x, playerBounds.min.y), // �k�U��
            new Vector2(playerBounds.max.x, playerBounds.max.y)  // �k�W��
        };

        // Debug �T���G��ܪ��a����I��m
        //Debug.Log("���a����I�G");
        //foreach (Vector2 corner in playerCorners)
        //{
            //Debug.Log(corner);
        //}

        // �ˬd�C������I�O�_���b�L�l�� Collider �d��
        foreach (Vector2 corner in playerCorners)
        {
            if (!plateCollider.OverlapPoint(corner))
            {
                Debug.Log($"����I {corner} �W�X�L�l�d��I");
                return false; // ���@���W�X�L�l�d��A�h��^ false
            }
        }

        return true; // �Ҧ������b�L�l�d��
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            // �I������ɮɭ��m��m
            Debug.Log("�I����ɡI");
            transform.position = startPosition;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("�C�������I");
            // �b���K�[�����C�����޿�
        }
    }

}
