using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : RecycleObject
{
    /// <summary>
    /// �� ������Ʈ�� ���� ������ ������
    /// </summary>
    public ItemData data = null;

    /// <summary>
    /// ��������Ʈ ( ItemData ���� �����´� )
    /// </summary>
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        data = null;
        base.OnEnable();
    }

    public void itemDel()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    /// ����Ǵ� �ڵ�

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
