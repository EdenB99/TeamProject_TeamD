using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    /// <summary>
    /// �� ������Ʈ�� ���� ������ ������
    /// </summary>
    ItemData data = null;

    /// <summary>
    /// ��������Ʈ ( ItemData ���� �����´� )
    /// </summary>
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// �̹��� ����
    /// </summary>
    public ItemData ItemData
    {
        get => data;
        set
        {
            if (data == null)  
            {
                data = value;
                //������ ���� �ٲ۴�
                spriteRenderer.sprite = data.itemIcon;              
            }
        }
    }
}
