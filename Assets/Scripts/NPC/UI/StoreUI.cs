using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreUI : MonoBehaviour
{
    public ItemCode[] allItemCode;
    ItemCode[] StoreItemCode = new ItemCode[4];

    public Transform Content;
    StoreSlot[] storeSlots;
    ItemDataManager itemDataManager;


    private void Awake()
    {

        storeSlots = Content.GetComponentsInChildren<StoreSlot>(); //상점의 있는 슬롯 초기화
    }

    private void Start()
    {
        itemDataManager = GameManager.Instance.ItemData; //아이템 매니저 초기화
        SetSlotItemData();
    }

    /// <summary>
    /// 슬롯 개수만큼 각 슬롯에 아이템 코드를 전송
    /// </summary>
    /// <param name="SlotNum">슬롯의 갯수</param>
    private void SetSlotItemData(int SlotNum = 4)
    {
        GetItemCodetoRandom(SlotNum); //슬롯의 갯수만큼 아이템 선별
        for (int i = 0; i < storeSlots.Length; i++)
        {
            Debug.Log(StoreItemCode[i]);
            ItemData itemData = itemDataManager[StoreItemCode[i]];
            storeSlots[i].SetItemData(itemData);
            //각 슬롯에 선정된 아이템 코드 입력
        }
    }

    /// <summary>
    /// 실제 사용될 StoreItemCode를 선별하는 함수
    /// </summary>
    /// <param name="Count">StoreItemCode의 길이</param>
    private void GetItemCodetoRandom(int Count)
    {
        //새로운 아이템 리스트를 선언, 미리 준비된 아이템 코드를 리스트에 전송하고,
        List<ItemCode> items = new List<ItemCode>();
        for (int i = 0; i < allItemCode.Length; i++)  items.Add(allItemCode[i]);

        //만일 준비된 아이템 코드의 수가 카운트보다 부족하다면
        if (items.Count < Count)
        {
            while (items.Count >= Count) //준비된 아이템 코드의 수가 Count보다 커질때까지
            {
                items.Add(itemDataManager.itemDatas[0].code); //전체 아이템 중에서 첫번째 아이템을 추가한다. 현재는 사과
            }
        }
        // 선별된 아이템 리스트에서 랜덤으로 상점에 추가할 아이템을 선정후 리스트에서 삭제
        for (int i = 0; i < StoreItemCode.Length; i++)
        {
            int rand = Random.Range(0, items.Count);
            StoreItemCode[i] = items[rand];
            items.RemoveAt(rand);
        }
    }
}
