using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    /// <summary>
    /// �κ��丮 ���۰���
    /// </summary>
    const int Defualt_Size = 20;
    /// <summary>
    /// �κ��丮�� ���Ե�
    /// </summary>
    InvenSlot[] slots;
    /// <summary>
    /// �κ��丮 ���Կ� �����ϱ� ���� �ε���
    /// </summary>
    /// <param name="index">������ �ε���</param>
    /// <returns>����</returns>
    public InvenSlot this[uint index] => slots[index];
    /// <summary>
    /// �κ��丮 ������ ����
    /// </summary>
    int SlotCount => slots.Length;
    /// <summary>
    /// �ӽ� ����(�巡�׳� ������ �и��۾���)
    ///</summary>
    InvenSlot tempSlot;
    /// <summary>
    /// �ӽ� ���Կ� �ε���
    /// </summary>
    uint tempSlotIndex = 9999;
    /// </summary>
    /// <summary>
    /// �ӽ� ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public InvenSlot TempSlot => tempSlot;
    /// <summary>
    /// ������ ������ �Ŵ���(�����ʿ�)
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// �κ��丮 ������
    /// </summary>
    /// <param name="owner">�κ��丮�� ������</param>
    /// <param name="size">�κ��丮�� ���� ����</param>
    public Inventory(uint size = Defualt_Size)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        //itemDataManager = GameManager.Instance.ItemData;    // Ÿ�̹� ����
    }
    
     /// <summary>
    /// �κ��丮�� Ư�� ���Կ� Ư�� �������� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="code">�߰��� �������� �ڵ�</param>
    /// <param name="slotIndex">�������� �߰��� ������ �ε���</param>
    /// <returns>true�� �߰� ����. false�� �߰� ����</returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if(IsValidIndex(slotIndex)) // �ε����� �������� Ȯ��
        {
            // ������ �ε�����
            ItemData data = itemDataManager[code];  // ������ ��������
            InvenSlot slot = slots[slotIndex];      // ���� ��������
            if(slot.IsEmpty)
            {
                // ������ �������
                slot.AssignSlotItem(data);          // �״�� ������ ����
                result = true;
            }
            else
            {
                Debug.Log($"������ �߰� ���� : [{slotIndex}]�� ���Կ��� �ٸ� �������� ����ֽ��ϴ�.");
            }
        }
        else
        {
            // �ε����� �߸� ������ ����
            Debug.Log($"������ �߰� ���� : [{slotIndex}]�� �߸��� �ε����Դϴ�.");
        }

        return result;
    }
    /// <summary>
    /// �κ��丮�� Ư�� ������ ���� �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ��� ����</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"���� ������ ���� ���� : [{slotIndex}]�� ���� �ε����Դϴ�.");
        }
    }
    /// <summary>
    /// �κ��丮�� ������ ���� ���� �Լ�
    /// </summary>
    public void ClearAllInventory()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    /// <summary>
    /// �κ��丮�� from ���Կ� �ִ� �������� to ��ġ�� �ű�� �Լ�
    /// </summary>
    /// <param name="from">��ġ ���� ���� �ε���</param>
    /// <param name="to">��ġ ���� ���� �ε���</param>
    public void MoveItem(uint from, uint to)
    {
        // from������ to������ ���� �ٸ� ��ġ�̰� ��� valid�� �ε����̾�� �Ѵ�.
        if ((from != to) && IsValidIndex(from) && IsValidIndex(to))
        {
            InvenSlot fromSlot = (from == tempSlotIndex) ? TempSlot : slots[from];

            if (!fromSlot.IsEmpty)
            {
                // from�� �������� �ִ�
                InvenSlot toSlot = (to == tempSlotIndex) ? TempSlot : slots[to];
                // �ٸ� ������ ������(or ����ִ�) => ���� ����
                ItemData tempData = fromSlot.ItemData;
                bool tempEquip = fromSlot.IsEquipped;

                fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.IsEquipped);
                toSlot.AssignSlotItem(tempData, tempEquip);
                Debug.Log($"[{from}]�� ���԰� [{to}]�� ������ ���� ������ ��ü");
            }
        }
    }
    /// <summary>
    /// �κ��丮�� Ư�� ���Կ��� �������� ������ �ӽ� �������� ������ �Լ�
    /// </summary>
    /// <param name="slotIndex">�������� ��� ����</param>
    public void SetTempItem(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            
            TempSlot.AssignSlotItem(slot.ItemData);  // �ӽ� ���Կ� �켱 �ְ�
            slots[slotIndex].ClearSlotItem(); //���� ������ ����
        }
    }
    /// <summary>
    /// �κ��丮�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="sortBy">���� ����</param>
    /// <param name="isAcending">true�� ��������, false�� ��������</param>
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        // ������ ���� �ӽ� ����Ʈ
        List<InvenSlot> temp = new List<InvenSlot>(slots);  // slots�� ������� ����Ʈ ����

        // ���Ĺ���� ���� �ӽ� ����Ʈ ����
        switch (sortBy)
        {
            case ItemSortBy.Code:
                temp.Sort((current, other) =>       // current, y�� temp����Ʈ�� ����ִ� ��� �� 2��
                {
                    if (current.ItemData == null)   // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // ��������/���������� ���� ó��
                    {
                        return current.ItemData.code.CompareTo(other.ItemData.code);
                    }
                    else
                    {
                        return other.ItemData.code.CompareTo(current.ItemData.code);
                    }
                });
                break;
            case ItemSortBy.Name:
                temp.Sort((current, other) =>       // current, y�� temp����Ʈ�� ����ִ� ��� �� 2��
                {
                    if (current.ItemData == null)   // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // ��������/���������� ���� ó��
                    {
                        return current.ItemData.itemName.CompareTo(other.ItemData.itemName);
                    }
                    else
                    {
                        return other.ItemData.itemName.CompareTo(current.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                temp.Sort((current, other) =>       // current, y�� temp����Ʈ�� ����ִ� ��� �� 2��
                {
                    if (current.ItemData == null)   // ����ִ� ������ �������� ������
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // ��������/���������� ���� ó��
                    {
                        return current.ItemData.price.CompareTo(other.ItemData.price);
                    }
                    else
                    {
                        return other.ItemData.price.CompareTo(current.ItemData.price);
                    }
                });
                break;
        }

        // �ӽ� ����Ʈ�� ������ ���Կ� ����
        List<(ItemData, bool)> sortedData = new List<(ItemData, bool)>(SlotCount);  // Ʃ�� ���
        foreach (var slot in temp)
        {
            sortedData.Add((slot.ItemData, slot.IsEquipped));   // �ʿ� �����͸� �����ؼ� ������
        }

        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);    // ������ ������ ���Կ� ����
            index++;
        }
    }
    /// <summary>
    /// ���� �ε����� ������ �ε������� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="index">Ȯ���� �ε���</param>
    /// <returns>true�� ������ �ε���, false�� �߸��� �ε���</returns>
    bool IsValidIndex(uint index)
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }
}
