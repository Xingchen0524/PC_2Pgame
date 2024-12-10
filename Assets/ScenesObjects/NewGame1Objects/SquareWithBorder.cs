using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWithBorder : MonoBehaviour
{
    public SpriteRenderer borderRenderer; // ��ت�SpriteRenderer
    public Color borderColor = Color.black; // ����C��]�q�{���¦�^

    void Start()
    {
        // �p�G���Renderer�s�b�A�]�m�C��
        if (borderRenderer != null)
        {
            borderRenderer.color = borderColor; // �]�m����C��
        }
        else
        {
            // �p�G�S���]�m���Renderer�A�h�Ыؤ@��
            CreateBorder();
        }
    }

    // �ʺA�Ы����
    private void CreateBorder()
    {
        // �Ыؤ@�ӷs�� GameObject�A�òK�[ SpriteRenderer �Ψ�������
        GameObject border = new GameObject("Border");
        border.transform.SetParent(transform); // ����س]����e��l���l����
        border.transform.localPosition = Vector3.zero; // ��ئb��l�������

        // ����زK�[ SpriteRenderer �ե�
        borderRenderer = border.AddComponent<SpriteRenderer>();

        // �]�m����C��
        borderRenderer.color = borderColor;

        // �ϥαz�Ыت���عϹ��νs�边�����w�]�Ϲ�
        // �Ҧp�A�ϥΤ@�ӥզ⪺�x����ء]�z�i�H�ھڻݨD��������ڪ���عϤ��^
        borderRenderer.sprite = Resources.Load<Sprite>("BorderSprite"); // ���]�z�w�g�N��عϹ���bResources��󧨤�

        // �]�m��ت��ƧǼh�A����Q��L����B��
        borderRenderer.sortingOrder = 1;
    }

    // �]�m����C�⪺��k
    public void SetBorderColor(Color color)
    {
        if (borderRenderer != null)
        {
            borderRenderer.color = color; // �ʺA��������C��
        }
    }
}

