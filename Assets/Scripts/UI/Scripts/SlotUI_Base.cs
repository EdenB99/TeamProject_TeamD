using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 가지는 슬롯
    /// </summary>
    InvenSlot invenSlot;

    /// <summary>
    /// 슬롯 확인용 프로퍼티
    /// </summary>
    public InvenSlot InvenSlot => invenSlot;

    /// <summary>
    /// 슬롯의 인덱스
    /// </summary>
    public uint Index => invenSlot.Index;
    
    /// <summary>
    /// 아이템의 아이콘을 표시할 UI
    /// </summary>
    Image itemIcon;

    TextMeshProUGUI itemEquip;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemEquip = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯을 초기화 하는 함수(=InvenSlot과 InvenSlotUI를 연결)
    /// </summary>
    /// <param name="slot"></param>
    public virtual void InitializeSlot(InvenSlot slot)
    {
        invenSlot = slot;
        invenSlot.onSlotItemChange = Refresh;   
        Refresh();          // 첫 화면 갱신
    }

    /// <summary>
    /// 슬롯UI의 화면 갱신
    /// </summary>
    private void Refresh()
    {
        if (InvenSlot.IsEmpty)
        {
            // 비어 있을 때
            itemIcon.color = Color.clear;   // 아이콘 투명하게
            itemIcon.sprite = null;         // 스프라이트 빼기
            itemEquip.gameObject.SetActive(false); //장착 여부와 상관없이 글씨해제
        }
        else
        {
            // 아이템이 들어있으면
            itemIcon.sprite = InvenSlot.ItemData.itemIcon;  // 스프라이트 이미지 설정
            itemIcon.color = Color.white;                   // 이미지 보이게 만들기
        }
        if (InvenSlot.IsEquipped)
        {
            itemEquip.gameObject.SetActive(true);
        }
        OnRefresh();
    }

    /// <summary>
    /// 화면 갱신할 때 자식 클래스에서 개별적으로 실행할 코드 수행용 함수
    /// </summary>
    protected virtual void OnRefresh()
    {
        // 장비 여부 표시 갱신용
    }
}
