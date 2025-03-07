using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Video;
using UnityEngine.Rendering;

public class Game2Controller1 : MonoBehaviourPunCallbacks
{
    public float speed = 1f; // ���ʳt��
    public GameObject plate; // �L�l�Ϥ��]�ݭn�b Inspector ���w�^
    private Vector2 movement;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private PolygonCollider2D plateCollider; // �L�l�� Polygon Collider
    private Collider2D playerCollider;       // ���a���� Collider

    private Vector2 networkPosition; // �Ω�P�B������m
    private Vector2 lastSentMovement; // �O���W���P�B�����ʶq�A��֤����n�������y�q

    

    

    void Start()
    {        

        // �O���_�l��m
        startPosition = transform.position;

        // ��� Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // �ҥ� Rigidbody2D �����ȡA�����ʧ󥭷�
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


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
        Vector2 localMovement = Vector2.zero;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "�f�f")
            {
                localMovement.x = Input.GetAxis("Horizontal"); // �f�f���� X �b
            }
            else
            {
                localMovement.y = Input.GetAxis("Vertical"); // �j�j���� Y �b
            }
        }

        // ��H��J���X�֡A�ϥ� Photon �� CustomProperties �ӰO���æP�B
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Input"))
        {
            Vector2 existingInput = (Vector2)PhotonNetwork.LocalPlayer.CustomProperties["Input"];
            if (localMovement != existingInput)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
                {
                    { "Input", localMovement }
                };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
        }
        else
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "Input", localMovement }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
    }

    void FixedUpdate()
    {
        Vector2 combinedMovement = Vector2.zero;

        // �����Ҧ����a����J�æX��
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Input"))
            {
                Vector2 playerInput = (Vector2)player.CustomProperties["Input"];
                combinedMovement += playerInput;
            }
        }

        // �i�沾��
        Vector2 move = combinedMovement * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // ��֦P�B�W�v�A�Ȧb�����ܤƮɤ~�P�B
        if (PhotonNetwork.IsMasterClient && (combinedMovement != lastSentMovement))
        {
            lastSentMovement = combinedMovement;
            photonView.RPC("SyncMovement", RpcTarget.Others, combinedMovement);
        }
    }

    [PunRPC]
    void SyncMovement(Vector2 move)
    {
        networkPosition += move;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
        }
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


