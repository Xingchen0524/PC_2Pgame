using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Video;
using UnityEngine.Rendering;

public class Game2Controller1 : MonoBehaviourPunCallbacks
{
    public float speed = 1f; // 移動速度
    public GameObject plate; // 盤子圖片（需要在 Inspector 指定）
    private Vector2 movement;
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private PolygonCollider2D plateCollider; // 盤子的 Polygon Collider
    private Collider2D playerCollider;       // 玩家物件的 Collider

    private Vector2 networkPosition; // 用於同步網路位置
    private Vector2 lastSentMovement; // 記錄上次同步的移動量，減少不必要的網路流量

    

    

    void Start()
    {        

        // 記錄起始位置
        startPosition = transform.position;

        // 獲取 Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // 啟用 Rigidbody2D 的插值，讓移動更平滑
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        // 獲取盤子的 Polygon Collider2D
        if (plate != null)
        {
            plateCollider = plate.GetComponent<PolygonCollider2D>();
            if (plateCollider == null)
            {
                Debug.LogError("盤子圖片必須有 Polygon Collider2D！");
            }
        }
        else
        {
            Debug.LogError("未指定盤子圖片！");
        }

        // 獲取玩家的 Collider2D
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("玩家圖片必須有 Collider2D！");
        }
    }

    void Update()
    {
        Vector2 localMovement = Vector2.zero;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "妹妹")
            {
                localMovement.x = Input.GetAxis("Horizontal"); // 妹妹控制 X 軸
            }
            else
            {
                localMovement.y = Input.GetAxis("Vertical"); // 姐姐控制 Y 軸
            }
        }

        // 兩人輸入的合併，使用 Photon 的 CustomProperties 來記錄並同步
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

        // 收集所有玩家的輸入並合併
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("Input"))
            {
                Vector2 playerInput = (Vector2)player.CustomProperties["Input"];
                combinedMovement += playerInput;
            }
        }

        // 進行移動
        Vector2 move = combinedMovement * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // 減少同步頻率，僅在移動變化時才同步
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
            // 碰撞到邊界時重置位置
            Debug.Log("碰到邊界！");
            transform.position = startPosition;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("遊戲完成！");
            // 在此添加完成遊戲的邏輯
        }
    }

    
}


