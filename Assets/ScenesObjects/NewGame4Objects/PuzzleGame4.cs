using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PuzzleGame4 : MonoBehaviourPunCallbacks
{
    public List<Image> slots;  // �s�񥪰����ӫ��Ϥ���
    public List<Sprite> puzzlePieces; // �i�ѿ�ܪ����ϸH��
    public List<Sprite> targetPieces; // �ؼЫ��Ϫ��U�ӸH��
    public GameObject dialogBox;
    public GameObject dialogBox2;
    public GameObject dialogBox3;
    public GameObject dialogBox4;
    public GameObject dialogBox5;
    public GameObject blackend;

    public GameObject No1, No2, No3, No4;
    private int selectedSlotIndex = 0;  // ��e��ܪ����Ϥ������
    private int selectedPieceIndex = 0; // ��e��ܪ��Ϥ�����

    private int currentStage = 4; // ��e���d�]1~4�^
    private bool isCompleted1 = false, isCompleted2 = false; // �O����W���a�O�_��������

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            string role = (string)PhotonNetwork.LocalPlayer.CustomProperties["Role"];
            if (role == "�f�f")
            {

            }
            else
            {
                No1.SetActive(false);
                No2.SetActive(false);
                No3.SetActive(false);
                No4.SetActive(true);
            }
        }
        ShufflePuzzlePieces(); // �A���ë��Ϥ�
        HighlightSelectedSlot(); // ��ܱj�տ�ܮ�

        if (slots == null || slots.Count == 0)
        {
            Debug.LogError("slots �|����l�Ʃά��šI");
            return;
        }
        if (targetPieces == null || targetPieces.Count == 0)
        {
            Debug.LogError("targetPieces �|����l�Ʃά��šI");
            return;
        }
    }

    void Update()
    {

        HandleInput();


    }
    //--------------------------------------���ʱ���ﶵ(�w����)------------------------------------
    void HandleInput()
    {
        // ���ʿ�ܪ����Ϥ���
        if (Input.GetKeyDown(KeyCode.W))
            MoveSelection(-1);
        if (Input.GetKeyDown(KeyCode.S))
            MoveSelection(1);

        // �������Ϥ�
        if (Input.GetKeyDown(KeyCode.A))
            ChangePiece(-1);
        if (Input.GetKeyDown(KeyCode.D))
            ChangePiece(1);

        // �T�{�O�_���ϥ��T
        if (Input.GetKeyDown(KeyCode.Return))
            CheckPuzzleCompletion();
    }

    //--------------------------------------�W�U����+Call��ܱj��(�w����)------------------------------------
    void MoveSelection(int direction)
    {
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + direction, 0, slots.Count - 1);
        HighlightSelectedSlot();
    }
    //--------------------------------------�ﶵ��ܱj��(�w����)------------------------------------
    void HighlightSelectedSlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Transform outline = slots[i].transform.Find("Highlight"); // ���l���� Outline
            if (outline != null)
                outline.gameObject.SetActive(i == selectedSlotIndex);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            Transform outline = slots[i].transform.Find("Highlight1"); // ���l���� Outline1
            if (outline != null)
                outline.gameObject.SetActive(i == selectedSlotIndex);
        }
    }
    //--------------------------------------���k�ﶵ����(�w����)------------------------------------

    void ChangePiece(int direction)
    {
        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("puzzlePieces �� null �μƶq�� 0�I");
            return;
        }

        // ��� slots[selectedSlotIndex] ��e�Ϥ��b puzzlePieces ��������
        selectedPieceIndex = puzzlePieces.IndexOf(slots[selectedSlotIndex].sprite);

        if (selectedPieceIndex == -1)
        {
            Debug.LogError($"�䤣���e slots[{selectedSlotIndex}] �� sprite �b puzzlePieces ���I");
            return;
        }

        // �ھڤ�V�ܧ����
        selectedPieceIndex = (selectedPieceIndex + direction + puzzlePieces.Count) % puzzlePieces.Count;

        // ��s UI Image
        slots[selectedSlotIndex].sprite = puzzlePieces[selectedPieceIndex];

        // ��s SpriteRenderer
        SpriteRenderer spriteRenderer = slots[selectedSlotIndex].GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = puzzlePieces[selectedPieceIndex];
            spriteRenderer.color = Color.white; // �T�O���Q�z���׼v�T
        }

        Debug.Log($"�ܧ� slot[{selectedSlotIndex}]�A�s�Ϥ�: {puzzlePieces[selectedPieceIndex].name}");

        if (slots[selectedSlotIndex].sprite == null)
        {
            Debug.LogError($"slots[{selectedSlotIndex}] �� sprite �� null�A�нT�{���Ϥ����O�_���T���t�I");
            return;
        }
    }

    //--------------------------------------------------



    // �H�����ë��Ϥ��w�ä��t�� slots
    void ShufflePuzzlePieces()
    {

        if (puzzlePieces == null || puzzlePieces.Count == 0)
        {
            Debug.LogError("puzzlePieces �� null �μƶq�� 0�A�L�k�H�����áI");
            return;
        }

        // �ϥ� Fisher-Yates Shuffle �ӽT�O������
        List<Sprite> shuffledPieces = new List<Sprite>(puzzlePieces);
        for (int i = 0; i < shuffledPieces.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledPieces.Count);
            (shuffledPieces[i], shuffledPieces[randomIndex]) = (shuffledPieces[randomIndex], shuffledPieces[i]);
        }

        // ���t���ë᪺���Ϥ����� slots
        for (int i = 0; i < slots.Count && i < shuffledPieces.Count; i++)
        {
            Image img = slots[i].GetComponent<Image>();
            SpriteRenderer spriteRenderer = slots[i].GetComponentInParent<SpriteRenderer>();  // �T�O��쥿�T�� SpriteRenderer
            if (slots[i] == null)
            {
                Debug.LogError($"slots[{i}] ���j�w�I");
                continue;
            }
            if (img != null)
            {
                img.sprite = shuffledPieces[i];
                spriteRenderer.sprite = shuffledPieces[i];
                Debug.Log($"��s {slots[i].name} �� Image �� {shuffledPieces[i].name}");
                Debug.Log($"��s {slots[i].name} �� SpriteRenderer �� {shuffledPieces[i].name}");
            }
            else
            {
                Debug.LogError($"slots[{i}] �䤣�� Image �ե�I");
            }
        }

        Debug.Log("���Ϥw�H�����èä��t�� slots");
    }
    //---------------------------------------------------------------

    void CheckPuzzleCompletion()
    {
        if (slots == null || slots.Count == 0 || targetPieces == null || targetPieces.Count == 0)
        {
            Debug.LogError("slots �� targetPieces �|�����T��l�ơI");
            return;
        }

        bool isComplete = true;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null || slots[i].sprite == null)
            {
                Debug.LogError($"slots[{i}] �� null �ΨS�� sprite�I");
                isComplete = false;
                break;
            }

            if (i >= targetPieces.Count || targetPieces[i] == null)
            {
                Debug.LogError($"targetPieces[{i}] �� null�I");
                isComplete = false;
                break;
            }

            if (slots[i].sprite != targetPieces[i])
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            Debug.Log("���ϧ����I");
            ShowDialogBox(currentStage);
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
                {

                    { "dialogBox4", true },

                };
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            AdvanceStage();
        }
        else
        {
            Debug.Log("���ϩ|�������I");
        }
    }



    void ShowDialogBox(int stage)
    {
        switch (stage)
        {
            case 1: dialogBox.SetActive(true); break;
            case 2: dialogBox2.SetActive(true); break;
            case 3: dialogBox3.SetActive(true); break;
            case 4: dialogBox4.SetActive(true); break;
        }

    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)

    {
        // �ˬd�O�_�P�B����v��
        if (changedProps.ContainsKey("PlayVideo"))
        {
            bool shouldPlayVideo = (bool)changedProps["PlayVideo"];
            if (shouldPlayVideo)
            {
                PlayVideo();
            }
        }

        // �ˬd�æP�B dialogBox ���A
        if (changedProps.ContainsKey("dialogBox"))
        {
            dialogBox.SetActive((bool)changedProps["dialogBox"]);
        }
        if (changedProps.ContainsKey("dialogBox2"))
        {
            dialogBox2.SetActive((bool)changedProps["dialogBox2"]);
        }
        if (changedProps.ContainsKey("dialogBox3"))
        {
            dialogBox3.SetActive((bool)changedProps["dialogBox3"]);
        }
        if (changedProps.ContainsKey("dialogBox4"))
        {
            dialogBox4.SetActive((bool)changedProps["dialogBox4"]);
        }

    }


    void AdvanceStage()
    {
        if ( currentStage == 4)
        {
            No3.SetActive(false);
            No4.SetActive(true);
            dialogBox5.SetActive(true);
            blackend.SetActive(true);
        }
    }


    void PlayVideo()
    {
        // ����v�����{���޿�
        Debug.Log("����v����...");
    }
}
