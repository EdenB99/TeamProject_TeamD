using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot 
{
    /// <summary>
    /// �κ��丮���� ���° ���������� ��Ÿ���� ����
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// ���� �ε����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// �� ���Կ� ����ִ� ������ ����(null�̸� ������ ����ִ� ��)
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// ���Կ� ����ִ� ������ ������ Ȯ���ϱ� ���� ������Ƽ(����� private)
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;
        private set // ���ο����� ��������
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();     // ������ �Ͼ�� ��������Ʈ�� �˸���.
            }
        }
    }
    /// <summary>
    /// ���Կ� ����ִ� �������� ����, ����, ��񿩺��� ������ �˸��� ��������Ʈ
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// ���Կ� �������� �ִ��� ������ Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    public bool IsEmpty => slotItemData == null;
    /// <summary>
    /// �� ������ �������� ���Ǿ����� ����
    /// </summary>
    bool isEquipped = false;
    /// <summary>
    /// �� ������ ��� ���θ� Ȯ���ϱ� ���� ������Ƽ(set�� private)
    /// </summary>
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;
            onSlotItemChange?.Invoke();
        }
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="index">������ �ε���(�κ��丮������ ��ġ)</param>
    public InvenSlot(uint index)
    {
        slotIndex = index;  // ������ ���� �����ϰ� ���� ������ �ʾƾ��Ѵ�.
        ItemData = null;
        IsEquipped = false;
    }
    /// <summary>
    /// �� ���Կ� �������� ����(set)�ϴ� �Լ�
    /// </summary>
    /// <param name="data">������ �������� ����</param>
    /// <param name="isEquipped">��� ����</param>
    public void AssignSlotItem(ItemData data, bool isEquipped = false)
    {
        if (data != null)
        {
            ItemData = data;
            IsEquipped = isEquipped;
        }
        else
        {
            ClearSlotItem();
        }
    }

    /// <summary>
    /// �� ������ ���� �Լ�
    /// </summary>
    public virtual void ClearSlotItem()
    {
        ItemData = null;
        isEquipped = false;
    }
    /// <summary>
    /// �� ������ �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ȿ���� ���� ���</param>
    public void UseItem()
    {
        IUsable usable = ItemData as IUsable;   // IUsable�� ��� �޾Ҵ��� Ȯ��
        if (usable != null)                     // ����� �޾�����
        {
            if (usable.Use())            // ������ ��� �õ�
            {
                ClearSlotItem();               // ���������� ��������� ���� 1�� ����
            }
        }
    }
    /// <summary>
    /// �� ������ �������� ����ϴ� �Լ�
    /// </summary>
    /// <param name="target">�������� ����� ���</param>
    public void EquipItem(bool isequip)
    {
        isEquipped = isequip;
        onSlotItemChange?.Invoke();
    }
}
