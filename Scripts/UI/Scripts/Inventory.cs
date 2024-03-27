using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    /// <summary>
    /// 인벤토리 시작갯수
    /// </summary>
    const int Defualt_Size = 20;
    /// <summary>
    /// 인벤토리의 슬롯들
    /// </summary>
    InvenSlot[] slots;
    /// <summary>
    /// 인벤토리 슬롯에 접근하기 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    /// <returns>슬롯</returns>
    public InvenSlot this[uint index] => slots[index];
    /// <summary>
    /// 인벤토리 슬롯의 개수
    /// </summary>
    int SlotCount => slots.Length;
    /// <summary>
    /// 임시 슬롯(드래그나 아이템 분리작업용)
    ///</summary>
    InvenSlot tempSlot;
    /// <summary>
    /// 임시 슬롯용 인덱스
    /// </summary>
    uint tempSlotIndex = 9999;
    /// </summary>
    /// <summary>
    /// 임시 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenSlot TempSlot => tempSlot;
    /// <summary>
    /// 아이템 데이터 매니저(생성필요)
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="owner">인벤토리의 소유자</param>
    /// <param name="size">인벤토리의 슬롯 개수</param>
    public Inventory(uint size = Defualt_Size)
    {
        slots = new InvenSlot[size];
        for (uint i = 0; i < size; i++)
        {
            slots[i] = new InvenSlot(i);
        }
        tempSlot = new InvenSlot(tempSlotIndex);
        //itemDataManager = GameManager.Instance.ItemData;    // 타이밍 조심
    }
    public bool AddItem(ItemCode code)
    {
        for (int i = 0; i< SlotCount; i++)
        {
            if (AddItem(code, (uint)i))
            {
                return true;
            }
        }
        return false;
    }
     /// <summary>
    /// 인벤토리의 특정 슬롯에 특정 아이템을 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <param name="slotIndex">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>true면 추가 성공. false면 추가 실패</returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if(IsValidIndex(slotIndex)) // 인덱스가 적절한지 확인
        {
            // 적절한 인덱스면
            ItemData data = itemDataManager[code];  // 데이터 가져오기
            InvenSlot slot = slots[slotIndex];      // 슬롯 가져오기
            if(slot.IsEmpty)
            {
                // 슬롯이 비었으면
                slot.AssignSlotItem(data);          // 그대로 아이템 설정
                result = true;
            }
            else
            {
                //슬롯이 차있으면
            }
        }
        else
        {
           //인덱스가 틀렸다면
        }

        return result;
    }
    /// <summary>
    /// 인벤토리의 특정 슬롯을 비우는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 비울 슬롯</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
        }
        else
        {
           //존재하지 않는 인덱스
        }
    }
    /// <summary>
    /// 인벤토리의 슬롯을 전부 비우는 함수
    /// </summary>
    public void ClearAllInventory()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }
    /// <summary>
    /// 인벤토리의 from 슬롯에 있는 아이템을 to 위치로 옮기는 함수
    /// </summary>
    /// <param name="from">위치 변경 시작 인덱스</param>
    /// <param name="to">위치 변경 도착 인덱스</param>
    public void MoveItem(uint from, uint to)
    {
        // from지점과 to지점은 서로 다른 위치이고 모두 valid한 인덱스이어야 한다.
        if ((from != to) && IsValidIndex(from) && IsValidIndex(to))
        {
            InvenSlot fromSlot = (from == tempSlotIndex) ? TempSlot : slots[from];

            if (!fromSlot.IsEmpty)
            {
                // from에 아이템이 있다
                InvenSlot toSlot = (to == tempSlotIndex) ? TempSlot : slots[to];
                // 다른 종류의 아이템(or 비어있다) => 서로 스왑
                ItemData tempData = fromSlot.ItemData;
                bool tempEquip = fromSlot.IsEquipped;

                fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.IsEquipped);
                toSlot.AssignSlotItem(tempData, tempEquip);
                Debug.Log($"[{from}]번 슬롯과 [{to}]번 슬롯을 서로 아이템 교체");
            }
        }
    }
    /// <summary>
    /// 슬롯 스왑용 함수
    /// </summary>
    /// <param name="slotA"></param>
    /// <param name="slotB"></param>
    void SwapSlot(InvenSlot slotA, InvenSlot slotB)
    {
        ItemData tempData = slotA.ItemData;
        bool tempEquip = slotA.IsEquipped;
        slotA.AssignSlotItem(slotB.ItemData, slotB.IsEquipped);
        slotB.AssignSlotItem(tempData,  tempEquip);
    }
    /// <summary>
    /// 인벤토리의 특정 슬롯에서 아이템을 꺼내어 임시 슬롯으로 보내는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 덜어낼 슬롯</param>
    public void SetTempItem(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            TempSlot.AssignSlotItem(slot.ItemData);  // 임시 슬롯에 우선 넣고
            slots[slotIndex].ClearSlotItem(); //슬롯 데이터 삭제
        }
    }
    /// <summary>
    /// 인벤토리를 정렬하는 함수
    /// </summary>
    /// <param name="sortBy">정렬 기준</param>
    /// <param name="isAcending">true면 오름차순, false면 내림차순</param>
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        // 정렬을 위한 임시 리스트
        List<InvenSlot> temp = new List<InvenSlot>(slots);  // slots를 기반으로 리스트 생성

        // 정렬방법에 따라 임시 리스트 정렬
        switch (sortBy)
        {
            case ItemSortBy.Code:
                temp.Sort((current, other) =>       // current, y는 temp리스트에 들어있는 요소 중 2개
                {
                    if (current.ItemData == null)   // 비어있는 슬롯을 뒤쪽으로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // 오름차순/내림차순에 따라 처리
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
                temp.Sort((current, other) =>       // current, y는 temp리스트에 들어있는 요소 중 2개
                {
                    if (current.ItemData == null)   // 비어있는 슬롯을 뒤쪽으로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // 오름차순/내림차순에 따라 처리
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
                temp.Sort((current, other) =>       // current, y는 temp리스트에 들어있는 요소 중 2개
                {
                    if (current.ItemData == null)   // 비어있는 슬롯을 뒤쪽으로 보내기
                        return 1;
                    if (other.ItemData == null)
                        return -1;
                    if (isAcending)                  // 오름차순/내림차순에 따라 처리
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

        // 임시 리스트의 내용을 슬롯에 설정
        List<(ItemData, bool)> sortedData = new List<(ItemData, bool)>(SlotCount);  // 튜플 사용
        foreach (var slot in temp)
        {
            sortedData.Add((slot.ItemData, slot.IsEquipped));   // 필요 데이터만 복사해서 가지기
        }

        int index = 0;
        foreach (var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);    // 복사한 내용을 슬롯에 설정
            index++;
        }
    }
    /// <summary>
    /// 슬롯 인덱스가 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스, false면 잘못된 인덱스</returns>
    bool IsValidIndex(uint index)
    {
        return (index < SlotCount) || (index == tempSlotIndex);
    }
    /// <summary>
    /// 찾기 실패를 나타내는 상수
    /// </summary>
    const uint FailFind = uint.MaxValue;

    /// <summary>
    /// 비어있는 슬롯을 찾아 인덱스를 돌려주는 함수
    /// </summary>
    /// <param name="index">비어있는 슬롯을 찾았으면 그 슬롯의 인덱스</param>
    /// <returns>찾았으면 true, 못찾았으면 false</returns>
    public bool FindEmptySlot(out uint index)
    {
        bool result = false;
        index = FailFind;

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                index = slot.Index;
                result = true;
                break;      // 하나라도 빈 것이 나오면 루프 종료
            }
        }

        return result;
    }
}
