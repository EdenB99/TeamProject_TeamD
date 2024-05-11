using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot 
{
    /// <summary>
    /// 인벤토리에서 몇번째 슬롯인지를 나타내는 변수
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// 슬롯 인덱스를 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 종류(null이면 슬롯이 비어있는 것)
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// 슬롯에 들어있는 아이템 종류를 확인하기 위한 프로퍼티(쓰기는 private)
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;
        private set // 내부에서만 설정가능
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();     // 변경이 일어나면 델리게이트로 알린다.
            }
        }
    }
    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류, 개수, 장비여부의 변경을 알리는 델리게이트
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// 슬롯에 아이템이 있는지 없는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsEmpty => slotItemData == null;
    /// <summary>
    /// 이 슬롯의 아이템이 장비되었는지 여부
    /// </summary>
    bool isEquipped = false;
    /// <summary>
    /// 이 슬롯의 장비 여부를 확인하기 위한 프로퍼티(set은 private)
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
    /// 생성자
    /// </summary>
    /// <param name="index">슬롯의 인덱스(인벤토리에서의 위치)</param>
    public InvenSlot(uint index)
    {
        slotIndex = index;  // 생성할 때만 설정하고 절대 변하지 않아야한다.
        ItemData = null;
        IsEquipped = false;
    }
    /// <summary>
    /// 이 슬롯에 아이템을 설정(set)하는 함수
    /// </summary>
    /// <param name="data">설정할 아이템의 종류</param>
    /// <param name="isEquipped">장비 상태</param>
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
    /// 이 슬롯을 비우는 함수
    /// </summary>
    public virtual void ClearSlotItem()
    {
        ItemData = null;
        isEquipped = false;
    }
    /// <summary>
    /// 이 슬롯의 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    public void UseItem()
    {
        IUsable usable = ItemData as IUsable;   // IUsable을 상속 받았는지 확인
        if (usable != null)                     // 상속을 받았으면
        {
            if (usable.Use())            // 아이템 사용 시도
            {
                ClearSlotItem();               // 성공적으로 사용했으면 개수 1개 감소
            }
        }
    }
    /// <summary>
    /// 이 슬롯의 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    public void EquipItem(bool isequip)
    {
        isEquipped = isequip;
        onSlotItemChange?.Invoke();
    }
}
