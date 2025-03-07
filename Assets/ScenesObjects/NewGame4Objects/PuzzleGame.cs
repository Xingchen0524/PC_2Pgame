using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGame : MonoBehaviour
{
    public List<Image> slots;  // �s�񥪰����ӫ��Ϥ���
    public List<Sprite> puzzlePieces; // �i�ѿ�ܪ����ϸH��
    public List<Sprite> targetPieces; // �ؼЫ��Ϫ��U�ӸH��
    public Sprite targetImage;  // �ؼЧ���Ϥ�

    private int selectedSlotIndex = 0;  // ��e��ܪ����Ϥ������
    private int selectedPieceIndex = 0; // ��e��ܪ��Ϥ�����

    void Start()
    {
        ShufflePuzzlePieces();// �H�����ë��Ϥ�
        HighlightSelectedSlot();
    }

    void Update()
    {
        HandleInput();


    }

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

    void MoveSelection(int direction)
    {
        selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + direction, 0, slots.Count - 1);
        HighlightSelectedSlot();
    }

    void ChangePiece(int direction)
    {
        selectedPieceIndex = (selectedPieceIndex + direction + puzzlePieces.Count) % puzzlePieces.Count;

        // ��s UI Image
        slots[selectedSlotIndex].sprite = puzzlePieces[selectedPieceIndex];

        // ���է������� SpriteRenderer �æP�B�ק�
        SpriteRenderer spriteRenderer = slots[selectedSlotIndex].GetComponentInParent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = puzzlePieces[selectedPieceIndex];  // ��s SpriteRenderer
            spriteRenderer.color = Color.white; // �T�O���Q�z���׼v�T
        }

        // Debug �T�{���
        Debug.Log($"�w��� slot[{selectedSlotIndex}]�ASprite: {puzzlePieces[selectedPieceIndex].name}");
    }


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

    // �H�����ë��Ϥ��w�ä��t�� slots
    void ShufflePuzzlePieces()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image img = slots[i].GetComponent<Image>();
            SpriteRenderer spriteRenderer = slots[i].GetComponent<SpriteRenderer>();

            // �H����ܤ@�i�Ϥ�
            Sprite randomSprite = puzzlePieces[Random.Range(0, puzzlePieces.Count)];

            if (img != null)
                img.sprite = randomSprite;

            if (spriteRenderer != null)
                spriteRenderer.sprite = randomSprite;
        }

    }

        void CheckPuzzleCompletion()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].sprite != targetPieces[i])
            {
                Debug.Log("�����٨S�����I");
                return;
            }
        }
        Debug.Log("���ϧ����I");
    }
}

